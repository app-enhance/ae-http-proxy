namespace AE.Http.Proxy.Context.Converters
{
    using System;

    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Abstractions.Context.Converters;

    public class SelfMessageConverter : IMessageConverter<RequestContext, ResponseContext>
    {
        public bool CanConvertFrom<TRequest, TResponse>()
        {
            return typeof(RequestContext).IsAssignableFrom(typeof(TRequest)) && typeof(ResponseContext).IsAssignableFrom(typeof(TResponse));
        }

        public RequestContext ConvertFromRequest(RequestContext request)
        {
            return request;
        }

        public ResponseContext ConvertFromResponse(ResponseContext response, IRequestContext requestContext)
        {
            return response;
        }

        public RequestContext ConvertToRequest(RequestContext requestContext)
        {
            return requestContext;
        }

        public ResponseContext ConvertToResponse(ResponseContext responseContext)
        {
            return responseContext;
        }

        public void ConvertToRequest(RequestContext requestContext, ref RequestContext request)
        {
            throw new NotSupportedException("Use ConvertToRequest(RequestContext) instead");
        }

        public void ConvertToResponse(ResponseContext responseContext, ref ResponseContext response)
        {
            throw new NotSupportedException("Use ConvertToResponse(ResponseContext) instead");
        }
    }
}