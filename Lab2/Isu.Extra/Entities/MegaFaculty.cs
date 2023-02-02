using Isu.Extra.Models;
using Isu.Extra.Tools;
using Isu.Models;

namespace Isu.Extra.Entities;

public class MegaFaculty
{
    private readonly List<ProGroup> _megaFaculty;
    private OGNP _ognp = null!;

    public MegaFaculty(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new IsuException("MegaFaculty can't be with this name");
        Name = name;
        _megaFaculty = new List<ProGroup>();
    }

    public string Name { get; }

    public OGNP Ognp => _ognp;

    public IReadOnlyList<ProGroup> Groups => _megaFaculty.AsReadOnly();

    public void SetOgnp(OGNP ognp)
    {
        if (string.IsNullOrWhiteSpace(ognp.Name))
            throw new IsuException("MegaFaculty can't be with this OGNP");
        _ognp = ognp;
    }

    public ProGroup? FindGroup(GroupName name)
    {
        return _megaFaculty.FirstOrDefault(tmp => tmp.GroupName == name) ?? null;
    }

    public void AddGroupToFaculty(ProGroup group)
    {
        if (FindGroup(group.GroupName) != null)
            throw new IsuException("this group there's in this megafaculty yet");
        _megaFaculty.Add(group);
    }

    public void RemoveGroupFromFaculty(ProGroup group)
    {
        if (FindGroup(group.GroupName) == null)
            throw new IsuException("this group there are not in this megafaculty");
        _megaFaculty.Remove(group);
    }

    public string GetFaculty()
    {
        switch (_ognp.GetFlow())
        {
            case 'M' or 'K' or 'J':
                return "TINT";
            case 'R' or 'P' or 'N' or 'H':
                return "CTU";
            case 'U':
                return "FTMI";
            case 'G' or 'T' or 'O':
                return "BTINS";
            case 'L' or 'Z' or 'V':
                return "FT";
            default:
                return string.Empty;
        }
    }
}