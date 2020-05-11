using AET.Unity.SimplSharp;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.SimplSharp.Tests {
  [TestClass]
  public class AnyKeyDictionaryTests {
    [TestMethod]
    public void IntDictionary_RequestValueForKeyThatDoesNotExist_Returns0() {
      var d = new AnyIndexDictionary<string, int>();
      d["Nothing Here"].Should().Be(0);
    }

    [TestMethod]
    public void IntDictionary_RequestValueForExisingKey_CorrectValueReturned() {
      var d = new AnyIndexDictionary<string, int>();
      d["hello"] = 40;  //No add needed, auto-creates kvp
      d["hello"] = 45;  //Updates first kvp
      d["hello"].Should().Be(45);
    }

    [TestMethod]
    public void StringDictionary_RequestValueForNonExistentKey_ReturnsNull() {
      var d = new AnyIndexDictionary<string, string>();
      d["Nothing Here"].Should().BeNull();
    }
  }
}
