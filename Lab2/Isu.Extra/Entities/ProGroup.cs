using Isu.Entities;
using Isu.Extra.Tools;
using Isu.Models;

namespace Isu.Extra.Entities;

public class ProGroup
{
    private readonly List<Lesson> _lessons;
    private readonly List<ProStudent> _students;

    public ProGroup(GroupName nameofGroup)
    {
        _lessons = new List<Lesson>();
        if (string.IsNullOrWhiteSpace(nameofGroup.NameOfGroup))
            throw new IsuException("There can't be a group with that name");
        GroupName = nameofGroup;
        _students = new List<ProStudent>();
    }

    public List<Lesson> Lessons => _lessons;
    public int MaxCountStudent { get; private set; } = 25;
    public IReadOnlyList<ProStudent> ProStudents => _students.AsReadOnly();

    public GroupName GroupName { get; }

    public void AddLesson(Lesson lesson)
    {
        if (string.IsNullOrWhiteSpace(lesson.Name))
            throw new IsuException("Can't add this lesson to group");
        bool flag = _lessons.Any(tmp => tmp.IntersectsWith(lesson));
        if (flag == true)
            throw new IsuException("Can't add this lesson");
        _lessons.Add(lesson);
    }

    public void AddLessons(List<Lesson> lesson)
    {
        bool flag = false;
        foreach (Lesson check in _lessons)
        {
            flag = lesson.Any(tmp => tmp.IntersectsWith(check) && !string.IsNullOrWhiteSpace(tmp.Name));
        }

        if (flag == true)
            throw new IsuException("lessons intersects");
        _lessons.AddRange(lesson);
    }

    public void AddStudent(ProStudent student)
    {
        if (_students.Count == MaxCountStudent)
            throw new IsuException("In Group there's 25 people");
        if (_students.Contains(student))
            throw new IsuException("In Group there's this student");
        _students.Add(student);
    }

    public void RemoveStudent(ProStudent student)
    {
        if (!_students.Contains(student))
            throw new IsuException("In Group there's no this student");
        _students.Remove(student);
    }
}