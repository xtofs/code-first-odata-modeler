using System.Text.Json.Serialization;

namespace modeling;

public sealed record Member(string Name) : IKeyed<string>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)] public string Key => Name;
}
