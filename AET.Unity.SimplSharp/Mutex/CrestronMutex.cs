using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp {
  public class CrestronMutex : IMutex {
    private readonly object mutex = new object();

    public override void Enter() {
      CMonitor.Enter(mutex);
    }

    public override bool TryEnter() {
      return CMonitor.TryEnter(mutex);
    }

    public override void Exit() {
      try {
        CMonitor.Exit(mutex);
      }
      catch { }
    }
  }
}