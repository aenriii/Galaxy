using System.Collections.Generic;

namespace Galaxy.Util
{
    public class ListDictionary<T1, T2>
    {
        internal Dictionary<T1, List<T2>> _dictionary = new();
        public ListDictionary()
        {
            
        }
        public bool TryAdd(T1 key, T2 value)
        {
            if (!_dictionary.TryGetValue(key, out var list))
            {
                list = new List<T2>();
                _dictionary.Add(key, list);
            }
            list.Add(value);
            return true;
        }
        public bool TryRemove(T1 key, T2 value)
        {
            if (!_dictionary.TryGetValue(key, out var list))
            {
                return false;
            }
            return list.Remove(value);
        }
        public bool TryGetValue(T1 key, out List<T2> value)
        {
            return _dictionary.TryGetValue(key, out value);
        }
        public bool TryGetValue(T1 key, int index, out T2 value)
        {
            value = default(T2);
            if (!_dictionary.TryGetValue(key, out var lst))
            {
                return false;
            }
            if (lst.Count <= index)
            {
                return false;
            }
            value = lst[index];
            return true;
            
            
        }
    }
}