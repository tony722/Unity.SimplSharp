﻿using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.SimplSharp;

namespace AET.SimplSharp.Tests {
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
  }
}