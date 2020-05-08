namespace Unity.SimplSharp {
  public interface INamedListItem {
    string Name { get; set; }
  }
  public class UniqueValueNamedList<T> : UniqueValueIndexedDictionary<string, T> where T : INamedListItem, new() {
    public virtual void Add(T item) {
      base.Add(item.Name, item);
    }

    public virtual void Remove(T item) {
      Remove(item.Name);
    }

    public override void RemoveValue(T item) {
      Remove(item.Name);
    }
  }
}
