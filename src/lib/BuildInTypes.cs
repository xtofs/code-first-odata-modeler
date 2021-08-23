using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace modeling;

    internal class BuildInTypes
    {
        private static OneToOneMap<Type, String> map = new OneToOneMap<Type, string> {
                {typeof(string), "String"},
                {typeof(int), "Integer"},
                {typeof(bool), "Bool"},
                {typeof(double), "Double"},
            };

        public static bool Contains(Type type) => map.Contains(type);

        public static string? Resolve(Type type) => map.TryGetValue(type, out var t) ? t : null;
        public static Type? Resolve(String type) => map.TryGetValue(type, out var t) ? t : null;
    }


    internal class OneToOneMap<T1, T2> : IEnumerable<(T1, T2)>
        where T1 : notnull
        where T2 : notnull
    {
        private readonly Dictionary<T1, T2> l2r = new Dictionary<T1, T2>();
        private readonly Dictionary<T2, T1> r2l = new Dictionary<T2, T1>();

        public void Add(T1 t1, T2 t2) { l2r.Add(t1, t2); r2l.Add(t2, t1); }


        public bool TryGetValue(T1 t1, [MaybeNullWhen(false)] out T2 t2) => l2r.TryGetValue(t1, out t2);

        public bool TryGetValue(T2 t2, [MaybeNullWhen(false)] out T1 t1) => r2l.TryGetValue(t2, out t1);

        public bool Contains(T1 t1) => l2r.ContainsKey(t1);

        public bool Contains(T2 t2) => r2l.ContainsKey(t2);

        public IEnumerator<(T1, T2)> GetEnumerator() => l2r.Select(p => (p.Key, p.Value)).GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
