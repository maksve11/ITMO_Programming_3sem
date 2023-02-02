namespace Isu.Tools
{
    public class InvalidStudentException : IsuException
    {
        public InvalidStudentException(string message)
            : base(message)
        {
        }
    }
}
