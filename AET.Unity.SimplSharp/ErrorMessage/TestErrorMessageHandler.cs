using System;
using System.Collections.Generic;

namespace AET.Unity.SimplSharp {
  public class TestErrorMessageHandler : IErrorMessageHandler {
    private Action<string, string> message;
    private List<string> messages = new List<string>();

    public List<string> Messages { get { return messages; } }
    public string LastErrorMessageType { get; private set; }
    public string LastErrorMessage { get; private set; }

    public void Clear() {
      LastErrorMessage = string.Empty;
      LastErrorMessageType = string.Empty;
      messages.Clear();
    }

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
      MessageAction(messageType, message);
    }

    public Action<string, string> MessageAction { get { return message ?? (MessageAction = AddMessage); } set { message = value; } }

    private void AddMessage(string messageType, string message) {
      LastErrorMessage = message;
      LastErrorMessageType = messageType;
      messages.Add(string.Format("{0}: {1}", messageType, message));
    }
  }
}