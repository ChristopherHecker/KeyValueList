// (c) 2017 Chris Hecker.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KeyValueList
{
    // Your specification doesn't require the use of Generics, and normally I wouldn't use them for those specs.
    // But I thought I'd demonstrate my knowledge of C# here.  And, it leads to some cool test code.
    public interface IKeyValueList<TKey, TVal> where TKey : IComparable<TKey> where TVal : IComparable<TVal>
    {
        IEnumerator Enumerator { get; }
        List<string> DisplayList { get; }
        void AddPair(TKey key, TVal value);
        void RemovePair(TKey key, TVal value);
        void ClearAll();
        void SaveAsXml(string filePath);
        void SaveAsJson(string filePath);
        void SortByKey();
        void SortByValue();
    }

}
