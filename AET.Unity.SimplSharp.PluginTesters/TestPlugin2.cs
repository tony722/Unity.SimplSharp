using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AET.Unity.SimplSharp.Plugins;

namespace AET.Unity.SimplSharp.PluginTesters
{
    public class TestPlugin2 : IPluginTester {
      public TestPlugin2() {
        Config.TestPlugin2 = this;
        Name = "Test Plugin #2";
      }

      public string Name { get; set; }
    }
}
