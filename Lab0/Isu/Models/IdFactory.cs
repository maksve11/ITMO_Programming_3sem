namespace Isu.Models
{
    public class IdFactory
    {
        private int _studentId;

        public IdFactory()
        {
            _studentId = 100000;
        }

        public int GenerateNewId()
        {
            return _studentId++;
        }
    }
}
