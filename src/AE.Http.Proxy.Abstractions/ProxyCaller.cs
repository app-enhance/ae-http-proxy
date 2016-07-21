namespace AE.Http.Proxy.Abstractions
{
    using System.Collections.Generic;

    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Abstractions.Context.Converters;

    public abstract class ProxyCaller<TRequest, TResponse> : IProxyCaller
    {
        private readonly IEnumerable<IMessageConverter> _messageConverters;

        protected ProxyCaller(IEnumerable<IMessageConverter> messageConverters)
        {
            _messageConverters = messageConverters;
        }

        public ResponseContext Execute(RequestContext requestContext)
        {
            var messageConverter = _messageConverters.FindMessageConverter<TRequest, TResponse>();
            var request = messageConverter.ConvertToRequest(requestContext);

            var response = ExecuteInternal(request);

            return messageConverter.ConvertFromResponse(response, requestContext);
        }

        protected abstract TResponse ExecuteInternal(TRequest request);
    }
}