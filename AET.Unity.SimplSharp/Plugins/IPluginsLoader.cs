using System.Collections.Generic;

namespace AET.Unity.SimplSharp.Plugins {
  public interface IPluginsLoader {
    Dictionary<string, T> LoadAssemblies<T>(string filespec, string pluginFolder);
    Dictionary<string, T> LoadAssemblies<T>(string filespec);
  }
}