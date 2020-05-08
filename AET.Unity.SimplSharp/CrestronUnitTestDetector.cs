using System;

namespace AET.SimplSharp {
  //Always returns false

  public static class UnitTestDetector {
    static UnitTestDetector() {
      IsInUnitTest = false;
    }

    public static bool IsInUnitTest { get; private set; }
  }
}
