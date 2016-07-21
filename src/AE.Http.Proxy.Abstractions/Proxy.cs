namespace AE.Http.Proxy.Abstractions
{
    using System;
    using System.Collections.Generic;

    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Abstractions.Context.Converters;
    using AE.Http.Proxy.Abstractions.Context.Filters;

    public class Proxy : IProxy
    {
        private readonly IEnumerable<IMessageConverter> messageConverters;

        private readonly IEnumerable<IProxyFilter> filters;

        private readonly IProxyCaller caller;

        public Proxy(IEnumerable<IMessageConverter> messageConverters, IEnumerable<IProxyFilter> filters, IProxyCaller caller)
        {
            this.messageConverters = messageConverters;
            this.filters = filters;
            this.caller = caller;
        }

        public TResponse Execute<TRequest, TResponse>(TRequest request)
        {
            var messageConverter = this.messageConverters.FindMessageConverter<TRequest, TResponse>();
            var requestContext = messageConverter.ConvertFromRequest(request);

            var responseContext = this.ExecuteInternal(requestContext);

            return messageConverter.ConvertToResponse(responseContext);
        }

        public void Execute<TRequest, TResponse>(TRequest request, TResponse response) 
        {
            if (response.Equals(default(TResponse)))
            {
                throw new ArgumentNullException("response");
            }

            var messageConverter = this.messageConverters.FindMessageConverter<TRequest, TResponse>();
            var requestContext = messageConverter.ConvertFromRequest(request);

            var responseContext = this.ExecuteInternal(requestContext);

            messageConverter.ConvertToResponse(responseContext, ref response);
        }

        private ResponseContext ExecuteInternal(RequestContext requestContext)
        {
            foreach (var filter in this.filters)
            {
                filter.OnRequest(requestContext);
            }

            var responseContext = this.caller.Execute(requestContext);

            foreach (var filter in this.filters)
            {
                filter.OnResponse(responseContext);
            }

            return responseContext;
        }
    }
}