using System;
using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp {
  /// <summary>
  /// Wrapper for lazy initialization support (before .NET 4.0).
  /// </summary>
  /// <typeparam name="T">Object type to be lazy initialized.</typeparam>
  public sealed class Lazy<T> {
    private Box boxValue;
    private volatile bool initialized;

    [NonSerialized]
    private readonly Func<T> valueInitFunc;
    [NonSerialized]
    private readonly CCriticalSection objLock = new CCriticalSection();

    public bool Initialized {
      get {
        try {
          objLock.Enter();
          return initialized;
        } finally {
          objLock.Leave();
        }
      }
    }

    public T Value {
      get {
        if (!initialized) {
          try {
            objLock.Enter();
            if (!initialized) {
              boxValue = new Box(valueInitFunc());
              initialized = true;
            }
          } finally {
            objLock.Leave();
          }
        }

        return boxValue.Value;
      }
    }

    public Lazy()
      : this(() => (T)Activator.CreateInstance(typeof(T))) { }

    public Lazy(Func<T> valueInitFunc) {
      if (valueInitFunc == null)
        throw new ArgumentNullException("valueInitFunc");

      this.valueInitFunc = valueInitFunc;
    }

    public override string ToString() {
      return initialized ? Value.ToString() : "null";
    }

    [Serializable]
    private class Box {
      internal readonly T Value;

      internal Box(T value) {
        Value = value;
      }
    }
  }

  /// <summary>
  /// Non-generic class helper
  /// </summary>
  public sealed class Lazy {
    /// <summary>
    /// Creates a default instance of the specified type.
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    /// <returns>Lazy instance of the specified type.</returns>
    /// <remarks>The new() type constraint is placed on this little factory method
    /// as a way to still allow for types that aren't restricted by this limitation
    /// to be used by the class</remarks>
    public static Lazy<T> CreateNew<T>()
        where T : new() {
      return new Lazy<T>(Activator.CreateInstance<T>);
    }
  }
}