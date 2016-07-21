namespace AE.Http.Proxy.Context.Filters.ForbiddenHeaders
{
    using System.Collections.Generic;
    using System.Linq;

    public class ForbiddenHeadersConfiguration
    {
        public ForbiddenHeadersConfiguration(IEnumerable<string> forbiddenRequestHeaders, IEnumerable<string> forbiddenResponseHeaders)
        {
            ForbiddenRequestHeaders = forbiddenRequestHeaders ?? Enumerable.Empty<string>();
            ForbiddenResponseHeaders = forbiddenResponseHeaders ?? Enumerable.Empty<string>();
        }

        public IEnumerable<string> ForbiddenRequestHeaders { get; private set; }

        public IEnumerable<string> ForbiddenResponseHeaders { get; private set; }
    }
}