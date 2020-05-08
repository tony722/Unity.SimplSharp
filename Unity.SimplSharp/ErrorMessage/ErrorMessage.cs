namespace Unity.SimplSharp.ErrorMessage {
  public static class ErrorMessage {
    static ErrorMessage() {
      ErrorMessageHandler = new CrestronErrorMessageHandler();
    }
    public static IErrorMessageHandler ErrorMessageHandler { get; set; }

    public static string LastErrorMessageType { get; private set; }
    public static string LastErrorMessage { get; private set; }

    public static void Error(string messageFormat, params object[] arg) {
      var message = string.Format(messageFormat, arg);
      ErrorMessageHandler.Error(message);
      LastErrorMessageType = "Error";
      LastErrorMessage = message;
    }
    public static void Notice(string messageFormat, params object[] arg) {
      var message = string.Format(messageFormat, arg);
      ErrorMessageHandler.Notice(message);
      LastErrorMessageType = "Notice";
      LastErrorMessage = string.Format(message, arg);
    }
    public static void Warn(string messageFormat, params object[] arg) {
      var message = string.Format(messageFormat, arg);
      ErrorMessageHandler.Warn(message);
      LastErrorMessageType = "Warn";
      LastErrorMessage = string.Format(message, arg);
    }
  }
}
