using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Isu.Models;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraTest
{
    [Fact]
    public void AddOgnp_CreateMegaFacultyAndOgnp_OgnpAdded()
    {
        var test = new IsuExtraService();
        MegaFaculty itip = test.AddMegaFaculty("ITIP");
        var courses = new List<StudentsFlow> { };
        OGNP ognp = test.AddOgnp(itip, "Матлогика", courses);
        Assert.True(itip.Ognp == ognp);
    }

    [Fact]
    public void AddStudentToOGNP_StudentHasOGNPAndContainsStudent()
    {
        var test = new IsuExtraService();
        ProGroup group = test.AddProGroup(new GroupName("M3206"));
        var lector = new Lector("Ivanov");
        var lessontime = new DateTime(2022, 09, 08, 08, 20, 00);
        var lessontime1 = new DateTime(2022, 09, 08, 10, 00, 00);
        var audience = new Audience(1234, new Building("Kronversky"));
        var audience1 = new Audience(2134, new Building("Kronversky"));
        StudentsFlow flow = test.AddStudentsFlow(
            new StudentFlowName("M7207"),
            new List<Lesson>() { new Lesson("EVM", lector, new LessonTime(lessontime), audience) });
        group.AddLesson(new Lesson("Ck", new Lector("Borshev"), new LessonTime(lessontime1), audience1));
        ProStudent student = test.AddProStudent("Maksim Velichko", group);
        MegaFaculty faculty = test.AddMegaFaculty("TINT");
        OGNP ognp = test.AddOgnp(faculty, "MatLogic", new List<StudentsFlow>() { flow });
        test.RegisterStudentOnOgnp(ognp, student, flow.Name);

        Assert.Contains(student, group.ProStudents);
        Assert.Contains(student, flow.Students);

        ProStudent student1 = test.AddProStudent("Oleg Serchenko", group);
        Assert.Contains(student1, group.ProStudents);
    }

    [Fact]
    public void DeleteStudentFromOGNP_StudentExistAndExistOgnp()
    {
        var test = new IsuExtraService();
        ProGroup group = test.AddProGroup(new GroupName("M3206"));
        var lector = new Lector("Ivanov");
        var lessontime = new DateTime(2022, 09, 08, 08, 20, 00);
        var lessontime1 = new DateTime(2022, 09, 08, 10, 00, 00);
        var audience = new Audience(1234, new Building("Kronversky"));
        var audience1 = new Audience(2134, new Building("Kronversky"));
        StudentsFlow flow = test.AddStudentsFlow(
            new StudentFlowName("M7207"),
            new List<Lesson>() { new Lesson("EVM", lector, new LessonTime(lessontime), audience) });
        group.AddLesson(new Lesson("Ck", new Lector("Borshev"), new LessonTime(lessontime1), audience1));
        ProStudent student = test.AddProStudent("Maksim Velichko", group);
        MegaFaculty faculty = test.AddMegaFaculty("TINT");
        OGNP ognp = test.AddOgnp(faculty, "MatLogic", new List<StudentsFlow>() { flow });
        test.RegisterStudentOnOgnp(ognp, student, flow.Name);
        Assert.Contains(student, flow.Students);
        test.DeleteRegistrationOnOgnp(ognp, student, flow.Name);
        Assert.True(!flow.Students.Contains(student));
    }

    [Fact]
    public void FindFlowsInOgnp_FlowsExistsAndTheyAreCorrect_FlowsFound()
    {
        var test = new IsuExtraService();
        var audiense = new Audience(403, new Building("Birzhevay"));
        ProGroup m3204 = test.AddProGroup(new GroupName("M3204"));
        var vozianova = new Lector("Vozianova Anna");
        var mayatin = new Lector("Mayatin Aleksandr");
        var lessontime = new DateTime(2022, 09, 08, 08, 20, 00);
        var lessontime1 = new DateTime(2022, 09, 08, 10, 00, 00);
        var matan = new Lesson("Матанализ", vozianova, new LessonTime(lessontime1), audiense);
        var osi = new Lesson("Операционные системы", mayatin, new LessonTime(lessontime1), audiense);
        m3204.AddLessons(new List<Lesson>() { matan, osi });

        ProStudent student = test.AddProStudent("Alex", m3204);
        MegaFaculty itip = test.AddMegaFaculty("ИТИП");
        var lessontime2 = new DateTime(2022, 09, 10, 13, 30, 00);
        var proMatan1 = new Lesson("Матанализ", vozianova, new LessonTime(lessontime2), audiense);
        var ml1 = new StudentsFlow(new StudentFlowName("M7207"), new List<Lesson>() { proMatan1 });
        var ml2 = new StudentsFlow(new StudentFlowName("M7206"), new List<Lesson>() { proMatan1 });
        var courses = new List<StudentsFlow> { ml1, ml2 };
        OGNP ognp = test.AddOgnp(itip, "Матлогика", courses);
        test.RegisterStudentOnOgnp(ognp, student, ml1.Name);

        List<StudentsFlow> result = test.FindFlowsInOgnp(ognp);
        Assert.True(result.Count == 2);
    }

    [Fact]
    public void FindStudentFlowsByCourseNumber_SituationValid_FoundStudents()
    {
        var test = new IsuExtraService();
        var audiense = new Audience(403, new Building("Birzhevay"));
        ProGroup m3204 = test.AddProGroup(new GroupName("M3204"));
        var vozianova = new Lector("Vozianova Anna");
        var mayatin = new Lector("Mayatin Aleksandr");
        var lessontime = new DateTime(2022, 09, 08, 08, 20, 00);
        var lessontime1 = new DateTime(2022, 09, 08, 10, 00, 00);

        var matan = new Lesson("Матанализ", vozianova, new LessonTime(lessontime), audiense);
        var osi = new Lesson("Операционные системы", mayatin, new LessonTime(lessontime), audiense);
        m3204.AddLessons(new List<Lesson>() { matan, osi });

        ProStudent student = test.AddProStudent("Alex", m3204);
        ProStudent student1 = test.AddProStudent("Petya", m3204);
        ProStudent student2 = test.AddProStudent("Vanya", m3204);

        var lessontime2 = new DateTime(2022, 09, 10, 13, 30, 00);
        MegaFaculty itip = test.AddMegaFaculty("ИТИП");
        var proMatan1 = new Lesson("Матанализ", vozianova, new LessonTime(lessontime2), audiense);

        var ml1 = test.AddStudentsFlow(new StudentFlowName("M7202"), new List<Lesson>() { proMatan1 });
        var ml2 = test.AddStudentsFlow(new StudentFlowName("M7205"), new List<Lesson>() { proMatan1 });
        var courses = new List<StudentsFlow> { ml1, ml2 };
        OGNP ognp = test.AddOgnp(itip, "Матлогика", courses);

        test.RegisterStudentOnOgnp(ognp, student, ml1.Name);
        test.RegisterStudentOnOgnp(ognp, student1, ml2.Name);
        test.RegisterStudentOnOgnp(ognp, student2, ml1.Name);

        List<StudentsFlow> result = test.FindStudentFlowsByCourseNumber(2);
        Assert.True(result.Count == 2);
    }

    [Fact]
    public void FindStudentsWithoutOgnpByGroup_SituationValid_FoundStudents()
    {
        var test = new IsuExtraService();
        var audiense = new Audience(403, new Building("Birzhevay"));
        ProGroup m3204 = test.AddProGroup(new GroupName("M3204"));
        var vozianova = new Lector("Vozianova Anna");
        var mayatin = new Lector("Mayatin Aleksandr");
        var lessontime = new DateTime(2022, 09, 08, 08, 20, 00);
        var lessontime1 = new DateTime(2022, 09, 08, 10, 00, 00);

        var matan = new Lesson("Матанализ", vozianova, new LessonTime(lessontime), audiense);
        var osi = new Lesson("Операционные системы", mayatin, new LessonTime(lessontime), audiense);
        m3204.AddLessons(new List<Lesson>() { matan, osi });

        ProStudent student = test.AddProStudent("Alex", m3204);
        ProStudent student1 = test.AddProStudent("Petya", m3204);
        ProStudent student2 = test.AddProStudent("Vanya", m3204);

        var lessontime2 = new DateTime(2022, 09, 10, 13, 30, 00);
        MegaFaculty itip = test.AddMegaFaculty("ИТИП");
        var proMatan1 = new Lesson("Матанализ", vozianova, new LessonTime(lessontime2), audiense);

        var ml1 = test.AddStudentsFlow(new StudentFlowName("M7202"), new List<Lesson>() { proMatan1 });
        var ml2 = test.AddStudentsFlow(new StudentFlowName("M7205"), new List<Lesson>() { proMatan1 });
        var courses = new List<StudentsFlow> { ml1, ml2 };
        OGNP ognp = test.AddOgnp(itip, "Матлогика", courses);

        test.RegisterStudentOnOgnp(ognp, student, ml1.Name);
        test.RegisterStudentOnOgnp(ognp, student1, ml2.Name);

        List<ProStudent> result = test.FindStudentsWithoutOgnpByGroup(m3204);
        Assert.True(result.Count == 1);

        test.RegisterStudentOnOgnp(ognp, student2, ml1.Name);
        List<ProStudent> result1 = test.FindStudentsWithoutOgnpByGroup(m3204);
        Assert.True(result1.Count == 0);
    }
}