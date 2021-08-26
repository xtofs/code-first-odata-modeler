namespace modeling;

public static class EnumerableExtensions
{
    public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> enumerable) => enumerable.Select((v, i) => (i, v));
}