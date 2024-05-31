using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AET.Unity.SimplSharp.FileIO {
  public class TestInMemoryFileIO : IFileIO {
    public TestInMemoryFileIO() {
      Files = new Dictionary<string, string>();
    }
    public Dictionary<string, string> Files { get; private set;} 
    public string ReadAllText(string fileName) { return Files[fileName];  }
    public void WriteText(string fileName, string data) { Files[fileName] = data; }
    public void WriteText(string fileName, string data, bool useVersioning) { Files[fileName] = data; }
    public bool Exists(string fileName) { return fileName != null || Files.Keys.Contains(fileName); }
  }
}
