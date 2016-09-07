namespace AE.Http.Proxy.Integration.RestSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    using AE.Http.Proxy.Abstractions.Context;

    using global::RestSharp;

    public static class RestSharpExtensions
    {
        public static RequestMethod ToRequestMethod(this Method method)
        {
            return method.ToString().ToRequestMethod();
        }

        public static IEnumerable<Parameter> GetHeaders(this IRestRequest request)
        {
            return request.Parameters.Where(x => x.Type == ParameterType.HttpHeader);
        }

        public static IDictionary<string, string> ToDictionary(this IEnumerable<Parameter> parameters)
        {
            return parameters.ToDictionary(x => x.Name, x => x.Value.ToString());
        }

        public static string GetPath(this IRestRequest request)
        {
            var path = new StringBuilder(request.Resource);
            var urlSegments = request.Parameters.Where(x => x.Type == ParameterType.UrlSegment);
            
            foreach (var urlSegment in urlSegments)
            {
                path.AppendFormat("/{0}", urlSegment.Value);
            }

            return path.ToString();
        }

        public static string GetQueryString(this IRestRequest request)
        {
            var queryStringParams = request.Parameters.Where(x => x.Type == ParameterType.QueryString);
            return "?" + string.Join("&", queryStringParams.Select(x => string.Join("=", x.Name, x.Value)).ToArray());
        }

        public static string GetContent(this IRestRequest request)
        {
            var bodyParameter = request.Parameters.FirstOrDefault(x => x.Type == ParameterType.RequestBody);
            if (bodyParameter != null && bodyParameter.Value != null)
            {
                if (bodyParameter.Value is string)
                {
                    return bodyParameter.Value as string;
                }

                return Convert.ToString(bodyParameter.Value);
            }

            return string.Empty;
        }

        public static byte[] GetBytes(this IRestRequest request)
        {
            var bodyParameter = request.Parameters.FirstOrDefault(x => x.Type == ParameterType.RequestBody);
            if (bodyParameter != null && bodyParameter.Value != null)
            {
                if (bodyParameter.Value is string)
                {
                    return Encoding.UTF8.GetBytes(bodyParameter.Value as string);
                }

                var formatter = new BinaryFormatter();
                using (var stream = new MemoryStream())
                {
                    formatter.Serialize(stream, bodyParameter.Value);
                    return stream.ToArray();
                }
            }

            return new byte[0];
        }

        public static Method ConvertToMethod(this RequestMethod method)
        {
            return (Method)Enum.Parse(typeof(Method), method.ToString());
        }
    }
}