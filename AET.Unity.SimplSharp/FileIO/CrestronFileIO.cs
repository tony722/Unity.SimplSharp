using Crestron.SimplSharp.CrestronIO;

namespace AET.Unity.SimplSharp.FileIO {
  public class CrestronFileIO : IFileIO {
    public string ReadAllText(string fileName) {
      var file = File.OpenText(fileName);
      string contents = file.ReadToEnd();
      file.Close();
      return contents;
    }

    public void WriteText(string fileName, string data) {
      var file = File.CreateText(fileName);
      file.Write(data);
      file.Close();
    }
  }
}