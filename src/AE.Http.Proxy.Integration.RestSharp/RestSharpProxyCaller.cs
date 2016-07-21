namespace AE.Http.Proxy.Integration.RestSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using AE.Http.Proxy.Abstractions;
    using AE.Http.Proxy.Abstractions.Context.Converters;

    using global::RestSharp;

    public class RestSharpProxyCaller : ProxyCaller<IRestRequest, IRestResponse>
    {
        private const int NumberOfRetries = 3;

        private const int DelayOnRetry = 500;

        private readonly RestClient _restClient;

        public RestSharpProxyCaller(RestClient restClient, IEnumerable<IMessageConverter> messageConverters)
            : base(messageConverters)
        {
            _restClient = restClient;
        }

        protected override IRestResponse ExecuteInternal(IRestRequest request)
        {
            IRestResponse response = null;
            for (var attempts = 0; attempts <= NumberOfRetries; attempts++)
            {
                try
                {
                    response = DoCall(request);
                    break;
                }
                catch (Exception e)
                {
                    // this.Logger.Warning(string.Format("Retries no. {0} sent request to {1} with error: {2}", attempts, request.Resource, e.Message));
                    if (attempts == NumberOfRetries)
                    {
                        // this.Logger.Error(string.Format("Cannot call request to {0}", request.Resource), e);
                        throw;
                    }

                    Thread.Sleep(DelayOnRetry);
                }
            }

            // this.Logger.Information(string.Format("[{1}{2}] Status: {0}", response.StatusCode, this.restClient.BaseUrl, request.Resource));

            // Fix Content-length (sometimes is -1 but should be content bytes count)
            if (response.ContentLength < 0)
            {
                FixContentLength(response);
            }

            return response;
        }

        private static void FixContentLength(IRestResponse response)
        {
            var existingContentLength = response.Headers.SingleOrDefault(x => x.Name == "Content-Length");
            if (existingContentLength == null)
            {
                response.Headers.Add(new Parameter { Type = ParameterType.HttpHeader, Name = "Content-Length", Value = response.RawBytes.Length });
            }
            else
            {
                existingContentLength.Value = response.RawBytes.Length;
            }
        }

        private IRestResponse DoCall(IRestRequest request)
        {
            var response = _restClient.Execute(request);
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            return response;
        }
    }
}