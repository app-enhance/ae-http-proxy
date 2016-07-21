namespace AE.Http.Proxy.Integration.OWIN
{
    using System;
    using System.Threading.Tasks;

    using AE.Http.Proxy.Abstractions;

    using Microsoft.Owin;
    using Microsoft.Owin.Logging;

    public class ProxyMiddleware : OwinMiddleware
    {
        private readonly IProxy _proxy;

        public ProxyMiddleware(OwinMiddleware next, IProxy proxy, ILogger logger)
            : base(next)
        {
            Logger = logger;
            _proxy = proxy;
        }

        public string RoutePath { get; set; }

        public ILogger Logger { get; set; }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Path.Value.StartsWith(RoutePath, StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    _proxy.Execute(context.Request, context.Response);
                    return;
                }
                catch (Exception e)
                {
                    Logger.WriteError("Unknown error during proxy calling", e);
                }
            }

            await Next.Invoke(context);
        }
    }
}