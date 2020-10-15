using System;
using System.Collections.Generic;

namespace AET.Unity.SimplSharp {
  public class CsvParser {
    private string line;
    private List<string> list;
    private int pos, lastChar;
    string cell;

    public IList<String> ParseLine(string lineToParse) {
      list = new List<string>();
      line = lineToParse;
      cell = string.Empty;
      if (!string.IsNullOrEmpty(line)) ParseCommas();
      return list;
    }

    private void ParseCommas() {
      lastChar = line.Length - 1;

      for (pos = 0; pos <= lastChar; pos++) {
        if (line[pos] == ',') {
          list.Add(cell);
          cell = string.Empty;
          if (pos < lastChar && line[pos + 1] == '\"') ParseQuoteDelimited();
        }
        else {
          cell += line[pos];
        }
      }

      list.Add(cell);
    }

    private void ParseQuoteDelimited() {
      pos = pos + 2;
      while (pos < lastChar && !(line[pos] == '\"' && line[pos + 1] == ',')) {
        if (line[pos] == '\"' && line[pos + 1] == '\"') pos += 1;
        cell += line[pos];
        pos++;
      }
    }
  }
}