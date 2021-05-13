using System;

namespace AET.Unity.SimplSharp {
  public static class ErrorMessage {
    static ErrorMessage() {
      ErrorMessageHandler = new CrestronErrorMessageHandler();
    }

    public static IErrorMessageHandler ErrorMessageHandler { get; set; }

    public static void Clear() {
      LastErrorMessage = string.Empty;
      LastErrorMessageType = string.Empty;
    }

    public static string LastErrorMessageType { get; private set; }
    public static string LastErrorMessage { get; private set; }

    public static void Error(string messageFormat, params object[] arg) {
      var message = string.Format(messageFormat, arg);
      ErrorMessageHandler.Error(message);
      LastErrorMessageType = "Error";
      LastErrorMessage = message;
    }

    public static bool ErrorIf(bool condition, string messageFormat, params object[] arg) {
      if (!condition) return false;
      Error(messageFormat, arg);
      return true;
    }

    public static void Notice(string messageFormat, params object[] arg) {
      var message = string.Format(messageFormat, arg);
      ErrorMessageHandler.Notice(message);
      LastErrorMessageType = "Notice";
      LastErrorMessage = string.Format(message, arg);
    }

    public static bool NoticeIf(bool condition, string messageFormat, params object[] arg) {
      if (!condition) return false;
      Notice(messageFormat, arg);
      return true;
    }
    public static void Warn(string messageFormat, params object[] arg) {
      var message = string.Format(messageFormat, arg);
      ErrorMessageHandler.Warn(message);
      LastErrorMessageType = "Warn";
      LastErrorMessage = string.Format(message, arg);
    }
    public static bool WarnIf(bool condition, string messageFormat, params object[] arg) {
      if (!condition) return false;
      Warn(messageFormat, arg);
      return true;
    }

  }
}