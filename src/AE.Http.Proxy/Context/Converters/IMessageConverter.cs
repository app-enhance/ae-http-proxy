namespace SelfServiceProxy.Api.PFM.Proxy.Context.Converters
{
    public interface IMessageConverter
    {
        bool CanConvertFrom<TRequest, TResponse>();
    }
}