using System.Collections.Generic;

namespace AET.Unity.SimplSharp {
  public class TestConsoleMessageHandler : IConsoleMessageHandler {
    private bool printInProgress;
    public TestConsoleMessageHandler() {
      Messages = new List<string>();
    }
    public List<string> Messages { get; private set; }

    public void Print(string message) {
      if (printInProgress) Messages[Messages.Count - 1] += message;
      else Messages.Add(message);
    }
    public void Print(string messageFormat, params object[] arg) { Print(string.Format(messageFormat, arg)); }

    public void PrintLine(string message) {
      if (printInProgress) {
        Print(message);
        printInProgress = false;
      } else {
        Messages.Add(message);
      }
    }
    public void PrintLine(string messageFormat, params object[] arg) { PrintLine(string.Format(messageFormat, arg)); }
  }
}
