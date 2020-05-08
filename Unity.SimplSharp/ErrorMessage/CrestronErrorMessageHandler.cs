using Crestron.SimplSharp;

namespace Unity.SimplSharp.ErrorMessage {
  class CrestronErrorMessageHandler : IErrorMessageHandler {

    public void Error(string message) {
      ErrorLog.Error(message);
    }

    public void Notice(string message) {
      ErrorLog.Notice(message);
    }

    public void Warn(string message) {
      ErrorLog.Warn(message);
    }
  }
}
