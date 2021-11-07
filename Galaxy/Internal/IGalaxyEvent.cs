using System.Net.Http;

namespace Galaxy.Internal
{
    public class IGalaxyEvent : HttpMethod
    {
        public static readonly IGalaxyEvent Get = new("Get");
        public static readonly IGalaxyEvent Post = new("Post");
        public static readonly IGalaxyEvent Put = new("Put");
        public static readonly IGalaxyEvent Delete = new("Delete");
        public static readonly IGalaxyEvent Patch = new("Patch");
        public static readonly IGalaxyEvent Head = new("Head");
        public static readonly IGalaxyEvent Options = new("Options");
        public static readonly IGalaxyEvent Trace = new("Trace");
        public static readonly IGalaxyEvent Connect = new("Connect");
        


        public IGalaxyEvent(string method) : base(method)
        {
        }
    }
    // TODO: Custom events
}