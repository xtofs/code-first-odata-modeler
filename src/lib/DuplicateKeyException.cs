using System.Runtime.Serialization;

namespace modeling
{
    [Serializable]
    internal class DuplicateKeyException : Exception
    {
        private ArgumentException aex;

        public DuplicateKeyException(ArgumentException aex)
        {
            this.aex = aex;
        }
    }
}