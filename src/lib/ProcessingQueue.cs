using System.Diagnostics.CodeAnalysis;

namespace modeling
{
    internal class ProcessingQueue<T>
    {
        private readonly Queue<T> queue;
        private readonly HashSet<T> processed;

        public ProcessingQueue(IEnumerable<T> types)
        {
            this.queue = new Queue<T>(types);
            this.processed = new HashSet<T>();
        }

        internal bool TryDequeue([MaybeNullWhen(false)] out T item)
        {
            if (queue.TryDequeue(out item))
            {
                processed.Add(item); return true;
            }
            return false;
        }
        internal void Enqueue(T item)
        {
            if (!processed.Contains(item))
            {
                queue.Enqueue(item);
            }
        }
    }
}