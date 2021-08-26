namespace modeling;

public sealed record Service(Properties Properties)
{
    public Service(params Property[] properties) :
        this(new Properties(properties))
    { }

    public string Name { get; init; } = "Service";
}
