using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sitecore;

namespace Lotus.Foundation.Kernel.Structures.Collections
{
    public class StaticQueue<T> : ConcurrentQueue<T>
    {
        private int _count;
        public new int Count => _count;
        
        public new void Enqueue([NotNull] T item)
        {
            base.Enqueue(item);
            _count++;
        }

        /// <summary>
        /// Deque and invoke a method with the resulting item - if safe, won't process if deque fails.
        /// </summary>
        public void DequeueWith([NotNull] Action<T> action, bool safe = true)
        {
            var item = TryDequeOrDefault();
            if (item != null)
                action?.Invoke(item);
        }

        public new bool TryDequeue([CanBeNull] out T item)
        {
            var result = base.TryDequeue(out item);
            if (result)
                _count--;
            return result;
        }
        
        [CanBeNull]
        public T TryDequeOrDefault([CanBeNull] T @default = default(T))
        {
            T result;
            TryDequeue(out result);
            return result;
        }
        
        public void Clear(bool dispose = false)
        {
            T item;
            while ((item = TryDequeOrDefault()) != null)
            {
                if (dispose)
                {
                    var disposable = item as IDisposable;
                    disposable?.Dispose();
                }
            }
            _count = 0;
        }
    }
}
