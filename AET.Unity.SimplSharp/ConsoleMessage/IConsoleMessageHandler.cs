namespace AET.Unity.SimplSharp {
  public interface IConsoleMessageHandler {
    void Print(string message);
    void Print(string messageFormat, params object[] arg);
    void PrintLine(string message);
    void PrintLine(string messageFormat, params object[] arg);
  }
}
