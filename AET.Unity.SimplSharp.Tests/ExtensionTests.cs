using System.Diagnostics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AET.Unity.SimplSharp;

namespace Unity.SimplSharp.Tests {
  [TestClass]
  public class ExtensionTests {
    [TestMethod]
    public void IsNullOrWhiteSpace_Null_ReturnsTrue() {
      string t = null;
      t.IsNullOrWhiteSpace().Should().BeTrue();
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_EmptyString_RetunsTrue() {
      string t = "";
      t.IsNullOrWhiteSpace().Should().BeTrue();
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_MultipleSpacesAndTabs_ReturnsTrue() {
      string t = "\t  \n ";
      t.IsNullOrWhiteSpace().Should().BeTrue();
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_Text_ReturnsFalse() {
      string t = "hello world";
      t.IsNullOrWhiteSpace().Should().BeFalse();
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
  }
}
