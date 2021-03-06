﻿namespace AE.Http.Proxy.Abstractions.Context
{
    using System.Collections.Generic;

    public class RequestContext : MessageContext, IRequestContext
    {
        public RequestContext(RequestMethod requestMethod, string path, IDictionary<string, string> headers, byte[] rawContent)
            : base(headers, rawContent)
        {
            Method = OriginMethod = requestMethod;
            Path = OriginPath = path;
        }

        public RequestMethod OriginMethod { get; private set; }

        public string OriginPath { get; private set; }

        protected override HeaderOrigin InitialHeadersOrigin
        {
            get { return HeaderOrigin.Request; }
        }

        public RequestMethod Method { get; set; }

        public string Path { get; set; }
    }
}