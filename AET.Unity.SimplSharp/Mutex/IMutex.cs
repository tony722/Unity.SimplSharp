namespace AET.Unity.SimplSharp {
  public interface IMutex {
    void Enter();
    bool TryEnter();
    void Exit();
  }
}