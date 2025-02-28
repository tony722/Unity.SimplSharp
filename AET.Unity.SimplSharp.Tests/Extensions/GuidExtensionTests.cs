using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;

namespace AET.Unity.SimplSharp.Tests.Extensions {
  [TestClass]
  public class GuidExtensionTests {
    [TestMethod]
    public void String_ToGuid() {
      "Hello, World!".ToGuid().Should().Be("7de2a865-7988-3828-31b6-64bd8b7f0ad4","because ToGuid should always create the same guid from a given string");
      "Testing123".ToGuid().Should().Be("648d1cac-23fd-5aae-7eac-5b7f7ffee1fa");
      "MyId".ToGuid().Should().Be("9511a22d-aab8-1872-7441-74154526677b");
      "101".ToGuid().Should().Be("f8efb338-f5ba-2766-478e-c76a704e9b52");
    }
  }
}
