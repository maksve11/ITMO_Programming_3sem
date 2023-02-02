namespace Isu.Tools
{
    public class InvalidCountStudentsInGroup : IsuException
    {
        public InvalidCountStudentsInGroup(string message)
            : base(message)
        {
        }
    }
}