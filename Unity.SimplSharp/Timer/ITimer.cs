using System;

namespace Unity.SimplSharp.Timer {
  public interface ITimer {
    Action TimerCallback { set; }
    bool IsRunning { get; }
    void Start(long timeoutMs);

    void Restart();
  }  
}