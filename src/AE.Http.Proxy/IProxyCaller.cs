namespace SelfServiceProxy.Api.PFM.Proxy
{
    using SelfServiceProxy.Api.PFM.Proxy.Context;

    public interface IProxyCaller
    {
        ResponseContext Execute(RequestContext requestContext);
    }
}