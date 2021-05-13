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
  }
}
