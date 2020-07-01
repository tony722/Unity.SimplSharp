using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AET.Unity.SimplSharp.Plugins {
  public class TestPluginsLoader : IPluginsLoader {
    private string filespec;
    private string pluginFolder;

    public Dictionary<string, T> LoadAssemblies<T>(string filespec) {
      return LoadAssemblies<T>(filespec, AppDomain.CurrentDomain.BaseDirectory);
    }

    public Dictionary<string, T> LoadAssemblies<T>(string filespec, string pluginFolder) {
      this.filespec = filespec;
      this.pluginFolder = pluginFolder;

      return GetPluginInstancesDictionary<T>();
    }

    private Dictionary<string, T> GetPluginInstancesDictionary<T>() {
      var types = GetTypesFromPluginAssemblies().FindAll(delegate(Type t) {
        var interfaceTypes = new List<Type>(t.GetInterfaces());
        return interfaceTypes.Contains(typeof(T));
      });
      var loadedAssemblies = types.Select(t => (T) Activator.CreateInstance(t)).ToList();

      return loadedAssemblies.ToDictionary(a => a.GetType().Name, a => a);
    }

    private List<Type> GetTypesFromPluginAssemblies() {
      var assemblies = LoadPluginAssemblies();
      List<Type> availableTypes = new List<Type>();
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
  }
}
