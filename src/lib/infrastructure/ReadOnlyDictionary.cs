using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace modeling;

public interface IKeyed<TK> { TK Key { get; } }

public interface IReadOnlyKeyedCollection<TK, TV> : IEnumerable<TV>
{
    IEnumerable<TK> Keys { get; }
    bool TryGetValue(TK key, [MaybeNullWhen(false)] out TV value);
}

public class ReadOnlyKeyedCollection<TK, TV> : IReadOnlyKeyedCollection<TK, TV>
    where TV : IKeyed<TK>
    where TK : notnull
{
    private readonly Dictionary<TK, TV> dictionary = new Dictionary<TK, TV>();

    public ReadOnlyKeyedCollection(IEnumerable<TV> items) : base()
    {
        try
        {
            foreach (var item in items)
            {
                dictionary.Add(item.Key, item);
            }
        }
        catch (System.ArgumentException aex) when (aex.Message.StartsWith("An item with the same key has already been added."))
        {
            var key = aex.Message.Substring(aex.Message.LastIndexOf(": ") + 2);
            throw new DuplicateKeyException(key, aex);
        }
    }

    public IEnumerable<TK> Keys => dictionary.Keys;

    public IEnumerator<TV> GetEnumerator() => dictionary.Values.GetEnumerator();

    public bool TryGetValue(TK key, [MaybeNullWhen(false)] out TV value) => dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => dictionary.Values.GetEnumerator();
}
