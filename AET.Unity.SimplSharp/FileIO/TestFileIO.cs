using System.IO;

namespace AET.Unity.SimplSharp.FileIO {
  public class TestFileIO : IFileIO {
    public string ReadAllText(string fileName) {
      if (!SimulatedFileContents.IsNullOrWhiteSpace()) return SimulatedFileContents;
      return File.ReadAllText(fileName);
    }

    public string SimulatedFileContents { get; set; }

    public void WriteText(string fileName, string data) {
      if (!SimulatedFileContents.IsNullOrWhiteSpace()) SimulatedFileContents = data;
      else File.WriteAllText(fileName, data);
    }

    public void WriteText(string fileName, string data, bool useVersioning) {
      WriteText(fileName, data);
    }

    public bool Exists(string fileName) {
      if (!SimulatedFileContents.IsNullOrWhiteSpace()) return true;
      if (fileName.IsNullOrWhiteSpace()) return false;
      return File.Exists(fileName);
    }
  }
}