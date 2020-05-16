using System;
using System.Collections.Generic;
using AET.Unity.SimplSharp.Plugins;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AET.Unity.SimplSharp.PluginTesters;

namespace Unity.SimplSharp.Tests {

  [TestClass]
  public class PluginsTests {
    private TestPlugin1 tmp;
    private static Dictionary<string, IPluginTester> plugins;

    [ClassInitialize]
    public static void ClassInit(TestContext unusedTestContext) {
      var loader = new TestPluginsLoader();
      plugins = loader.LoadAssemblies<IPluginTester>("AET.Unity.SimplSharp.Plugin*.dll");

    }

    [TestMethod]
    public void TestPluginsLoader_LoadsTestPlugins() {
      plugins.Count.Should().Be(2);
      plugins.Keys.Should().Contain("TestPlugin1");
      plugins.Keys.Should().Contain("TestPlugin2");
      Config.TestPlugin2.Should().Be(plugins["TestPlugin2"]);
    }

    [TestMethod]
    public void Plugin1_StaticMethodsWork() {
      TestPlugin1 plugin = (TestPlugin1)plugins["TestPlugin1"];
      plugin.Name = "Hello World";
      var plugin1x2 = new TestPlugin1();
      plugin1x2.Name.Should().Be("Hello World");
    }
  }
}
