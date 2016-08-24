namespace AE.Http.Proxy.Test.Context.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Context.Converters;

    using Xunit;

    public class SelfConverterTests
    {
        [Fact]
        public void Convert_RequestContext_to_request_without_changes()
        {
            // Arrange
            var selfConverter = new SelfMessageConverter();
            var requestContext = new RequestContext(RequestMethod.GET, "/test", new Dictionary<string, string> { { "test", "test" } }, new byte[0]);
            
            // Act
            var request = selfConverter.ConvertToRequest(requestContext);

            // Assert
            Assert.Equal(requestContext, request);
            Assert.Equal(requestContext.Method, request.Method);
            Assert.Equal(requestContext.Path, request.Path);
            Assert.Equal(requestContext.RawContent, request.RawContent);
            var headersFromContext = requestContext.Headers.ToDictionary(context => context.Key, context => context.Value);
            var headersFromRequest = request.Headers.ToDictionary(context => context.Key, context => context.Value);
            Assert.Equal(headersFromContext, headersFromRequest);
        }

        [Fact]
        public void Convert_RequestContext_from_request_without_changes()
        {
            // Arrange
            var selfConverter = new SelfMessageConverter();
            var request = new RequestContext(RequestMethod.GET, "/test", new Dictionary<string, string> { { "test", "test" } }, new byte[0]);

            // Act
            var requestContext = selfConverter.ConvertFromRequest(request);

            // Assert
            Assert.Equal(request, requestContext);
            Assert.Equal(request.Method, requestContext.Method);
            Assert.Equal(request.Path, requestContext.Path);
            Assert.Equal(request.RawContent, requestContext.RawContent);
            var headersFromContext = requestContext.Headers.ToDictionary(context => context.Key, context => context.Value);
            var headersFromRequest = request.Headers.ToDictionary(context => context.Key, context => context.Value);
            Assert.Equal(headersFromRequest, headersFromContext);
        }

        [Fact]
        public void Can_convert_RequestContext_and_ResponseContext()
        {
            // Arrange
            var selfConverter = new SelfMessageConverter();

            // Act
            var result = selfConverter.CanConvertFrom<RequestContext, ResponseContext>();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Not_support_converting_to_request_by_reference()
        {
            // Arrange
            var selfConverter = new SelfMessageConverter();
            var stubRequestContext = new RequestContext(RequestMethod.GET, "", new Dictionary<string, string>(), new byte[0]);

            // Act
            var action = new Action(() => selfConverter.ConvertToRequest(null, ref stubRequestContext));

            // Assert
            Assert.Throws<NotSupportedException>(action);
        }

        [Fact]
        public void Not_support_converting_to_response_by_reference()
        {
            // Arrange
            var selfConverter = new SelfMessageConverter();
            var stubRequestContext = new RequestContext(RequestMethod.GET, "", new Dictionary<string, string>(), new byte[0]);
            var stubResponseContext = new ResponseContext(stubRequestContext, new Dictionary<string, string>(), new byte[0], 200);

            // Act
            var action = new Action(() => selfConverter.ConvertToResponse(null, ref stubResponseContext));

            // Assert
            Assert.Throws<NotSupportedException>(action);
        }
    }
}