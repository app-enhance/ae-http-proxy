namespace AE.Http.Proxy.Abstractions.Context
{
    using System;
    using System.Collections.Generic;

    public class ResponseContext : MessageContext
    {
        public ResponseContext(IRequestContext request, IDictionary<string, string> headers, byte[] rawContent, int status)
            : base(headers, rawContent)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (status < 100 && status > 550)
            {
                throw new ArgumentException("Status parameter is incorrect");
            }

            Status = status;
            Request = request;
        }

        public IRequestContext Request { get; private set; }

        public int Status { get; private set; }

        protected override HeaderOrigin InitialHeadersOrigin
        {
            get { return HeaderOrigin.Response; }
        }
    }
}