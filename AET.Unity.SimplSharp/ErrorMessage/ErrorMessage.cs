using System;

namespace AET.Unity.SimplSharp {
  public enum ErrorMessageType {
    Error, Warning, Notice
  }

  public class ErrorMessageException : Exception {

    public ErrorMessageException(ErrorMessageType errorMessageType, string messageFormat, params object[] arg) : base(string.Format(string.Format(messageFormat, arg))) {
      ErrorMessageType = errorMessageType;
    }

    public ErrorMessageType ErrorMessageType { get; set; }
  }

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

    public static void HandleException(ErrorMessageException ex) {
      switch (ex.ErrorMessageType) {
        case (ErrorMessageType.Notice): {
          Notice(ex.Message);
          break;
        }
        case (ErrorMessageType.Error): {
          Error(ex.Message);
          break;
        }
        case (ErrorMessageType.Warning): {
          Warn(ex.Message);
          break;
        }
      }
    }

    public static void Error(string messageFormat, params object[] arg) {
      var message = string.Format(messageFormat, arg);
      ErrorMessageHandler.Error(message);
      LastErrorMessageType = "Error";
      LastErrorMessage = message;
    }

    public static bool ErrorIf(this bool condition, string messageFormat, params object[] arg) {
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

    public static bool NoticeIf(this bool condition, string messageFormat, params object[] arg) {
      if (!condition) return false;
      Notice(messageFormat, arg);
      return true;
    }

    public static bool NoticeIfNot(this bool condition, string messageFormat, params object[] arg) {
      return NoticeIf(!condition, messageFormat, arg);
    }

    public static void Warn(string messageFormat, params object[] arg) {
      var message = string.Format(messageFormat, arg);
      ErrorMessageHandler.Warn(message);
      LastErrorMessageType = "Warn";
      LastErrorMessage = string.Format(message, arg);
    }
    public static bool WarnIf(this bool condition, string messageFormat, params object[] arg) {
      if (!condition) return false;
      Warn(messageFormat, arg);
      return true;
    }

  }
}