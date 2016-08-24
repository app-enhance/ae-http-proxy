namespace AE.Http.Proxy.Integration.OWIN
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Abstractions.Context.Converters;

    using Microsoft.Owin;

    public class OwinConverter : IMessageConverter<IOwinRequest, IOwinResponse>
    {
        public bool CanConvertFrom<TRequest, TResponse>()
        {
            return typeof(IOwinRequest).IsAssignableFrom(typeof(TRequest)) && typeof(IOwinResponse).IsAssignableFrom(typeof(TResponse));
        }

        public RequestContext ConvertFromRequest(IOwinRequest request)
        {
            var method = request.Method.ToRequestMethod();
            var headers = request.Headers.ToDictionary();
            var path = request.Path.ToString() + request.QueryString;
            var rawContent = GetBytes(request.Body);
            return new RequestContext(method, path, headers, rawContent);
        }

        public ResponseContext ConvertFromResponse(IOwinResponse response, IRequestContext requestContext)
        {
            var headers = response.Headers.ToDictionary();
            var content = GetBytes(response.Body);
            return new ResponseContext(requestContext, headers, content, response.StatusCode);
        }

        public IOwinRequest ConvertToRequest(RequestContext requestContext)
        {
            throw new NotSupportedException("There is not possible to create OwinRequest internally. Use ConvertToRequest(RequestContext, ref IOwinRequest) instead");
        }

        public IOwinResponse ConvertToResponse(ResponseContext responseContext)
        {
            throw new NotSupportedException("There is not possible to create OwinResponse internally. Use ConvertToResponse(ResponseContext, ref IOwinResponse) instead");
        }

        public void ConvertToRequest(RequestContext requestContext, ref IOwinRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            request.Method = requestContext.Method.ToString();
            SetPathAndQueryString(requestContext.Path, request);
            SetHeaders(requestContext.Headers, request.Headers);
            request.Body.Write(requestContext.RawContent, 0, (int)requestContext.ContentLength);
        }

        public void ConvertToResponse(ResponseContext responseContext, ref IOwinResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            response.StatusCode = responseContext.Status;
            SetHeaders(responseContext.Headers, response.Headers);
            response.Write(responseContext.RawContent);
        }

        private static byte[] GetBytes(Stream bodyStream)
        {
            var buffer = new byte[16 * 1024];
            using (var stream = new MemoryStream())
            {
                int read;
                while ((read = bodyStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, read);
                }

                return stream.ToArray();
            }
        }

        private static void SetPathAndQueryString(string resource, IOwinRequest request)
        {
            string path;
            string queryString;
            SplitResource(resource, out path, out queryString);
            request.Path = new PathString(path);
            request.QueryString = new QueryString(queryString);
        }

        private static void SplitResource(string resource, out string path, out string queryString)
        {
            var parts = resource.Split('?');
            path = parts[0];
            queryString = string.Empty;

            if (parts.Length > 2)
            {
                throw new InvalidOperationException(string.Format("Incorrect resource: {0}", resource));
            }

            if (parts.Length == 2)
            {
                queryString = parts[1];
            }
        }

        private static void SetHeaders(IEnumerable<IHeaderContext> headers, IHeaderDictionary responseHeaders)
        {
            foreach (var headerContext in headers.Where(x => !x.IsDeleted))
            {
                responseHeaders.Set(headerContext.Key, headerContext.Value);
            }
        }
    }
}