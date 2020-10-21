using System;

namespace AET.Unity.SimplSharp.Timer {
  public abstract class ITimer : IDisposable {
    public abstract  Action TimerCallback { protected get; set; }
    public abstract bool IsRunning { get; protected set; }
    public abstract void Start(long timeoutMs);
    public abstract void Restart();
    public abstract void Dispose();
  }
}