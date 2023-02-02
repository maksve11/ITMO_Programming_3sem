using Isu.Entities;
using Isu.Models;
using Isu.Services;
using Isu.Tools;
using Xunit;

namespace Isu.Test
{
    public class IsuTests
    {
        [Fact]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            var test = new IsuService();
            Group group = test.AddGroup(new GroupName("M3106"));
            Student student = test.AddStudent(group, "Maksim Velichko");

            Assert.Contains(test.GetStudent(student.Id), group.Students);

            Student student1 = test.AddStudent(group, "Oleg Serchenko");
            Assert.Contains(test.FindStudent(student1.Id), group.Students);
        }

        [Fact]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Throws<InvalidCountStudentsInGroup>(() =>
            {
                var test = new IsuService();
                Group group = test.AddGroup(new GroupName("M3202"));
                for (int i = 0; i < 40; i++)
                    test.AddStudent(group, "Nikita Oleshko");
            });
        }

        [Fact]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Throws<InvalidGroupNameException>(() =>
            {
                var unused = new Group(new GroupName("M3904"));
                var unused1 = new Group(new GroupName("Ю9999"));
            });
        }

        [Fact]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            var test = new IsuService();
            Group oldGroup = test.AddGroup(new GroupName("M3107"));
            Group newGroup = test.AddGroup(new GroupName("M3108"));
            Student student = test.AddStudent(oldGroup, "Misha Kalinov");
            test.ChangeStudentGroup(student, newGroup);

            Assert.Contains(student, newGroup.Students);
        }
    }
}