using System.Text.Json.Serialization;

namespace modeling;

public sealed record Property(string Name, string Type, bool isNavigation) : IKeyed<string>
{
    public bool IsKey { get; init; }

    public bool IsMultiValue { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)] public string Key => Name;
}
