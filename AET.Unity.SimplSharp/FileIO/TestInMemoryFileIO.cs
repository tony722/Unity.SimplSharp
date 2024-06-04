using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crestron.SimplSharp.CrestronIO;

namespace AET.Unity.SimplSharp.FileIO {
  public class TestInMemoryFileIO : IFileIO {
    public TestInMemoryFileIO() {
      Files = new Dictionary<string, string>();
    }
    public Dictionary<string, string> Files { get; private set;}

    public string ReadAllText(string fileName) {
      try {return Files[fileName];}
      catch { throw new FileNotFoundException("Could not open file '{0}'."); }
    }
    public void WriteText(string fileName, string data) { Files[fileName] = data; }
    public void WriteText(string fileName, string data, bool useVersioning) { Files[fileName] = data; }
    public bool Exists(string fileName) { return fileName != null && Files.Keys.Contains(fileName); }
  }
}
