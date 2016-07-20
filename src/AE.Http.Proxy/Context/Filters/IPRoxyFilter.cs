namespace SelfServiceProxy.Api.PFM.Proxy.Context.Filters
{
    public interface IProxyFilter
    {
        void OnRequest(RequestContext requestContext);

        void OnResponse(ResponseContext responseContext);
    }
}