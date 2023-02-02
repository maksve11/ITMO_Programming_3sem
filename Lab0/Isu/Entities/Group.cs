using Isu.Models;
using Isu.Tools;

namespace Isu.Entities
{
    public class Group
    {
        private readonly List<Student> _listOfStudents;

        public Group(GroupName nameofGroup)
        {
            _listOfStudents = new List<Student>();
            GroupName = nameofGroup ?? throw new InvalidGroupNameException("There can't be a group with that name");
        }

        public int MaxCountStudent { get; private set; } = 25;

        public GroupName GroupName { get; }

        public IReadOnlyList<Student> Students => _listOfStudents.AsReadOnly();

        public void AddStudent(Student student)
        {
            if (_listOfStudents.Count == MaxCountStudent)
                throw new InvalidCountStudentsInGroup("In Group there's 25 people");
            if (_listOfStudents.Contains(student))
                throw new InvalidStudentException("In Group there's this student");
            _listOfStudents.Add(student);
        }

        public void RemoveStudent(Student student)
        {
            if (!_listOfStudents.Contains(student))
                throw new InvalidStudentException("In Group there's no this student");
            _listOfStudents.Remove(student);
        }
    }
}