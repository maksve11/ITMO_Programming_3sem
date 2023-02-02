using Isu.Entities;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities
{
    public class ProStudent
    {
        private bool _ognpStatus;
        public ProStudent(string name, ProGroup group, int id)
        {
            Id = id;
            StudentName = name;
            if (string.IsNullOrWhiteSpace(name))
                throw new IsuException("Name value is empty");

            ProGroup = group ?? throw new IsuException("Group value is null");
            if (group.ProStudents.Contains(this))
                throw new IsuException("There's student in group yet");
            group.AddStudent(this);
            _ognpStatus = false;
            OgnpLessons = new List<Lesson>();
        }

        public int Id { get; }
        public string StudentName { get; }
        public bool OgnpStatus => _ognpStatus;
        public ProGroup ProGroup { get; private set; }

        public List<Lesson> OgnpLessons { get; set; }

        public void ChangeProGroup(ProGroup newGroup)
        {
            if (ProGroup == newGroup)
                throw new IsuException("Student in this group yet");
            if (newGroup.ProStudents.Contains(this))
                throw new IsuException("There's student in group yet");

            ProGroup.RemoveStudent(this);
            ProGroup = newGroup;
            newGroup.AddStudent(this);
        }

        public void ChangeOgnpStatus()
        {
            _ognpStatus = !_ognpStatus;
        }

        public void AddLessons(List<Lesson> lessons)
        {
            if (_ognpStatus == false)
                throw new IsuException("Can't add lessons, because this Student don't has OGNP");
            OgnpLessons = lessons;
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