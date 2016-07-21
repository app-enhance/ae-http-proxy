namespace AE.Http.Proxy.Integration.OWIN
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Owin;

    public static class OwinExtensions
    {
        public static IDictionary<string, string> ToDictionary(this IHeaderDictionary headers)
        {
            return headers.ToDictionary(x => x.Key, x => x.Value[0]);
        }
    }
}