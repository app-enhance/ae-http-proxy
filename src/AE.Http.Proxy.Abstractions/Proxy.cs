namespace AE.Http.Proxy.Abstractions
{
    using System;
    using System.Collections.Generic;

    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Abstractions.Context.Converters;
    using AE.Http.Proxy.Abstractions.Context.Filters;

    public class Proxy : IProxy
    {
        private readonly IProxyCaller _caller;

        private readonly IEnumerable<IProxyFilter> _filters;

        private readonly IEnumerable<IMessageConverter> _messageConverters;

        public Proxy(IEnumerable<IMessageConverter> messageConverters, IEnumerable<IProxyFilter> filters, IProxyCaller caller)
        {
            _messageConverters = messageConverters;
            _filters = filters;
            _caller = caller;
        }

        public TResponse Execute<TRequest, TResponse>(TRequest request)
        {
            var messageConverter = _messageConverters.FindMessageConverter<TRequest, TResponse>();
            var requestContext = messageConverter.ConvertFromRequest(request);

            var responseContext = ExecuteInternal(requestContext);

            return messageConverter.ConvertToResponse(responseContext);
        }

        public void Execute<TRequest, TResponse>(TRequest request, TResponse response)
        {
            if (response.Equals(default(TResponse)))
            {
                throw new ArgumentNullException("response");
            }

            var messageConverter = _messageConverters.FindMessageConverter<TRequest, TResponse>();
            var requestContext = messageConverter.ConvertFromRequest(request);

            var responseContext = ExecuteInternal(requestContext);

            messageConverter.ConvertToResponse(responseContext, ref response);
        }

        private ResponseContext ExecuteInternal(RequestContext requestContext)
        {
            foreach (var filter in _filters)
            {
                filter.OnRequest(requestContext);
            }

            var responseContext = _caller.Execute(requestContext);

            foreach (var filter in _filters)
            {
                filter.OnResponse(responseContext);
            }

            return responseContext;
        }
    }
}