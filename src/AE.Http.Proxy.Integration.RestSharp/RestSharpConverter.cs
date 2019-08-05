namespace AE.Http.Proxy.Integration.RestSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Abstractions.Context.Converters;

    using global::RestSharp;

    public class RestSharpConverter : IMessageConverter<IRestRequest, IRestResponse>
    {
        public bool CanConvertFrom<TRequest, TResponse>()
        {
            return typeof(IRestRequest).IsAssignableFrom(typeof(TRequest)) && typeof(IRestResponse).IsAssignableFrom(typeof(TResponse));
        }

        public RequestContext ConvertFromRequest(IRestRequest request)
        {
            var method = request.Method.ToRequestMethod();
            var headers = request.GetHeaders().ToDictionary();
            var path = request.GetPath() + request.GetQueryString();
            var rawContent = request.GetBytes();
            return new RequestContext(method, path, headers, rawContent);
        }

        public ResponseContext ConvertFromResponse(IRestResponse response, IRequestContext requestContext)
        {
            var headers = response.Headers.ToDictionary();
            return new ResponseContext(requestContext, headers, response.RawBytes, (int)response.StatusCode);
        }

        public IRestRequest ConvertToRequest(RequestContext requestContext)
        {
            var request = new RestRequest(requestContext.Path, requestContext.Method.ConvertToMethod());
            SetHeaders(requestContext.Headers, request);
            SetBody(requestContext, request);
            return request;
        }

        public IRestResponse ConvertToResponse(ResponseContext responseContext)
        {
            var response = new RestResponse { Content = responseContext.Content };
            SetHeaders(responseContext.Headers, response);
            return response;
        }

        public void ConvertToRequest(RequestContext requestContext, ref IRestRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            request.Method = requestContext.Method.ConvertToMethod();
            request.Resource = requestContext.Path;
            SetHeaders(requestContext.Headers, request);
            SetBody(requestContext, request);
        }

        public void ConvertToResponse(ResponseContext responseContext, ref IRestResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            response.StatusCode = (HttpStatusCode)responseContext.Status;
            SetHeaders(responseContext.Headers, response);
            response.Content = responseContext.Content;
        }

        private static void SetHeaders(IEnumerable<IHeaderContext> headers, IRestRequest request)
        {
            request.Parameters.RemoveAll(x => x.Type == ParameterType.HttpHeader);
            foreach (var headerContext in headers.Where(x => !x.IsDeleted))
            {
                request.AddHeader(headerContext.Key, headerContext.Value);
            }
        }

        private static void SetHeaders(IEnumerable<IHeaderContext> headers, IRestResponse response)
        {
            response.Headers.Clear();
            foreach (var headerContext in headers.Where(x => !x.IsDeleted))
            {
                response.Headers.Add(new Parameter { Name = headerContext.Key, Value = headerContext.Value, Type = ParameterType.HttpHeader });
            }
        }

        private static void SetBody(RequestContext requestContext, IRestRequest request)
        {
            // TODO: possible to add more logic to handle uploading files and images
            if (requestContext.Method == RequestMethod.POST || requestContext.Method == RequestMethod.PATCH || requestContext.Method == RequestMethod.PUT)
            {
                request.AddParameter(requestContext.ContentType, requestContext.Content, ParameterType.RequestBody);
            }
        }
    }
}