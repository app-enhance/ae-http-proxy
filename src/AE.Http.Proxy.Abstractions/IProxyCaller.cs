namespace AE.Http.Proxy.Abstractions
{
    using AE.Http.Proxy.Abstractions.Context;

    public interface IProxyCaller
    {
        ResponseContext Execute(RequestContext requestContext);
    }
}