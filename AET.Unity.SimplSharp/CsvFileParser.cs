using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Crestron.SimplSharp.CrestronIO;

namespace AET.Unity.SimplSharp {
  public static class CsvFileParser {
    //Adapted from David Woodward: https://stackoverflow.com/questions/34132392/regular-expression-c-for-csv-by-rfc-4180/39939559#39939559

    public static List<List<string>> Parse(string csv) {
      // Same regex from before shortened to one line for brevity
      var csvParser = new Regex(
          @"(?<=\r|\n|^)(?!\r|\n|$)(?:(?:""(?<Value>(?:[^""]|"""")*)""|(?<Value>(?!"")[^,\r\n]+)|""(?<OpenValue>(?:[^""]|"""")*)(?=\r|\n|$)|(?<Value>))(?:,|(?=\r|\n|$)))+?(?:(?<=,)(?<Value>))?(?:\r\n|\r|\n|$)",
          RegexOptions.Compiled);

      var rows = new List<List<string>>();

      using (var csvReader = new StringReader(csv)) {
        var csvLine = csvReader.ReadLine();
        var recordText = new StringBuilder();
        var recordNum = 0;

        while (csvLine != null) {
          recordText.AppendLine(csvLine);
          var recordsRead = csvParser.Matches(recordText.ToString());
          Match record = null;

          for (var recordIndex = 0; recordIndex < recordsRead.Count; recordIndex++) {
            record = recordsRead[recordIndex];

            if (record.Groups["OpenValue"].Success && recordIndex == recordsRead.Count - 1) {
              // We're still trying to find the end of a muti-line value in this record
              // and it's the last of the records from this segment of the CSV.
              // If we're not still working with the initial record we started with then
              // prep the record text for the next read and break out to the read loop.
              if (recordIndex != 0)
                recordText.AppendLine(record.Value);

              break;
            }

            // Valid record found or new record started before the end could be found
            recordText = new StringBuilder();
            
            recordNum++;
            var row = new List<string>();
            rows.Add(row);

            for (var valueIndex = 0; valueIndex < record.Groups["Value"].Captures.Count; valueIndex++) {
              var c = record.Groups["Value"].Captures[valueIndex];
              if (c.Length == 0 || c.Index == record.Index || record.Value[c.Index - record.Index - 1] != '\"')
                row.Add(c.Value);
              else
                row.Add((c.Value.Replace("\"\"", "\"")));
            }

            foreach (Capture openValue in record.Groups["OpenValue"].Captures)
              ErrorMessage.Warn("Unity.GoogleSheetsReader.CsvParser R" + recordNum + ":ERROR - Open ended quoted value: " + openValue.Value);
          }

          csvLine = csvReader.ReadLine();

          if (csvLine == null && record != null) {
            recordNum++;

            //End of file - still working on an open value?
            foreach (Capture openValue in record.Groups["OpenValue"].Captures)
              ErrorMessage.Warn("Unity.GoogleSheetsReader.CsvParser R" + recordNum + ":ERROR - Open ended quoted value: " + openValue.Value);
          }

        }
      }
      return rows;
    }
  }
}
