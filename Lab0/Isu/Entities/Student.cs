using Isu.Models;
using Isu.Tools;

namespace Isu.Entities
{
    public class Student : IEquatable<Student>
    {
        public Student(Group group, string name, int id)
        {
            Id = id;
            StudentName = name;
            if (StudentName.Length == 0)
                throw new InvalidStudentException("Name value is empty");

            StudentGroup = group ?? throw new InvalidGroupNameException("Group value is null");
            if (group.Students.Contains(this))
                throw new InvalidStudentException("There's student in group yet");
            group.AddStudent(this);
        }

        public int Id { get; }
        public Group StudentGroup { get; private set; }

        public string StudentName { get; }

        public void ChangeGroup(Group newGroup)
        {
            if (StudentGroup == newGroup)
                throw new InvalidGroupNameException("Student in this group yet");
            if (newGroup.Students.Contains(this))
                throw new InvalidStudentException("There's student in group yet");

            StudentGroup.RemoveStudent(this);
            StudentGroup = newGroup;
            newGroup.AddStudent(this);
        }

        public bool Equals(Student? other)
        {
            if (other == null)
                return false;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StudentName, Id);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is not Student studentObj)
                return false;
            else
                return Equals(studentObj);
        }
    }
}