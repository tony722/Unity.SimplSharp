using System;
using System.Collections.Generic;
using AET.Unity.SimplSharp;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Unity.SimplSharp.Tests {
  [TestClass]
  public class CsvParserTests {
    private CsvParser parser = new CsvParser();

    [TestMethod]
    public void NullString_ReturnsEmptyList() {
      parser.ParseLine(null).Should().BeEquivalentTo(new List<string>());
      parser.ParseLine("").Should().BeEquivalentTo(new List<string>());
    }

    [TestMethod]
    public void SingleCell_ReturnsCellAsSingleElementList() {
      parser.ParseLine("Hello").Should().BeEquivalentTo(new[] {"Hello"});
    }

    [TestMethod]
    public void SimpleCommaSplit_SplitsIntoMultipleCells() {
      parser.ParseLine("Hello,This,Is").Should().BeEquivalentTo(new[] {"Hello", "This", "Is"});
      parser.ParseLine(",,").Should().BeEquivalentTo(new[] { "", "", "" });
    }

    [TestMethod]
    public void CommaWithQuotesDelimiting_SplitsCorrectly() {
      parser.ParseLine("Hello,\"This\",Is").Should().BeEquivalentTo(new[] { "Hello", "This", "Is" });
      parser.ParseLine("Hello,\"This,Is\",A Test").Should().BeEquivalentTo(new[] { "Hello", "This,Is", "A Test" });
    }

    [TestMethod]
    public void DoubleQuotesInsideQuotes_SplitsCorrectly() {
      parser.ParseLine("Hello,\"\"\"This\"\",Is\",A Test").Should().BeEquivalentTo(new[] { "Hello", "\"This\",Is", "A Test" });
      parser.ParseLine("Hello,\"This\"\",Is\",A Test").Should().BeEquivalentTo(new[] { "Hello", "This\",Is", "A Test" });
      parser.ParseLine("Hello,\"This\"\",,,\"\",\"\"Is\",\"\"\"A\"\" Test\"").Should().BeEquivalentTo(new[] { "Hello", "This\",,,\",\"Is", "\"A\" Test" });
    }

    [TestMethod]
    public void CsvParserTest1() {
      var csvText = "\"Section 1 Name\",\"Name\",\"Date\",\"Value\"\r\n\"\",\"Item 1\",\"12/1/2022\",\"105.3\"\r\n\"\",\"Item 2\",\"12/13/2022\",\"99.7\"\r\n\"My 2nd Section\",\"Description\",\"\",\"\"\r\n\"\",\"Item Type A\",\"\",\"25.95\"\r\n\"\",\"Item Type B\",\"\",\"14.95\"";
      var results = CsvFileParser.Parse(csvText);
      results[0][0].Should().Be("Section 1 Name");
    }

    [TestMethod]
    public void CsvParserTest2() {
      var csvSample = ",record1 value2,val3,\"value 4\",\"testing \"\"embedded double quotes\"\"\",\"testing quoted \"\",\"\" character\", value 7,,value 9,\"testing empty \"\"\"\" embedded quotes\",\"testing a quoted value," + 
                      Environment.NewLine + Environment.NewLine + "that includes CR/LF patterns" + Environment.NewLine + Environment.NewLine + "(which we wish would never happen - but it does)\"";
      var results = CsvFileParser.Parse(csvSample);
      results[0][0].Should().BeNullOrWhiteSpace();
      results[0][1].Should().Be("record1 value2");
      results[0][2].Should().Be("val3");
      results[0][3].Should().Be("value 4");
      results[0][4].Should().Be("testing \"embedded double quotes\"");
      results[0][5].Should().Be("testing quoted \",\" character");
      results[0][6].Should().Be(" value 7");
      results[0][7].Should().BeNullOrWhiteSpace();
      results[0][8].Should().Be("value 9");
      results[0][9].Should().Be("testing empty \"\" embedded quotes");
      results[0][10].Should().Be("testing a quoted value,\r\n\r\nthat includes CR/LF patterns\r\n\r\n(which we wish would never happen - but it does)");
    }

  }
}
