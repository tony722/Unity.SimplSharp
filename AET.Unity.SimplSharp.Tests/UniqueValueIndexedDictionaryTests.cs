using System;
using AET.Unity.SimplSharp;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.SimplSharp.Tests {
  [TestClass]
  public class UniqueValueIndexedDictionaryTests {
    readonly object testObject = new object();
    private readonly UniqueValueIndexedDictionary<string, object> dict = new UniqueValueIndexedDictionary<string, object>();

    [TestMethod]
    public void Add_DuplicateValues_ThrowsError() {            
      dict.Add("Key1", testObject);

      dict.Invoking(d => d.Add("Key2", testObject))
        .ShouldThrow<ArgumentException>()
        .WithMessage("An item with the same key has already been added.", "because we added the same value twice, even though the keys were different");
    }

    [TestMethod]
    public void Add_AfterRemoval_ItemCanBeAddedAgain() {
      dict.Add("Key1", testObject);
      dict.Remove("Key1");
      dict.Add("Key1", testObject);
      dict.Count.Should().Be(1);
    }

    [TestMethod]
    public void RemoveValue_ShouldRemove() {
      dict.Add("Key1", testObject);
      dict.RemoveValue(testObject);
      dict.Count.Should().Be(0);
    }

    [TestMethod]
    public void Get_ByIndex_ReturnsObject() {
      dict.Add("Key1", testObject);
      dict[0].Should().Be(testObject);
      dict["Key1"].Should().Be(testObject);
    }
  }
}
