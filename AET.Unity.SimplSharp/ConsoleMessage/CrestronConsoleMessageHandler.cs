using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp {
  public class CrestronConsoleMessageHandler : IConsoleMessageHandler {
    public void Print(string message) { CrestronConsole.Print(message); }
    public void Print(string messageFormat, params object[] arg) { CrestronConsole.Print(messageFormat, arg); }
    public void PrintLine(string message) { CrestronConsole.PrintLine(message); }
    public void PrintLine(string messageFormat, params object[] arg) { CrestronConsole.PrintLine(messageFormat, arg); }
  }
}
