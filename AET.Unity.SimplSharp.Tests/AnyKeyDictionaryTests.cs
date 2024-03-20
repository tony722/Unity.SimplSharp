using System.Collections.Generic;
using System.Text;
using AET.Unity.SimplSharp;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Unity.SimplSharp.Tests {
  [TestClass]
  public class AnyKeyDictionaryTests {
    [TestMethod]
    public void IntDictionary_RequestValueForKeyThatDoesNotExist_Returns0() {
      var d = new AnyKeyDictionary<string, int>();
      d["Nothing Here"].Should().Be(0);
    }

    [TestMethod]
    public void IntDictionary_RequestValueForExisingKey_CorrectValueReturned() {
      var d = new AnyKeyDictionary<string, int>();
      d["hello"] = 40;  //No add needed, auto-creates kvp
      d["hello"] = 45;  //Updates first kvp
      d["hello"].Should().Be(45);
    }

    [TestMethod]
    public void StringDictionary_RequestValueForNonExistentKey_ReturnsNull() {
      var d = new AnyKeyDictionary<string, string>();
      d["Nothing Here"].Should().BeNull();
    }
    [TestMethod]
    public void StringDictionary_RequestValueForNonExistentKey_ValueFactoryIsDefined_ReturnsNewItem() {
      var d = new AnyKeyDictionary<string, StringBuilder> {
        ValueFactory = s => new StringBuilder()
      };
      d["Nothing Here"].ShouldBeEquivalentTo(new StringBuilder());
    }

  }
}
