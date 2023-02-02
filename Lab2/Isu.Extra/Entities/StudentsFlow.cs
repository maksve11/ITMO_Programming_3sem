using Isu.Extra.Models;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class StudentsFlow
{
    private readonly List<ProStudent> _students;
    private readonly List<Lesson> _lessons;

    public StudentsFlow(StudentFlowName name, List<Lesson> lessons)
    {
        Name = name;
        if (lessons.Count == 0)
            throw new IsuException("Can't be 0 lessons");
        _lessons = lessons;
        _students = new List<ProStudent>();
    }

    public int MaxCountOfStudents { get; private set; } = 50;
    public StudentFlowName Name { get; }
    public List<Lesson> Lessons => _lessons;
    public IReadOnlyList<ProStudent> Students => _students.AsReadOnly();

    public void AddStudent(ProStudent student)
    {
        if (_students.Count == MaxCountOfStudents)
            throw new IsuException("In Group there's 50 people");
        if (_students.Contains(student))
            throw new IsuException("There's this student in flow yet");
        _students.Add(student);
    }

    public void RemoveStudent(ProStudent student)
    {
        if (!_students.Contains(student))
            throw new IsuException("There's no student in this flow");
        _students.Remove(student);
        student.OgnpLessons = new List<Lesson>();
    }

    public void AddLesson(Lesson lesson)
    {
        if (string.IsNullOrWhiteSpace(lesson.Name))
            throw new IsuException("Can't add this lesson to group");

        bool flag = _lessons.Any(tmp => tmp.IntersectsWith(lesson));
        if (flag == false)
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
}