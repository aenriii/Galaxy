using System;

namespace Galaxy.Tcp.Util
{
    public class Assert
    {
        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception("Assertion failed, " + message);
            }
        }
        public static bool Exists(object? o)
        {
            return o != null;
        }
    }
}