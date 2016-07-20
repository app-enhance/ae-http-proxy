namespace SelfServiceProxy.Api.PFM.Proxy.Context.Converters
{
    public interface IMessageConverter<TRequest, TResponse> : IMessageConverter
    {
        RequestContext ConvertFromRequest(TRequest request);

        ResponseContext ConvertFromResponse(TResponse response, IRequestContext requestContext);

        TRequest ConvertToRequest(RequestContext requestContext);

        TResponse ConvertToResponse(ResponseContext responseContext);

        void ConvertToRequest(RequestContext requestContext, ref TRequest request);

        void ConvertToResponse(ResponseContext responseContext, ref TResponse response);
    }
}