using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.Reflection;
using Activator = Crestron.SimplSharp.Reflection.Activator;

namespace AET.Unity.SimplSharp.Plugins {
  public class CrestronPluginsLoader : IPluginsLoader {
    private string filespec;
    private string pluginFolder;

    public Dictionary<string, T> LoadAssemblies<T>(string filespec) {
      return LoadAssemblies<T>(GetCurrentDirectory());
    }

    public Dictionary<string, T> LoadAssemblies<T>(string filespec, string pluginFolder) {
      this.filespec = filespec;
      this.pluginFolder = pluginFolder;

      var types = GetTypesFromPluginAssemblies().FindAll(delegate(CType t) {
        var interfaceTypes = new List<CType>(t.GetInterfaces());
        return interfaceTypes.Contains(typeof(T));
      });
      var loadedAssemblies = types.Select(t => (T)Activator.CreateInstance(t)).ToList();
      return loadedAssemblies.ToDictionary(a => a.GetType().Name, a => a);
    }

    private List<CType> GetTypesFromPluginAssemblies() {
      var assemblies = LoadPluginAssemblies();
      var availableTypes = new List<CType>();
      foreach (var assembly in assemblies) {
        availableTypes.AddRange(assembly.GetTypes());
      }
      return availableTypes;
    }

    private List<Assembly> LoadPluginAssemblies() {
      var dInfo = new DirectoryInfo(pluginFolder);
      var files = dInfo.GetFiles(filespec);
      var plugInAssemblyList = new List<Assembly>();

      if (null != files) {
        foreach (var file in files) {
          plugInAssemblyList.Add(Assembly.LoadFrom(file.FullName));
        }
      }

      return plugInAssemblyList;
    }

    private string GetCurrentDirectory() {
      return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
    }
  }
}