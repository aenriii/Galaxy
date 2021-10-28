
namespace Galaxy.Internal
{
    interface IGalaxyWare
    {
        // returns true if the request has been handled
        public bool HandleRequest(HttpContext context);
    }
}