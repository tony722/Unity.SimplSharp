using System;
using System.Collections.Generic;

namespace AET.Unity.SimplSharp {
  public class TestConsoleMessageHandler : IConsoleMessageHandler {
    private bool PrintInProgress;
    public TestConsoleMessageHandler() {
      Messages = new List<string>();
    }
    public List<string> Messages { get; private set; }

    public void Print(string message) {
      if (PrintInProgress) Messages[Messages.Count - 1] += message;
      else Messages.Add(message);
    }
    public void Print(string messageFormat, params object[] arg) { Print(string.Format(messageFormat, arg)); }

    public void PrintLine(string message) {
      if (PrintInProgress) {
        Print(message);
        PrintInProgress = false;
      } else {
        Messages.Add(message);
      }
    }
    public void PrintLine(string messageFormat, params object[] arg) { PrintLine(string.Format(messageFormat, arg)); }
  }
}
