using System;

namespace AET.Unity.SimplSharp {
  public class TestErrorMessageHandler : IErrorMessageHandler {

    public void Error(string message) {
      WriteErrorMessage("Error", message);
    }

    public void Notice(string message) {
      WriteErrorMessage("Notice", message);
    }

    public void Warn(string message) {
      WriteErrorMessage("Warn", message);
    }

    private void WriteErrorMessage(string messageType, string message) {
      if(Message != null) Message(messageType, message);
    }

    public Action<string, string> Message { get; set; }

  }
}
