namespace SelfServiceProxy.Api.PFM.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SelfServiceProxy.Api.PFM.Proxy.Context.Converters;
    using SelfServiceProxy.Api.PFM.Proxy.Context.Filters;

    public class ProxyBuilder
    {
        private readonly List<IMessageConverter> messageConverters;

        private readonly List<IProxyFilter> proxyfilters;

        private readonly IProxyCaller caller;

        public ProxyBuilder(IProxyCaller caller)
        {
            if (caller == null)
            {
                throw new ArgumentNullException("caller");
            }

            this.caller = caller;
            this.messageConverters = new List<IMessageConverter>();
            this.proxyfilters = new List<IProxyFilter>();
        }

        public ProxyBuilder AddMessageConverters(params IMessageConverter[] converters)
        {
            if (converters == null || converters.Any() == false)
            {
                throw new ArgumentNullException("converters");
            }

            foreach (var messageConverter in converters)
            {
                this.AddMessageConverter(messageConverter);
            }

            return this;
        }

        public ProxyBuilder AddMessageConverter(IMessageConverter messageConverter)
        {
            if (messageConverter == null)
            {
                throw new ArgumentNullException("messageConverter");
            }

            this.messageConverters.Add(messageConverter);
            return this;
        }

        public ProxyBuilder AddFilters(params IProxyFilter[] filters)
        {
            if (filters == null || filters.Any() == false)
            {
                throw new ArgumentNullException("filters");
            }

            foreach (var filter in filters)
            {
                this.AddFilter(filter);
            }

            return this;
        }

        public ProxyBuilder AddFilter(IProxyFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            this.proxyfilters.Add(filter);
            return this;
        }

        public IProxy Build()
        {
            return new Proxy(this.messageConverters, this.proxyfilters, this.caller);
        }
    }
}