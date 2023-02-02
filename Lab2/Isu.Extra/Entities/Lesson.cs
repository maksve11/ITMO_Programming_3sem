using Isu.Extra.Models;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class Lesson
{
    public Lesson(string name, Lector lector, LessonTime time, Audience audience)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new IsuException("Can't be lesson with this name");
        Name = name;
        Lector = lector;
        Time = time;
        Audience = audience;
    }

    public string Name { get; }
    public Lector Lector { get; }
    public LessonTime Time { get; }
    public Audience Audience { get; }

    public bool IntersectsWith(Lesson other)
    {
        return other.Audience == this.Audience &&
               !(Time.Start > other.Time.End || Time.End < other.Time.Start);
    }
}