using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AET.Unity.SimplSharp.Plugins;

namespace AET.Unity.SimplSharp.PluginTesters {

  public class TestPlugin1 : IPluginTester {
    private static string name;
    public string Name {
      get => name;
      set => name = value;
    }

    public static Action<string> MyAction { get; set; }
  }
}
