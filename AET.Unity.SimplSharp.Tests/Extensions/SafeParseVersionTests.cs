using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;

namespace AET.Unity.SimplSharp.Tests.Extensions {
  [TestClass]
  public class SafeParseVersionTests {
    [TestMethod]
    public void NullInput_Returns0_0_0() {
      string s = null;
      s.SafeParseVersion().Should().Be(new Version(0, 0, 0));
    }

    [TestMethod]
    public void InvalidInput_Returns0_0_0() {
      "Oops".SafeParseVersion().Should().Be(new Version(0, 0, 0));
    }

    [DataTestMethod]
    [DataRow("1", "1.0.0")]
    [DataRow("1.1", "1.1.0")]
    [DataRow("1.1.1", "1.1.1")]
    [DataRow("1.1.1.1", "1.1.1")]
    public void ValidInput_ParsesCorrectly(string version, string expectedVersion) {
      version.SafeParseVersion().Should().Be(new Version(expectedVersion));
    }
  }
}
