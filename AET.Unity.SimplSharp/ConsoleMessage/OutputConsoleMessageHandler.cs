using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AET.Unity.SimplSharp {
  /// <summary>
  /// Writes Console Messages to Output
  /// </summary>
  public class OutputConsoleMessageHandler : IConsoleMessageHandler {
    public OutputConsoleMessageHandler() {
      Trace.Listeners.Add(new ConsoleTraceListener());
    }

    public void Print(string message) {
      Trace.Write(message);
    }

    public void Print(string messageFormat, params object[] arg) {
      Trace.Write(string.Format(messageFormat, arg));
    }

    public void PrintLine(string message) {
      Trace.WriteLine(message);
    }

    public void PrintLine(string messageFormat, params object[] arg) {
      Trace.WriteLine(string.Format(messageFormat, arg));
    }
  }
}
