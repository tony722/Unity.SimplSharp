using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp.Plugins {
  public interface IPluginsLoader {
    Dictionary<string, T> LoadAssemblies<T>(string filespec, string pluginFolder);
    Dictionary<string, T> LoadAssemblies<T>(string filespec);
  }
}