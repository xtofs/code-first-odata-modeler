using System.Diagnostics.CodeAnalysis;

namespace modeling
{
    internal class ProcessingQueue<T>
    {
        private readonly Queue<T> queue;
        private readonly HashSet<T> added;

        public ProcessingQueue(IEnumerable<T> types)
        {
            this.queue = new Queue<T>(types);
            this.added = new HashSet<T>(types);
        }

        internal bool TryDequeue([MaybeNullWhen(false)] out T item)
        {
            if (queue.TryDequeue(out item))
            {
                return true;
            }
            return false;
        }
        internal void Enqueue(T item)
        {
            if (!added.Contains(item))
            {
                added.Add(item);
                queue.Enqueue(item);
            }
        }
    }
}