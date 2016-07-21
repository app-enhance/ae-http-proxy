namespace AE.Http.Proxy.Context.Filters.ForbiddenHeaders
{
    using System.Collections.Generic;
    using System.Linq;

    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Abstractions.Context.Filters;

    public class ForbiddenHeadersProxyFilter : IProxyFilter
    {
        private readonly ForbiddenHeadersConfiguration configuration;

        public ForbiddenHeadersProxyFilter(ForbiddenHeadersConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnRequest(RequestContext requestContext)
        {
            RemoveHeaders(requestContext, this.configuration.ForbiddenRequestHeaders);
        }

        public void OnResponse(ResponseContext responseContext)
        {
            RemoveHeaders(responseContext, this.configuration.ForbiddenResponseHeaders);
        }

        private static void RemoveHeaders(MessageContext context, IEnumerable<string> forbiddenHeaders)
        {
            var headersToRemove = context.Headers.Select(x => x.Key).Intersect(forbiddenHeaders);
            foreach (var headerKey in headersToRemove)
            {
                context.DeleteHeader(headerKey);
            }
        }
    }
}