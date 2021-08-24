namespace modeling;

public sealed class Members : ReadOnlyKeyedCollection<string, Member>
{
    public Members(IEnumerable<Member> members) : base(members) { }
}
