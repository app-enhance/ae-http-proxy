namespace AE.Http.Proxy.Context.Filters
{
    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Abstractions.Context.Filters;

    public abstract class ProxyResponseFilter : IProxyFilter
    {
        public void OnRequest(RequestContext requestContext)
        {
        }

        public abstract void OnResponse(ResponseContext responseContext);
    }
}