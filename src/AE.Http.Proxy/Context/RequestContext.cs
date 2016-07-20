namespace SelfServiceProxy.Api.PFM.Proxy.Context
{
    using System.Collections.Generic;

    public class RequestContext : MessageContext, IRequestContext
    {
        public RequestContext(RequestMethod requestMethod, string path, IDictionary<string, string> headers, byte[] rawContent)
            : base(headers, rawContent)
        {
            this.Method = this.OriginMethod = requestMethod;
            this.Path = this.OriginPath = path;
        }

        public RequestMethod OriginMethod { get; private set; }

        public RequestMethod Method { get; set; }

        public string OriginPath { get; private set; }

        public string Path { get; set; }

        protected override HeaderOrigin InitialHeadersOrigin
        {
            get { return HeaderOrigin.Request; }
        }
    }
}