namespace AE.Http.Proxy.Context.Filters
{
    public interface IProxyFilter
    {
        void OnRequest(RequestContext requestContext);

        void OnResponse(ResponseContext responseContext);
    }
}