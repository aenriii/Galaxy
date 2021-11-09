using System;

namespace Galaxy.Tcp.Util
{
    public class TypeUtils
    {
        public static T To<T> (object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}