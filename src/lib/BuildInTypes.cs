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
