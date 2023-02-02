using Isu.Entities;
using Isu.Models;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _groups;
        private readonly IdFactory _isuIdGenerator;

        public IsuService()
        {
            _groups = new List<Group>();
            _isuIdGenerator = new IdFactory();
        }

        public Group AddGroup(GroupName name)
        {
            if (FindGroup(name) != null)
                throw new InvalidGroupNameException("There's Group with that name yet");
            var group = new Group(name);
            _groups.Add(group);
            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            if (!_groups.Contains(group))
                throw new IsuException("This group does not exist");
            int id = _isuIdGenerator.GenerateNewId();
            var student = new Student(group, name, id);
            _groups.Add(group);
            return student;
        }

        public Student GetStudent(int id)
        {
            return FindStudent(id) ?? throw new InvalidStudentException("No student with this Id");
        }

        public Student? FindStudent(int id)
        {
            return _groups.SelectMany(tmpGroup => tmpGroup.Students).FirstOrDefault(tmp => tmp.Id == id) ?? null;
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            Group? ans = _groups.FirstOrDefault(tmpGroup => tmpGroup.GroupName == groupName);
            return ans?.Students.ToList() ?? new List<Student>();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            Group? ans = _groups.FirstOrDefault(tmpGroup => tmpGroup.GroupName.CourseNumber == courseNumber);
            return ans?.Students.ToList() ?? new List<Student>();
        }

        public Group? FindGroup(GroupName groupName)
        {
            return _groups.FirstOrDefault(ans => ans.GroupName == groupName) ?? null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups.Where(tmpGroup => tmpGroup.GroupName.CourseNumber == courseNumber).ToList() ?? new List<Group>();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (FindStudent(student.Id) == null)
                throw new InvalidStudentException("Don't find student with this Id");

            if (FindGroup(newGroup.GroupName) == null)
                throw new InvalidGroupNameException("Don't find group with this Id");

            if (newGroup.Students.Count == newGroup.MaxCountStudent)
                throw new InvalidCountStudentsInGroup("In new Group there's max students");

            student.ChangeGroup(newGroup);
        }
    }
}