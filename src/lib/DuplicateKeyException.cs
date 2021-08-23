using System.Runtime.Serialization;

namespace modeling
{
    [Serializable]
    internal class DuplicateKeyException : Exception
    {
        public DuplicateKeyException(string key, ArgumentException aex) : base($"duplicate Key: {key}", aex)
        {
            Key = key;
        }

        public string Key { get; }
    }
}