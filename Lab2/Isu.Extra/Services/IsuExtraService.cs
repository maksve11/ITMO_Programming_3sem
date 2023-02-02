using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Extra.Tools;
using Isu.Models;
using CourseNumber = Isu.Models.CourseNumber;

namespace Isu.Extra.Services;

public class IsuExtraService : IIsuExtraService
{
    private readonly List<MegaFaculty> _faculties;
    private readonly List<Group> _groups;
    private readonly List<ProGroup> _proGroups;
    private readonly List<StudentsFlow> _studentsFlows;
    private readonly IdFactory _isuIdGenerator;
    private readonly List<Lesson> _lessons;

    public IsuExtraService()
    {
        _faculties = new List<MegaFaculty>();
        _groups = new List<Group>();
        _isuIdGenerator = new IdFactory();
        _proGroups = new List<ProGroup>();
        _studentsFlows = new List<StudentsFlow>();
        _lessons = new List<Lesson>();
    }

    public MegaFaculty AddMegaFaculty(string name)
    {
        foreach (MegaFaculty faculty in _faculties)
        {
            if (_faculties.Count != 0 && name != faculty.Name) continue;
            throw new IsuException("MegaFaculty already exist");
        }

        var megaFaculty = new MegaFaculty(name);
        _faculties.Add(megaFaculty);
        return megaFaculty;
    }

    public void AddGroupToMegaFaculty(ProGroup group, MegaFaculty megaFacultyName)
    {
        if (string.IsNullOrWhiteSpace(megaFacultyName.Name))
            throw new IsuException("Can't add group in this megafaculty name");
        if (megaFacultyName.FindGroup(group.GroupName) != null)
            throw new IsuException("there's this group in megafaculty yet");
        megaFacultyName.AddGroupToFaculty(group);
    }

    public ProStudent AddProStudent(string name, ProGroup group)
    {
        if (!_proGroups.Contains(group))
            throw new IsuException("This ProGroup does not exist");
        int id = _isuIdGenerator.GenerateNewId();
        var student = new ProStudent(name, group, id);
        return student;
    }

    public StudentsFlow AddStudentsFlow(StudentFlowName name, List<Lesson> lessons)
    {
        if (FindStudentsFlow(name) != null)
            throw new IsuException("There's StudentFlow with that name yet");
        var studentFlow = new StudentsFlow(name, lessons);
        _studentsFlows.Add(studentFlow);
        return studentFlow;
    }

    public OGNP AddOgnp(MegaFaculty megaFacultyName, string ognpName, List<StudentsFlow> courses)
    {
        if (string.IsNullOrWhiteSpace(ognpName) || string.IsNullOrWhiteSpace(megaFacultyName.Name))
            throw new IsuException("Can't add Ognp with this name or on this MegaFaculty");
        var ognp = new OGNP(ognpName, courses);
        megaFacultyName.SetOgnp(ognp);
        return ognp;
    }

    public void RegisterStudentOnOgnp(OGNP ognp, ProStudent student, StudentFlowName flowName)
    {
        if (string.IsNullOrWhiteSpace(ognp.Name))
            throw new IsuException("Can't find this OGNP");
        StudentsFlow? ans =
            ognp.Courses.First(tmp => CheckValidGroupAndOgnpLessons(tmp.Lessons, student.ProGroup.Lessons) == false && tmp.Name == flowName);
        ans.AddStudent(student);
        student.ChangeOgnpStatus();
        student.AddLessons(ans.Lessons.ToList());
    }

    public void DeleteRegistrationOnOgnp(OGNP ognp, ProStudent student, StudentFlowName flowName)
    {
        if (string.IsNullOrWhiteSpace(ognp.Name))
            throw new IsuException("Can't find this OGNP");
        StudentsFlow? ans = ognp.Courses.FirstOrDefault(tmp => tmp.Students.Contains(student) && tmp.Name == flowName);
        if (ans != null)
        {
            ans.RemoveStudent(student);
            student.ChangeOgnpStatus();
        }
    }

    public List<StudentsFlow> FindFlowsInOgnp(OGNP ognp)
    {
        return ognp.Courses.ToList() ?? new List<StudentsFlow>();
    }

    public List<StudentsFlow> FindStudentFlowsByCourseNumber(int course)
    {
        return _studentsFlows.Where(tmp => tmp.Name.CourseNumber.Year == course).ToList() ?? new List<StudentsFlow>();
    }

    public List<ProStudent> FindStudentsWithoutOgnpByGroup(ProGroup group)
    {
        return _proGroups.SelectMany(tmp => tmp.ProStudents).Where(t => t.OgnpStatus == false).ToList() ??
               new List<ProStudent>();
    }

    public Group AddGroup(GroupName name)
    {
        if (FindGroup(name) != null)
            throw new IsuException("There's Group with that name yet");
        var group = new Group(name);
        _groups.Add(group);
        return group;
    }

    public ProGroup AddProGroup(GroupName name)
    {
        if (FindGroup(name) != null)
            throw new IsuException("There's ProGroup with that name yet");
        var group = new ProGroup(name);
        _proGroups.Add(group);
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
        return FindStudent(id) ?? throw new IsuException("No student with this Id");
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

    public StudentsFlow? FindStudentsFlow(StudentFlowName name)
    {
        return _studentsFlows.FirstOrDefault(tmp => tmp.Name == name) ?? null;
    }

    public ProGroup? FindProGroups(GroupName name)
    {
        return _proGroups.FirstOrDefault(ans => ans.GroupName == name) ?? null;
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        return _groups.Where(tmpGroup => tmpGroup.GroupName.CourseNumber == courseNumber).ToList() ?? new List<Group>();
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        if (FindStudent(student.Id) == null)
            throw new IsuException("Don't find student with this Id");

        if (FindGroup(newGroup.GroupName) == null)
            throw new IsuException("Don't find group with this Id");

        if (newGroup.Students.Count == newGroup.MaxCountStudent)
            throw new IsuException("In new Group there's max students");

        student.ChangeGroup(newGroup);
    }

    private bool CheckValidGroupAndOgnpLessons(List<Lesson> timetable1, List<Lesson> timetable2)
    {
        bool flag = false;
        foreach (Lesson l1 in timetable1)
        {
            flag = timetable2.Any(tmp => tmp.IntersectsWith(l1));
        }

        return flag;
    }
}