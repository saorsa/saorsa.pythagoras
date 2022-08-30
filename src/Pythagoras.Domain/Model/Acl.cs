namespace Saorsa.Pythagoras.Domain.Model;

public class Acl
{
    public string User { get; }
    public string Group { get; }
    public int UserFlags { get; }
    public int GroupFlags { get; }
    public int OtherFlags { get; }

    public Acl(string user, string group, int userFlags = 0, int groupFlags = 0, int otherFlags = 0)
    {
        User = user;
        Group = group;
        UserFlags = userFlags;
        GroupFlags = groupFlags;
        OtherFlags = otherFlags;
    }
}
