using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Lotus.Foundation.Kernel.Structures.Collections
{
    public class StaticList<T> : SynchronizedCollection<T>, IDisposable
    {
        private readonly Random _random = new Random(DateTime.UtcNow.Millisecond);
        private int _count;

        public volatile bool SafeRandomAccess = true;

        public StaticList(IList<T> collection)
            : base(collection, collection)
        {
            Count = base.Count;
        }

        public StaticList(ICollection<T> collection)
            : base(collection, collection)
        {
            Count = base.Count;
        }

        public StaticList(IOrderedEnumerable<T> collection)
            : base(collection, collection)
        {
            Count = base.Count;
        }

        public StaticList(IEnumerable<T> collection)
            : base(collection, collection)
        {
            Count = base.Count;
        }

        public StaticList(SynchronizedCollection<T> collection)
            : base(collection, collection)
        {
            Count = base.Count;
        }

        public StaticList(StaticList<T> collection)
            : base(collection, collection)
        {
            Count = base.Count;
        }

        public StaticList(RandomList<T> collection)
            : base(collection, collection)
        {
            Count = base.Count;
        }

        public StaticList(List<T> collection)
            : base(collection, collection)
        {
            Count = base.Count;
        }

        public StaticList()
            : base()
        {
            Count = 0;
        }

        public IOrderedEnumerable<T> Sort(Func<T, T> sortBy = null)
        {            
            return sortBy == null ? this.OrderBy(x => x) : this.OrderBy(sortBy);
        }

        public StaticList<T> Sorted()
        {
            return new StaticList<T>(Sort());
        }

        public IOrderedEnumerable<T> Shuffle()
        {
            return this.OrderBy(x => _random.Next());
        }
        
        public new int Count
        {
            get
            {
                return _count;
            }
            private set
            {
                _count = value;
            }
        }

        public new void Add(T item)
        {
            base.Add(item);
            Count++;
        }
        
        public void Add(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        public new void CopyTo(T[] array, int index)
        {
            CopyTo(array, index, _count);
        }

        public void CopyTo(T[] array, int index, int count)
        {
            lock (this)
            {
                if (count > _count) count = _count;
                Array.Resize(ref array, array.Length + count);
                for (var i = 0; i < count; i++)
                {
                    array[index + count] = base[index + count];
                }
            }
        }

        public void CopyTo(StaticList<T> list)
        {
            lock (this)
            {
                foreach (var item in this)
                {
                    list.Add(item);
                }
            }
        }

        public T FirstItemOrDefault(Func<T, bool> predicate)
        {
            for (var i = Count; i > 0; i--)
            {
                var item = GetByIndex(i - 1);
                if (predicate.Invoke(item)) return item;
            }
            return default(T);
        }

        public StaticList<T> Filter(Func<T, bool> predicate)
        {
            return new StaticList<T>(base.Items.Where(predicate));
        }

        public StaticList<T> Each(Action<T> action)
        {
            var count = _count;
            for (var i = count; i > 0; i--)
            {
                var item = GetByIndex(i - 1);
                action.Invoke(item);
            }
            return this;
        }

        public StaticList<T> GetRange(int index, int count)
        {
            lock (this)
            {
                if (index >= _count) return new StaticList<T>();
                var list = new StaticList<T>();
                for (int i = index; i < count; i++)
                {
                    if (i >= _count)
                    {
                        return list;
                    }
                    list.Add(this[i]);
                }
                return list;
            }
        }

        /// <summary>
        /// Safely get by index by catching exception.
        /// </summary>
        public T GetByIndex(int index, T @default = default(T), bool assert = false)
        {
            try
            {
                return this[index];
            }
            catch
            {
                if (assert)
                    throw;
                return @default;
            }
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            Count++;
        }

        public new bool Remove(T item)
        {
            if (base.Remove(item))
            {
                Count--;
                return true;
            }
            return false;
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            Count--;
        }

        public new void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            Count++;
        }

        public new void RemoveItem(int index)
        {
            base.RemoveItem(index);
            Count--;
        }

        /// <summary>
        /// Clear the collection and optionally dispose of the inner items (they MUST implement an IDisposable interface)
        /// </summary>
        /// <param name="dispose">Whether to dispose of the object.</param>
        public void Clear(bool dispose = false)
        {
            if (dispose)
            {
                foreach (var item in this)
                {
                    try
                    {
                        var disposable = item as IDisposable;
                        disposable?.Dispose();
                    }
                    catch { }
                }
            }
            Count = 0;
            base.Clear();
        }

        public void ClearWith(Action<T, bool> action, bool dispose = false)
        {
            if (dispose)
            {
                foreach (var item in this)
                {
                    try
                    {
                        action.Invoke(item, true);
                    }
                    catch { }
                }
            }
            else
            {
                foreach (var item in this)
                {
                    try
                    {
                        action.Invoke(item, false);
                    }
                    catch { }
                }
            }
            Count = 0;
            base.Clear();
        }

        public new T this[int index]
        {
            get
            {
                if (index >= _count)
                {
                    return default(T);
                }
                return base[index];
            }
            set
            {
                if (index < _count)
                {
                    base[index] = value;
                }
            }
        }

        public override string ToString()
        {
            return ", ".Join(this);
        }

        public T[] ToArray()
        {
            var array = new T[Count];
            for (var i = 0; i < Count; i++)
            {
                array[i] = this[i];
            }
            return array;
        }

        public void Dispose()
        {
            Clear();
        }

        public void DisposeWith(Action<T, bool> action, bool withDispose = false)
        {
            foreach (var item in this)
            {
                action.Invoke(item, withDispose);
            }
            Dispose();
        }

        public void RemoveAll(Predicate<T> match)
        {
            var count = Count;
            for (var i = count; i > 0; i--)
            {
                var item = GetByIndex(i - 1);
                if (item != null && match.Invoke(item))
                {
                    Remove(item);
                }
            }
        }
    }
}
