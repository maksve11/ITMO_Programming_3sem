using Isu.Models;

namespace Isu.Extra.Models;

public class StudentFlowName : GroupName
{
    public StudentFlowName(string nameofStudentFlow)
        : base(nameofStudentFlow)
    {
        Flow = char.Parse(nameofStudentFlow[0].ToString());
    }

    public char Flow { get; }
}