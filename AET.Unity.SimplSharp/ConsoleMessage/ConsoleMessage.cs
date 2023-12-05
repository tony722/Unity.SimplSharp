namespace AET.Unity.SimplSharp {
  public static class ConsoleMessage {
    static ConsoleMessage() {
      ConsoleMessageHandler = new CrestronConsoleMessageHandler();
    }

    public static IConsoleMessageHandler ConsoleMessageHandler { private get; set; }
    public static void Print(string message) { ConsoleMessageHandler.Print(message);}
    public static void Print(string messageFormat, params object[] arg) { ConsoleMessageHandler.Print(messageFormat, arg);}
    public static void PrintLine(string message) { ConsoleMessageHandler.PrintLine(message);}
    public static void PrintLine(string messageFormat, params object[] arg) { ConsoleMessageHandler.PrintLine(messageFormat, arg);}
  }
}
