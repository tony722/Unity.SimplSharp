using System.IO;

namespace AET.Unity.SimplSharp.FileIO {
  public class TestFileIO : IFileIO {
    public string ReadAllText(string fileName) {
      return File.ReadAllText(fileName);
    }

    public void WriteText(string fileName, string data) {
      File.WriteAllText(fileName, data);
    }

    public void WriteText(string fileName, string data, bool useVersioning) {
      WriteText(fileName, data);
    }

    public bool Exists(string fileName) {
      if (fileName.IsNullOrWhiteSpace()) return false;
      return File.Exists(fileName);
    }
  }
}