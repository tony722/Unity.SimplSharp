using System;
using AET.Unity.SimplSharp;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.SimplSharp.Tests {
  [TestClass]
  public class ConfigCollectionTests {

    #region Test Object Definitions
    private class ConfigCollectionTester : IdCollection<TestItem> {

    }

    private class TestItem : IdCollectionItem {

    }
    #endregion

    private ConfigCollectionTester testCollection = new ConfigCollectionTester();


    [TestMethod]
    public void SetIndexer_ItemNotAlreadyAdded_CanAddRoomConfig() {
      var item = new TestItem();
      testCollection[1] = item;
      testCollection.Count.Should().Be(1);
    }

    [TestMethod]
    public void SetIndexer_ItemAlreadyAdded_ExistingItemReplaced() {
      var item = new TestItem();
      var item2 = new TestItem();
      testCollection[1] = item;
      testCollection[1] = item2;
      testCollection.Count.Should().Be(1);
      testCollection[1].Should().Be(item2);
    }

    [TestMethod]
    public void GetIndexer_ItemHasBeenAdded_ItemReturned() {
      var rm = new TestItem();
      testCollection[1] = rm;
      testCollection[1].Should().Be(rm);
    }

    [TestMethod]
    public void GetIndexer_NoItemWithId_ReturnsNullAndLogsError() {
      testCollection[1] = new TestItem();
      var invalidItem = testCollection[2];
      ErrorMessage.LastErrorMessage.Should().Be("Tried to read TestItem config with Id 2 which has not been loaded.");
      invalidItem.Should().BeNull();
    }
  }
}
