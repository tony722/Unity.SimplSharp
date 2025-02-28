using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp {
  public class CrestronDelay : IDelay {
    public void Sleep(int milliseconds) { CrestronEnvironment.Sleep(milliseconds); }
  }
}
