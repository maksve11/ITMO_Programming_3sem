using Isu.Extra.Entities;
using Isu.Extra.Tools;

namespace Isu.Extra.Models;

public class OGNP
{
    private readonly List<StudentsFlow> _courses;

    public OGNP(string name, List<StudentsFlow> ognpStudents)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new IsuException("Can't be ognp course with this name");
        Name = name;
        _courses = ognpStudents;
    }

    public string Name { get; }
    public IReadOnlyList<StudentsFlow> Courses => _courses.AsReadOnly();

    public char GetFlow()
    {
        return Courses.First(tmp => true).Name.Flow;
    }
}