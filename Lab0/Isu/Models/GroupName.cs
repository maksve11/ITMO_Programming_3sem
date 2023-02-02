using Isu.Tools;

namespace Isu.Models
{
    public class GroupName
    {
        private const int MaxLengthOfGroupName = 5;
        private const char MinCourseNumber = '1';
        private const char MaxCourseNumber = '6';
        private const char MinCourseLetter = 'A';
        private const char MaxCourseLetter = 'Z';

        public GroupName(string nameofGroup)
        {
            if (!IsValidNameofGroup(nameofGroup)) throw new InvalidGroupNameException("There can't be a group with that name");
            NameOfGroup = nameofGroup;
            CourseNumber = new CourseNumber(int.Parse(nameofGroup[2].ToString()));
        }

        public string NameOfGroup { get; }

        public CourseNumber CourseNumber { get; }

        private static bool IsValidNameofGroup(string name)
        {
            return (name.Length == MaxLengthOfGroupName) && (name[2] >= MinCourseNumber && name[2] <= MaxCourseNumber) && (name[0] >= MinCourseLetter && name[0] <= MaxCourseLetter);
        }
    }
}
