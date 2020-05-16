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
  }
}
