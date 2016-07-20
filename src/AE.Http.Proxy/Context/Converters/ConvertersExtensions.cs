namespace AE.Http.Proxy.Context.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ConvertersExtensions
    {
        public static IMessageConverter<TRequest, TResponse> FindMessageConverter<TRequest, TResponse>(this IEnumerable<IMessageConverter> messageConverters)
        {
            var messageConverter = messageConverters.FirstOrDefault(x => x.CanConvertFrom<TRequest, TResponse>()) as IMessageConverter<TRequest, TResponse>;
            if (messageConverter == null)
            {
                throw new ArgumentException(string.Format("Cannot find correct converter for pair request/response type ({0}/{1})", typeof(TRequest).Name, typeof(TResponse).Name));
            }

            return messageConverter;
        }
    }
}