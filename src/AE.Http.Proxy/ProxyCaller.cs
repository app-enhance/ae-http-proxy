namespace SelfServiceProxy.Api.PFM.Proxy
{
    using System.Collections.Generic;

    using SelfServiceProxy.Api.PFM.Proxy.Context;
    using SelfServiceProxy.Api.PFM.Proxy.Context.Converters;

    public abstract class ProxyCaller<TRequest, TResponse> : IProxyCaller
    {
        private readonly IEnumerable<IMessageConverter> messageConverters;

        protected ProxyCaller(IEnumerable<IMessageConverter> messageConverters)
        {
            this.messageConverters = messageConverters;
        }

        public ResponseContext Execute(RequestContext requestContext)
        {
            var messageConverter = this.messageConverters.FindMessageConverter<TRequest, TResponse>();
            var request = messageConverter.ConvertToRequest(requestContext);

            var response = this.ExecuteInternal(request);

            return messageConverter.ConvertFromResponse(response, requestContext);
        }

        protected abstract TResponse ExecuteInternal(TRequest request);
    }
}