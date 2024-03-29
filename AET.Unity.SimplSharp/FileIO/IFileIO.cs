﻿namespace AET.Unity.SimplSharp.FileIO {
  public interface IFileIO {
    string ReadAllText(string fileName);
    void WriteText(string fileName, string data);
    void WriteText(string fileName, string data, bool useVersioning);

    bool Exists(string fileName);
  }
}