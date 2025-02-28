using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AET.Unity.SimplSharp;

namespace AET.Unity.SimplSharp.Tests {
  [TestClass]
  public class ExtensionTests {
    [TestMethod]
    public void IsNullOrWhiteSpace_Null_ReturnsTrue() {
      string t = null;
      t.IsNullOrWhiteSpace().Should().BeTrue();
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_EmptyString_RetunsTrue() {
      var t = "";
      t.IsNullOrWhiteSpace().Should().BeTrue();
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_MultipleSpacesAndTabs_ReturnsTrue() {
      var t = "\t  \n ";
      t.IsNullOrWhiteSpace().Should().BeTrue();
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_Text_ReturnsFalse() {
      var t = "hello world";
      t.IsNullOrWhiteSpace().Should().BeFalse();
    }

    [TestMethod]
    public void StripWhiteSpace_Null_ReturnsNull() {
      string t = null;
      t.StripWhiteSpace().Should().BeNull();
    }

    [TestMethod]
    public void StripWhiteSpace_EmptyString_ReturnsEmptyString() {
      var t = string.Empty;
      t.StripWhiteSpace().Should().BeEmpty();
    }

    [TestMethod]
    public void StripWhiteSpace_StringHasWhiteSpace_ReturnsStringWithoutWhitespace() {
      var t = "h\te l lo";
      t.StripWhiteSpace().Should().Be("hello");
    }

    private enum TestEnum { V1, V2 }

    [TestMethod]
    public void SafeParseEnum_NullString_ReturnsDefaultEnum() {
      var result = "".SafeParseEnum<TestEnum>();
      result.Should().Be(TestEnum.V1);
      ErrorMessage.LastErrorMessage.Should().Be("Tried to parse Enum 'TestEnum' from a null/empty value.");
    }

    [TestMethod]
    public void SafeParseEnum_ValidString_RetrunsEnumWithConnectValue() {
      var result = "V2".SafeParseEnum<TestEnum>();
      result.Should().Be(TestEnum.V2);
    }

    [TestMethod]
    public void SafeParseEnum_StringNotFound_ReturnsDefaultEnum() {
      var result = "Oops".SafeParseEnum<TestEnum>();
      result.Should().Be(TestEnum.V1);
      ErrorMessage.LastErrorMessage.Should().Be("Tried to parse Enum 'TestEnum', but value 'Oops' not found.");
    }

    [TestMethod]
    public void SafeParseInt_Null_Returns0() {
      string t = null;
      t.SafeParseInt().Should().Be(0);
    }

    [TestMethod]
    public void SafeParseInt_NonNumericString_Returns0() {
      "NotANumber".SafeParseInt().Should().Be(0);
    }

    [TestMethod]
    public void SafeParseInt_DecimalNumber_ReturnsNumberRounded() {
      "75.60".SafeParseInt().Should().Be(76);
      "50.00\r\n".SafeParseInt().Should().Be(50);
    }

    [TestMethod]
    public void SafeParseInt_NegativeNumber_ReturnsNegativeInt() {
      "-45".SafeParseInt().Should().Be(-45);
    }

    #region SafeParseBool

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("\t")]
    [DataRow(" \t \r\n ")]
    public void SafeParseBool_NullOrEmptyorWhiteSpace_ReturnsFalse(string value) {
      value.SafeParseBool().Should().BeFalse();
    }

    [DataTestMethod]
    [DataRow("true")]
    [DataRow("True")]
    [DataRow("TRUE")]
    [DataRow("1")]
    [DataRow("On")]
    [DataRow("Yes")]
    [DataRow("Y")]
    [DataRow("X")]
    public void SafeParseBool_TrueValues_ReturnsTrue(string value) {
      value.SafeParseBool().Should().BeTrue();
    }

    [DataTestMethod]
    [DataRow("true ")]
    [DataRow(" true\t\r\n")]
    public void SafeParseBool_TrueValuesWithWhiteSpace_ReturnsTrue(string value) {
      value.SafeParseBool().Should().BeTrue();
    }

    [DataTestMethod]
    [DataRow("axy")]
    [DataRow("false")]
    [DataRow("\r\nhello123\r\n")]
    [DataRow("123")]
    public void SafeParseBool_NonTrueValues_ReturnsFalse(string value) {
      value.SafeParseBool().Should().BeFalse();
    }

    #endregion
    #region FormatAsList

    [TestMethod]
    public void FormatAsList_Null_ReturnsNull() {
      string[] s = null;
      s.FormatAsList().Should().BeNull();
    }

    [TestMethod]
    public void FormatAsList_SingleElement_ReturnsElement() {
      var s = new [] {""};
      s.FormatAsList().Should().Be("");
    }

    [TestMethod]
    public void FormatAsList_TwoElements_ReturnsElementsSeparatedByOr() {
      var s = new[] {"this", "that"};
      s.FormatAsList().Should().Be("this or that");
    }

    [TestMethod]
    public void FormatAsList_MoreThanTwoElements_ReturnsElementsSeparatedByCommas() {
      var s = new[] { "this", "that", "the other" };
      s.FormatAsList().Should().Be("this, that, or the other");
    }
    #endregion

    [DataTestMethod]
    [DataRow(0,0)]
    [DataRow(1,655)]
    [DataRow(50,32767)]
    [DataRow(99,64879)]
    [DataRow(100,65535)]
    public void ConvertHundredBaseTo16Bit_ConvertsCorrectly(int value, int expected) {
      var v = (ushort)value;
      v.ConvertHundredBaseTo16Bit().Should().Be((ushort)expected);
    }

    [DataTestMethod]
    [DataRow(0, 0)]
    [DataRow(655, 1)]
    [DataRow(32767, 50)]
    [DataRow(64879, 99)]
    [DataRow(65535, 100)]
    public void Convert16BitToHundredBase_ConvertsCorrectly(int value, int expected) {
      var v = (ushort)value;
      v.Convert16BitToHundredBase().Should().Be((ushort)expected);
    }

    [TestMethod]
    public void ToSafeFileName_SanitizesCorretly() {
      var fileName = "Test/File\\A:M*A?R\"Z<Q>M|1.txt";
      fileName.ToSafeFileName().Should().Be("Test-File-A-M-A-R-Z-Q-M-1.txt");
    }

    #region SplitLines

    [TestMethod]
    public void SplitLines_NullOrEmptyString_ReturnsNullEnumeration() {
      string lines = null;
      lines.SplitLines().Should().BeEmpty();

      lines = string.Empty;
      lines.SplitLines().Should().BeEmpty();
    }

    [TestMethod]
    public void SplitLines_SplitWithCrLf_ReturnsCorrectlySplitLines() {
      var lines = "Hello\r\nThere\r\nWorld".SplitLines(); 
      lines.Should().BeEquivalentTo("Hello", "There", "World");
    }

    [TestMethod]
    public void SplitLines_SplitWithCr_ReturnsCorrectlySplitLines() {
      var lines = "Hello\rThere\rWorld".SplitLines(); 
      lines.Should().BeEquivalentTo( "Hello", "There", "World");
    }
    [TestMethod]
    public void SplitLines_SplitWithLf_ReturnsCorrectlySplitLines() {
      var lines = "Hello\nThere\nWorld\n".SplitLines(); 
      lines.Should().BeEquivalentTo( "Hello", "There", "World" );
    }
    #endregion

    [TestMethod]
    public void Base64Decode_ValidBase64String_DecodesCorrectly() {
      "SGVsbG8gV29ybGQuIDEyMzQ1Njc4OTAge31bXQ==".Base64Decode().Should().Be("Hello World. 1234567890 {}[]");
    }
  }

}
