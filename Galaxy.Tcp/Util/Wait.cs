using System;
using System.Threading;

namespace Galaxy.Tcp.Util
{
    public class Wait
    {
        public static void For(Func<bool> condition, int timeout = 1, bool waitBefore = false)
        {
            if (waitBefore)
                Thread.Sleep(1000);
            
            while (!condition())
            {
                Thread.Sleep(timeout);
            }
        }
    }
}