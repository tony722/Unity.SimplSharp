using System;
using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp {
  /// <summary>
  /// An object pool to help with minimizing GC cleanup and re-allocations.
  /// </summary>
  /// <typeparam name="T">Pool object type.</typeparam>
  public sealed class ObjectPool<T> : IDisposable
      where T : class, new() {
    private readonly CCriticalSection disposeLock = new CCriticalSection();
    private readonly CrestronQueue<T> objectPool;

    private readonly CEvent queueAddEvent = new CEvent(false, true);
    private readonly CEvent queueReturnEvent = new CEvent(false, true);
    private int currentCount;
    private bool disposed;

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new object pool.
    /// </summary>
    public ObjectPool()
      : this(64) { }

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new object pool with a specified initial capacity.
    /// </summary>
    /// <param name="initialCapacity">Initial pool capacity.</param>
    public ObjectPool(int initialCapacity)
      : this(initialCapacity, -1, () => new T()) { }

    /// <summary>
    /// Initializes a new object pool with a specified initial and max capacity.
    /// </summary>
    /// <param name="initialCapacity">Initial pool capacity.</param>
    /// <param name="maxCapacity">Max pool capacity</param>
    /// <param name="initFunc">Initialization function</param>
    /// <exception cref="ArgumentException">Invalid initial or max capacity.</exception>
    public ObjectPool(int initialCapacity, int maxCapacity, Func<T> initFunc) {
      if (initialCapacity < 1)
        throw new ArgumentException("Initial capacity cannot be less than 1.");

      if (maxCapacity < 1)
        throw new ArgumentException("Max capacity cannot be less than 1.");

      if (initialCapacity > maxCapacity)
        throw new ArgumentException("Initial capacity cannot be greater than max capacity.");

      MaxCapacity = maxCapacity;
      objectPool = new CrestronQueue<T>(initialCapacity);

      if (initialCapacity > 0)
        AddToPool(initialCapacity, initFunc);
    }

    /// <summary>
    /// Max capacity of the object pool.
    /// </summary>
    /// <remarks>A max capacity of -1 indicates there is no max capacity.</remarks>
    public int MaxCapacity { get; private set; }

    /// <summary>
    /// Determines whether internal objects in the queue are disposed
    /// when this object is disposed if this pool is holding IDisposable objects.
    /// </summary>
    public bool CleanupPoolOnDispose { get; set; }

    public void Dispose() {
      Dispose(true);
    }

    /// <summary>
    /// Adds (or returns) an object to the pool.
    /// </summary>
    /// <param name="obj"></param>
    public void AddToPool(T obj) {
      if (Interlocked.Increment(ref currentCount) > MaxCapacity)
        queueReturnEvent.Wait();

      if (disposed) return;
      objectPool.Enqueue(obj);
      queueAddEvent.Set();
    }

    /// <summary>
    /// Adds a new object to the pool.
    /// </summary>
    /// <param name="initFunc">Initialization function</param>
    /// <returns>True if object could be added to the pool; otherwise false.</returns>
    public bool AddToPool(Func<T> initFunc) {
      return AddToPool(1, initFunc);
    }

    /// <summary>
    /// Adds multiple objects to the pool.
    /// </summary>
    /// <param name="count">Number of instantiated objects to add.</param>
    /// <param name="initFunc">Initialization function</param>
    /// <returns>True if objects could be added to the pool; otherwise false.</returns>
    public bool AddToPool(int count, Func<T> initFunc) {
      for (var i = 0; i < count; i++) {
        if (disposed || currentCount == MaxCapacity)
          return false;

        Interlocked.Increment(ref currentCount);
        objectPool.Enqueue(initFunc.Invoke());
        queueAddEvent.Set();
      }

      return true;
    }

    /// <summary>
    /// Retrieves an object from the pool.
    /// </summary>
    /// <returns>Pool object.</returns>
    public T GetFromPool() {
      if (currentCount == 0)
        queueAddEvent.Wait();

      if (disposed) return null;
      Interlocked.Decrement(ref currentCount);
      var obj = objectPool.Dequeue();
      queueReturnEvent.Set();
      return obj;
    }

    private void Dispose(bool disposing) {
      disposed = true;
      if (disposing) {
        queueAddEvent.Set();
        queueReturnEvent.Set();

        if (CleanupPoolOnDispose && typeof(IDisposable).IsAssignableFrom(typeof(T)))
          while (!objectPool.IsEmpty)
            ((IDisposable)objectPool.Dequeue()).Dispose();

        objectPool.Dispose();
        queueAddEvent.Dispose();
        queueReturnEvent.Dispose();
        disposeLock.Dispose();
      }
    }
  }
}