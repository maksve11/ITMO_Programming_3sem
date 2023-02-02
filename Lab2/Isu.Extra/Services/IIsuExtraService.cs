using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Models;
using Isu.Services;
using CourseNumber = Isu.Extra.Models.CourseNumber;

namespace Isu.Extra.Services;

public interface IIsuExtraService : IIsuService
{
    MegaFaculty AddMegaFaculty(string name);
    void AddGroupToMegaFaculty(ProGroup group, MegaFaculty megaFacultyName);
    ProStudent AddProStudent(string name, ProGroup group);
    ProGroup AddProGroup(GroupName name);
    StudentsFlow AddStudentsFlow(StudentFlowName name, List<Lesson> lessons);

    OGNP AddOgnp(MegaFaculty megaFacultyName, string ognpName, List<StudentsFlow> courses);
    void RegisterStudentOnOgnp(OGNP ognp, ProStudent student, StudentFlowName flowName);
    void DeleteRegistrationOnOgnp(OGNP ognp, ProStudent student, StudentFlowName flowName);
    List<StudentsFlow> FindFlowsInOgnp(OGNP ognp);
    List<StudentsFlow> FindStudentFlowsByCourseNumber(int course);
    List<ProStudent> FindStudentsWithoutOgnpByGroup(ProGroup group);
    ProGroup? FindProGroups(GroupName name);
}