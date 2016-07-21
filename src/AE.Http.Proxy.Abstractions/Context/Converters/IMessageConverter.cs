namespace AE.Http.Proxy.Abstractions.Context.Converters
{
    public interface IMessageConverter
    {
        bool CanConvertFrom<TRequest, TResponse>();
    }
}