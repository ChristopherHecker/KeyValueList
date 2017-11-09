// (c) 2017 Chris Hecker.

using System;
using System.Collections.Generic;
using System.Text;

namespace KeyValueList
{
    class KeyValueComparers<TKey, TVal> where TKey: IComparable<TKey> where TVal: IComparable<TVal>
    {
        public static int CompareByKey(KeyValuePair<TKey, TVal> a, KeyValuePair<TKey, TVal> b)
        {
            int comparison = a.Key.CompareTo(b.Key);
            
            return (comparison !=0) ? comparison : a.Value.CompareTo(b.Value);
        }

        public static int CompareByValue(KeyValuePair<TKey, TVal> a, KeyValuePair<TKey, TVal> b)
        {
            int comparison = a.Value.CompareTo(b.Value);

            return (comparison != 0) ? comparison : a.Key.CompareTo(b.Key);
        }
    }
    

}
