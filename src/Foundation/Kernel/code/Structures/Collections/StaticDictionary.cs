﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Messaging;
using Sitecore;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Kernel.Structures.Collections
{
    public class StaticDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, IDisposable
    {
        private int _count;

        public StaticDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
            
        }

        public StaticDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : base(collection)
        {
            Count = base.Count;
        }

        public StaticDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
            : base(collection, comparer)
        {
            Count = base.Count;
        }

        public StaticDictionary(ConcurrentDictionary<TKey, TValue> collection)
            : base(collection)
        {
            Count = base.Count;
        }

        public StaticDictionary(Dictionary<TKey, TValue> collection)
            : base(collection)
        {
            Count = base.Count;
        }

        public StaticDictionary()
            : base()
        {

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

        public bool Add([NotNull] TKey key, [NotNull] TValue value)
        {
            if (base.TryAdd(key, value))
            {
                Count++;
                return true;
            }

            return false;
        }

        [NotNull]
        public new TValue GetOrAdd([NotNull] TKey key, [NotNull] Func<TKey, TValue> valueFactory)
        {
            Assert.IsNotNull(key, "{0} is required.", nameof(key));
            Assert.IsNotNull(valueFactory, "{0} is required.", nameof(valueFactory));

            while (true)
            {
                TValue value;
                if (TryGetValue(key, out value))
                {
                    return value;
                }

                value = valueFactory.Invoke(key);
                if (TryAdd(key, value))
                {
                    return value;   
                }
            }
        }

        [NotNull]
        public new TValue GetOrAdd([NotNull] TKey key, [NotNull] TValue value)
        {
            Assert.IsNotNull(key, "{0} is required.", nameof(key));

            while (true)
            {
                if (TryGetValue(key, out value))
                    return value;

                if (TryAdd(key, value))
                    return value;
            }
        }

        [NotNull]
        private new TValue AddOrUpdate([NotNull] TKey key, [NotNull] Func<TKey, TValue> addValueFactory, [NotNull] Func<TKey, TValue, TValue> updateValueFactory)
        {
            Assert.IsNotNull(key, "{0} is required.", nameof(key));
            Assert.IsNotNull(addValueFactory, "{0} is required.", nameof(addValueFactory));
            Assert.IsNotNull(updateValueFactory, "{0} is required.", nameof(updateValueFactory));

            while (true)
            {
                TValue value;
                if (TryGetValue(key, out value))
                {
                    TValue updatedValue = updateValueFactory.Invoke(key, value);
                    if (TryUpdate(key, updatedValue, value))
                    {
                        return updatedValue;
                    }
                }
                else
                {
                    value = addValueFactory.Invoke(key);
                    if (TryAdd(key, value))
                    {
                        return value;
                    }
                }
            }
        }

        [NotNull]
        public new TValue AddOrUpdate([NotNull] TKey key, [NotNull] TValue addValue, [NotNull] Func<TKey, TValue, TValue> updateValueFactory)
        {
            Assert.IsNotNull(key, "{0} is required.", nameof(key));
            Assert.IsNotNull(addValue, "{0} is required.", nameof(addValue));
            Assert.IsNotNull(updateValueFactory, "{0} is required.", nameof(updateValueFactory));

            while (true)
            {
                TValue value;
                if (TryGetValue(key, out value))
                {
                    var updatedValue = updateValueFactory.Invoke(key, value);
                    if (TryUpdate(key, updatedValue, value))
                    {
                        return updatedValue;
                    }
                }
                else
                {
                    if (TryAdd(key, addValue))
                    {
                        return value;
                    }
                }
            }
        }

        public new bool TryAdd([NotNull] TKey key, [NotNull] TValue value)
        {
            return Add(key, value);
        }

        public bool TryAddWithLock([NotNull] TKey key, [NotNull] TValue value, [NotNull] Func<TKey, TValue, bool?> func)
        {
            lock (this)
            {
                var invoked = func?.Invoke(key, value);
                return invoked.GetValueOrDefault(false) && Add(key, value);
            }
        }

        public bool Remove([NotNull] TKey key)
        {
            TValue value;
            if (base.TryRemove(key, out value))
            {
                Count--;
                return true;
            }
            return false;
        }

        [CanBeNull]
        public TValue TryRemoveOrDefault([NotNull] TKey key, [CanBeNull] TValue @default = default(TValue))
        {
            TValue value;
            return TryRemove(key, out value) ? value : @default;
        }

        public new bool TryRemove([NotNull] TKey key, [CanBeNull] out TValue item)
        {
            if (base.TryRemove(key, out item))
            {
                Count--;
                return true;
            }
            return false;
        }

        [CanBeNull]
        public TValue TryGetValueOrDefault([NotNull] TKey key, [CanBeNull] TValue @default = default(TValue))
        {
            TValue value;
            return TryGetValue(key, out value) ? value : @default;
        }

        public new bool TryGetValue([NotNull] TKey key, [NotNull] out TValue item)
        {
            return base.TryGetValue(key, out item);
        }

        public void Clear(bool dispose = false)
        {
            lock (this)
            {
                Count = 0;
                if (dispose)
                {
                    foreach (var key in base.Keys)
                    {
                        try
                        {
                            var disposable = key as IDisposable;
                            disposable?.Dispose();
                        }
                        catch { }
                    }

                    foreach (var value in base.Values)
                    {
                        try
                        {
                            var disposable = value as IDisposable;
                            disposable?.Dispose();
                        }
                        catch { }
                    }
                }
                base.Clear();
            }
        }

        public void ClearWith([NotNull] Action<KeyValuePair<TKey, TValue>, bool> action, bool dispose = false)
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

        [CanBeNull]
        public TValue GetValue([NotNull] TKey key)
        {
            TValue value;
            if (base.TryGetValue(key, out value))
            {
                return value;
            }
            return value;
        }

        public void SetValue([NotNull] TKey key, [NotNull] TValue value)
        {
            AddOrUpdate(key, value, (k, v) => value);
        }

        public new bool ContainsKey(TKey key)
        {
            TValue value;
            return TryGetValue(key, out value);
        }

        public void Dispose()
        {
            Clear();
        }
        
        [NotNull]
        public StaticDictionary<TKey, TValue> Each([NotNull] Action<TKey, TValue> action)
        {
            foreach (var pair in this)
            {
                action.Invoke(pair.Key, pair.Value);
            }
            return this;
        }

        /// <summary>
        /// Supports safe-access - will replace if key exists, otherwise will set.
        /// </summary>
        [CanBeNull]
        public new TValue this[TKey key]
        {
            get
            {
                return GetValue(key);
            }
            set
            {
                SetValue(key, value);
            }
        }

        [NotNull]
        public new ICollection<TKey> Keys
        {
            get
            {
                lock (this)
                {
                    return base.Keys;
                }
            }
        }

        [NotNull]
        public new ICollection<TValue> Values
        {
            get
            {
                lock (this)
                {
                    return base.Values;
                }
            }
        }
    }
}
