using System.Threading;

namespace AET.Unity.SimplSharp {
  public class TestMutex : IMutex {
    private readonly object mutex = new object();

    public override void Enter() {
      Monitor.Enter(mutex);
    }

    public override bool TryEnter() {
      return Monitor.TryEnter(mutex);
    }

    public override void Exit() {
      Monitor.Exit(mutex);
    }
  }
}