﻿namespace AET.Unity.SimplSharp {
  public interface IErrorMessageHandler {
    void Error(string message);
    void Warn(string message);
    void Notice(string message);
  }
}