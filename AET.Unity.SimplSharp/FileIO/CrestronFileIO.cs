using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharp.CrestronIO;

namespace AET.Unity.SimplSharp.FileIO {
  public class CrestronFileIO : IFileIO {
    public string ReadAllText(string fileName) {
      using (var file = File.OpenText(fileName)) {
        var contents = file.ReadToEnd();
        file.Close();
        return contents;
      }
    }

    public bool Exists(string fileName) {
      if (fileName.IsNullOrWhiteSpace()) return false;
      return File.Exists(fileName);
    }

    public void WriteText(string fileName, string data) {
      try {
        using (var file = File.CreateText(fileName)) WriteFile(fileName, data, file);
      } catch (Exception ex) {
        ErrorMessage.Error("WriteText({0}) Error Creating File: \r\n{1}", fileName, ex.Message);
      }
    }

    private static void WriteFile(string fileName, string data, StreamWriter file) {
      try {
        file.Write(data);
      } catch (Exception ex) {
        ErrorMessage.Error("WriteText({0}) Error Writing:\r\n{1}", fileName, ex.Message);
      } finally {
        file.Close();
      }
    }


    public void WriteText(string fileName, string data, bool useVersioning) {
      if (useVersioning) {
        VersionExistingFile(fileName);
        PurgeOldVersions(fileName);
      } else {
        DeleteExistingFile(fileName);
      }
      WriteText(fileName, data);
    }

    private void DeleteExistingFile(string fileName) {
      try {
        if (File.Exists(fileName)) File.Delete(fileName);
      } catch (Exception ex) {
        ErrorMessage.Error("Error deleting file '{0}': \r\n {1}", fileName, ex.Message);
      }
    }

    private void VersionExistingFile(string fileName) {
      try {
        if (!File.Exists(fileName)) return;
        CheckBackupDirectory(fileName);
        var fileInfo = new FileInfo(fileName);
        var newFileName = string.Format("{0}\\{1:yyyy-MM-dd_HH.mm.ss}_{2}", BackupDirectoryPath(fileName), DateTime.Now, fileInfo.Name);
        File.Move(fileName, newFileName);
      } catch (Exception ex) {
        ErrorMessage.Error("Error VersionExistingFile({0}):\r\n{1}", fileName, ex.Message);
      }
    }

    private void PurgeOldVersions(string fileName) {
      var files = GetFilesToPurge(fileName, 30).ToList();
      if (files.Count <= 2) return;
      foreach (var file in files.Skip(2)) {
        try {
          File.Delete(file); //Keep most recent file
          ErrorMessage.Notice("PurgeOldVersions() deleted '{0}'.", file);
        } catch (Exception ex) {
          ErrorMessage.Error("PurgeOldVersions({0} Error:\r\n{1}", file, ex.Message);
        }
      }
    }

    private IEnumerable<string> GetFilesToPurge(string fileName, int daysOld) {
      var files = GetBackupFiles(fileName);
      var filesToDelete = new Dictionary<DateTime, string>();
      foreach (var file in files) {
        try {
          var fileDate = File.GetCreationTime(file);
          var fileAge = DateTime.Now - fileDate;
          if (fileAge.Days > daysOld) filesToDelete.Add(fileDate, file);
        } catch (Exception ex) {
          ErrorMessage.Error("Error PurgeOldVersions({0}):\r\n{1}", file, ex.Message);
        }
      }
      return filesToDelete.OrderByDescending(kv => kv.Key).Select(kv => kv.Value);
    }

    private IEnumerable<string> GetBackupFiles(string fileName) {
      try {
        var folder = BackupDirectoryPath(fileName);
        var files = Directory.GetFiles(folder);
        return files;
      } catch (Exception ex) {
        ErrorMessage.Error("Unable to GetBackupFiles({0}):\r\n{1}", fileName, ex.Message);
        return new string[] { };
      }
    }

    private void CheckBackupDirectory(string fileName) {
      var folderName = BackupDirectoryPath(fileName);
      if (Directory.Exists(folderName)) return;
      Directory.CreateDirectory(folderName);
    }

    private string BackupDirectoryPath(string fileName) {
      return fileName + "_Backup";
    }
  }
}