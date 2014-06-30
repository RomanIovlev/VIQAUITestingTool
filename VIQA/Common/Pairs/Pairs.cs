﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VIQA.Common.Pairs
{
    public class Pairs : Pairs<string, string>
    {
        public Pairs() {}
        public Pairs(Object value1, Object value2) : base(value1.ToString(), value2.ToString()) { }

        public void Add(Object value1, Object value2) { 
            Add(new Pair<string, string>((value1 != null) ? value1.ToString() : null,
                (value2 != null) ? value2.ToString() : null)); 
        }
    }

    public class Pairs<TValue1, TValue2> : Collection<Pair<TValue1, TValue2>>
    {
        public Pairs() { }

        public Pairs(IEnumerable<Pair<TValue1, TValue2>> pairs = null)
        {
            if (pairs == null) return;
            foreach (var element in pairs)
                Add(element);
        }

        public Pairs(TValue1 value1, TValue2 value2, IEnumerable<Pair<TValue1, TValue2>> pairs = null)
        {
            if (pairs != null)
                foreach (var element in pairs)
                    Add(element);
            Add(value1, value2);
        }

        public void Add(TValue1 value1, TValue2 value2) { Add(new Pair<TValue1, TValue2>(value1, value2)); }
        public void Add(Pairs<TValue1, TValue2> pairs) { pairs.ForEach(Add); }

        public void AddNew(TValue1 value1, TValue2 value2)
        {
            Clear();
            Add(new Pair<TValue1, TValue2>(value1, value2));
        }


        public void ForEach(Action<Pair<TValue1, TValue2>> action)
        {
            foreach (var element in this)
                action(element);
        }

        public string Print(string separator = "; ", string pairFormat = "{0}: {1}")
        {
            return (this.Any()) ? string.Join(separator, this.Select(pair => string.Format(pairFormat, pair.Value1, pair.Value2))) : "";
        }
    }
}
