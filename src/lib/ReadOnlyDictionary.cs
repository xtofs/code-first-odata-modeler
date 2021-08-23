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

    public ReadOnlyKeyedCollection(IEnumerable<TV> properties) : base()
    {
        try
        {
            foreach (var prop in properties)
            {
                dictionary.Add(prop.Key, prop);
            }
        }
        catch (System.ArgumentException aex)
        {
            throw new DuplicateKeyException(aex);
        }
    }

    public IEnumerable<TK> Keys => dictionary.Keys;

    public IEnumerator<TV> GetEnumerator() => dictionary.Values.GetEnumerator();

    public bool TryGetValue(TK key, [MaybeNullWhen(false)] out TV value) => dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => dictionary.Values.GetEnumerator();
}
