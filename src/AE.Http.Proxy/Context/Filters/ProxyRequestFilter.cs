namespace AE.Http.Proxy.Context.Filters
{
    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Abstractions.Context.Filters;

    public abstract class ProxyRequestFilter : IProxyFilter
    {
        public abstract void OnRequest(RequestContext requestContext);

        public void OnResponse(ResponseContext responseContext)
        {
        }
    }
}