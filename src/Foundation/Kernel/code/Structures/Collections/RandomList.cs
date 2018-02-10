﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lotus.Foundation.Kernel.Structures.Collections
{
    public class RandomList<T> : StaticList<T>
    {
        private int _rateRange;
        private StaticDictionary<T, int> _rates = new StaticDictionary<T, int>();
        private Random _random = new Random(DateTime.UtcNow.Millisecond);

        public RandomList(IList<T> collection, int raterange = 0)
            : base(collection)
        {
            _rateRange = raterange;
        }

        public RandomList(ICollection<T> collection, int raterange = 0)
            : base(collection)
        {
            _rateRange = raterange;
        }

        public RandomList(IEnumerable<T> collection, int raterange = 0)
            : base(collection)
        {
            _rateRange = raterange;
        }

        public RandomList(SynchronizedCollection<T> collection, int raterange = 0)
            : base(collection)
        {
            _rateRange = raterange;
        }

        public RandomList(StaticList<T> collection, int raterange = 0)
            : base(collection)
        {
            _rateRange = raterange;
        }

        public RandomList(RandomList<T> collection, int raterange = 0)
            : base(collection)
        {
            _rateRange = raterange;
        }

        public RandomList(List<T> collection, int raterange = 0)
            : base(collection)
        {
            _rateRange = raterange;
        }

        public RandomList(int raterange = 0)
            : base()
        {
            _rateRange = raterange;
        }

        public new RandomList<T> Shuffle()
        {
            int rnum = _random.Next();
            this.OrderBy(x => rnum);
            this._rates.OrderBy(x => rnum);
            return this;
        }

        /// <summary>
        /// Called to get a single item with a rate (usecombined will get the item that the rate intercepts)
        /// </summary>
        public T GetRandomWithRate(int rate, bool usecombined = false)
        {
            T returnitem = default(T);
            if (Count == 0) return returnitem;
            if (_rateRange == 0)
            {
                return base[_random.Next(0, Count)];
            }
            else
            {
                var combinedlist = new RandomList<T>();
                var combined = 0;
                var last = 0;
                foreach (var item in _rates)
                {
                    if (usecombined)
                    {
                        last = combined;
                        combined += item.Value;

                        if (rate >= last && rate <= combined)
                        {
                            combinedlist.Add(item.Key);
                        }
                    }
                    else
                    {
                        if (rate >= item.Value)
                        {
                            combinedlist.Add(item.Key);
                        }
                    }
                }
                if (combinedlist.Count > 0)
                {
                    Shuffle();
                    return combinedlist.GetRandom();
                }
                else
                {
                    return returnitem;
                }
            }
        }

        /// <summary>
        /// Called to get many items with a rate (usecombined will get the item that the rate intercepts)
        /// </summary>
        public RandomList<T> GetRandomListWithRate(int rate, bool usecombined = false)
        {
            var returnitems = new RandomList<T>();
            if (Count == 0) return returnitems;
            if (_rateRange == 0)
            {
                return new RandomList<T>(base.GetRange(0, Count));
            }
            else
            {
                int combined = 0;
                foreach (var item in _rates)
                {
                    if (usecombined)
                    {
                        combined += item.Value;
                    }
                    else
                    {
                        combined = item.Value;
                    }
                    if (rate >= combined)
                    {
                        returnitems.Add(item.Key);
                    }
                }
                Shuffle();
                return returnitems;
            }
        }

        public T GetRandom()
        {
            var returnitem = default(T);
            if (Count == 0) return returnitem;
            if (_rateRange == 0)
            {
                return base[_random.Next(0, Count)];
            }
            else
            {                
                var rate = _random.Next(0, _rateRange);
                var combinedlist = new RandomList<T>();
                foreach (var item in _rates)
                {
                    if (rate >= item.Value)
                    {
                        combinedlist.Add(item.Key);
                    }
                }
                if (combinedlist.Count > 0)
                {
                    Shuffle();
                    return combinedlist.GetRandom();
                }
                else
                {
                    return returnitem;
                }
            }
        }

        /// <summary>
        /// Called to add an item (rate is optional - else will be 0)
        /// </summary>
        /// <param name="item">Item to add (should be unique - duplicates are allowed but will combine the rates if detected)</param>
        /// <param name="rate">Rate of the item (used to randomly pick an item using rates - of 100/1000 or 1000000)</param>
        public void Add(T item, int rate = 0)
        {
            base.Add(item);
            _rates.Add(item, rate);
        }

        public new void Clear(bool dispose = false)
        {
            _rates.Clear(dispose);
            base.Clear(dispose);
        }
        
        public new T this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
            }
        }

        public override string ToString()
        {
            var b = new StringBuilder("[");
            foreach (var item in base.Items)
            {
                b.Append($@"{item.ToString()}, ");
            }
            if (b.Length > 1)
                b.Remove(b.Length - 2, 2);
            b.Append("]");
            return b.ToString();
        }
    }
}
