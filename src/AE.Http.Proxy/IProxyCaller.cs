namespace AE.Http.Proxy
{
    using AE.Http.Proxy.Context;

    public interface IProxyCaller
    {
        ResponseContext Execute(RequestContext requestContext);
    }
}