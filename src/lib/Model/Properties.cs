namespace modeling;

public sealed class Properties : ReadOnlyKeyedCollection<string, Property>
{
    public Properties(IEnumerable<Property> properties) : base(properties)
    {
    }

    public static implicit operator Properties(Property[] properties) => new Properties(properties);
}
