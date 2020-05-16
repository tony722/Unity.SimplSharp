using System;
using AET.Unity.SimplSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.SimplSharp.Tests {
  [TestClass]
  public class AssemblyInit {
    [AssemblyInitialize]
    public static void Init(TestContext unusedTestContext) {
      ErrorMessage.ErrorMessageHandler = new TestErrorMessageHandler();
    }

  }
}
