using Isu.Extra.Tools;

namespace Isu.Extra.Models;

public class LessonTime
{
    public LessonTime(DateTime time)
    {
        if (time.Hour < 8 || time.Hour > 20)
            throw new IsuException("Lesson can't be in this time");
        Start = time;
        End = Start.AddMinutes(90);
    }

    public DateTime Start { get; }
    public DateTime End { get; }
}