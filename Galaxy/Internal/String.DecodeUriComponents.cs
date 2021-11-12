using System;

namespace Galaxy.Internal
{
    public static class String_DecodeUriComponents
    {
        public static string DecodeUriComponents(this string uri)
        {
            return Uri.UnescapeDataString(uri);
        }
    }
}