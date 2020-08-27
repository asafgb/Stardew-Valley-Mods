using System;
using System.Collections.Generic;

namespace StardewValleyMods.CategorizeChests
{
    internal static class Utility
    {
        public static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return Utility.YieldBatchElements<T>(enumerator, batchSize - 1);
                }
            }
            yield break;
        }

        private static IEnumerable<T> YieldBatchElements<T>(IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;
            int i = 0;
            while (i < batchSize && source.MoveNext())
            {
                yield return source.Current;
                int num = i;
                i = num + 1;
            }
            yield break;
        }

        public static IDictionary<Key, IEnumerable<Value>> KeyBy<Key, Value>(this IEnumerable<Value> values, Func<Value, Key> makeKey)
        {
            Dictionary<Key, IEnumerable<Value>> dictionary = new Dictionary<Key, IEnumerable<Value>>();
            foreach (Value value in values)
            {
                Key key = makeKey(value);
                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = new List<Value>();
                }
                ((List<Value>)dictionary[key]).Add(value);
            }
            return dictionary;
        }
    }
    //public static class ReflectionExtensions
    //{
    //    public static T GetFieldValue<T>(this object obj, string name)
    //    {
    //        // Set the flags so that private and public fields from instances will be found
    //        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    //        var field = obj.GetType().GetField(name, bindingFlags);
    //        return (T)field?.GetValue(obj);
    //    }
    //}
}
