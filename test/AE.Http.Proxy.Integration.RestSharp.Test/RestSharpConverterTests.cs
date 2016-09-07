namespace AE.Http.Proxy.Integration.RestSharp.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AE.Http.Proxy.Abstractions.Context;

    using global::RestSharp;

    using Xunit;

    public class RestSharpConverterTests
    {
        [Fact]
        public void Convert_RequestContext_to_request()
        {
            // Arrange
            var restSharpConverter = new RestSharpConverter();
            var body = "{ \"testKey\": \"testValue\" }";
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            var requestContext = new RequestContext(RequestMethod.POST, "/test?testKey=testValue", new Dictionary<string, string> { { "test", "test" } }, bodyRaw);

            // Act
            var request = restSharpConverter.ConvertToRequest(requestContext);

            // Assert
            Assert.Equal(requestContext.Method.ToString(), request.Method.ToString());
            Assert.Equal(requestContext.Path, request.GetPath());
            Assert.Equal(body, request.Parameters.Single(x => x.Type == ParameterType.RequestBody).Value.ToString());
            Assert.Equal(requestContext.RawContent, request.GetBytes());
            var headersFromContext = requestContext.Headers.ToDictionary(context => context.Key, context => context.Value);
            var headersFromRequest = request.GetHeaders().ToDictionary(context => context.Name, context => context.Value.ToString());
            Assert.Equal(headersFromContext, headersFromRequest);
        }

        [Fact]
        public void Convert_requestContext_to_request_by_reference()
        {
            var restSharpConverter = new RestSharpConverter();
            var body = "{ \"testKey\": \"testValue\" }";
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            var requestContext = new RequestContext(RequestMethod.POST, "/test?testKey=testValue", new Dictionary<string, string> { { "test", "test" } }, bodyRaw);
            var request = new RestRequest("") as IRestRequest;

            // Act
            restSharpConverter.ConvertToRequest(requestContext, ref request);

            // Assert
            Assert.Equal(requestContext.Method.ToString(), request.Method.ToString());
            Assert.Equal(requestContext.Path, request.GetPath());
            Assert.Equal(body, request.Parameters.Single(x => x.Type == ParameterType.RequestBody).Value.ToString());
            Assert.Equal(requestContext.RawContent, request.GetBytes());
            var headersFromContext = requestContext.Headers.ToDictionary(context => context.Key, context => context.Value);
            var headersFromRequest = request.GetHeaders().ToDictionary(context => context.Name, context => context.Value.ToString());
            Assert.Equal(headersFromContext, headersFromRequest);
        }
    }
}