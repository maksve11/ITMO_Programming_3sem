using Isu.Extra.Tools;

namespace Isu.Extra.Models;

public class CourseNumber
{
    private const int MinCourseNumber = 1;
    private const int MaxCourseNumber = 6;

    public CourseNumber(int value)
    {
        if (value is < MinCourseNumber or > MaxCourseNumber)
            throw new IsuException("Course number ranging from 1 to 6");
        Year = value;
    }

    public int Year { get; }
}