namespace AE.Http.Proxy.Context.Converters
{
    public interface IMessageConverter
    {
        bool CanConvertFrom<TRequest, TResponse>();
    }
}