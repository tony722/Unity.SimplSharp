using System.Runtime.CompilerServices;
using Crestron.SimplSharp.CrestronIO;

namespace AET.Unity.SimplSharp.FileIO {
  public class CrestronFileIO : IFileIO {
    public string ReadAllText(string fileName) {
      var file = File.OpenText(fileName);
      var contents = file.ReadToEnd();
      file.Close();
      return contents;
    }

    public bool Exists(string fileName) {
      if (fileName.IsNullOrWhiteSpace()) return false;
      return File.Exists(fileName);
    }

    public void WriteText(string fileName, string data) {
      var file = File.CreateText(fileName);
      file.Write(data);
      file.Close();
    }
  }
}