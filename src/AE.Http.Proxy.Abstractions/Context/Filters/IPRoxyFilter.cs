namespace AE.Http.Proxy.Abstractions.Context.Filters
{
    public interface IProxyFilter
    {
        void OnRequest(RequestContext requestContext);

        void OnResponse(ResponseContext responseContext);
    }
}