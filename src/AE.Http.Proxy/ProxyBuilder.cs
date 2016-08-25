namespace AE.Http.Proxy
{
    using System;
    using System.Collections.Generic;

    using AE.Http.Proxy.Abstractions;
    using AE.Http.Proxy.Abstractions.Context.Converters;
    using AE.Http.Proxy.Abstractions.Context.Filters;
    using AE.Http.Proxy.Context.Converters;

    public class ProxyBuilder
    {
        private readonly IProxyCaller _caller;

        private readonly List<IMessageConverter> _messageConverters;

        private readonly List<IProxyFilter> _proxyfilters;

        public ProxyBuilder(IProxyCaller caller)
        {
            if (caller == null)
            {
                throw new ArgumentNullException("caller");
            }

            _caller = caller;
            _messageConverters = new List<IMessageConverter>();
            _proxyfilters = new List<IProxyFilter>();

            // Add default self converter
            AddMessageConverter(new SelfMessageConverter());
        }

        public ProxyBuilder AddMessageConverters(params IMessageConverter[] converters)
        {
            if (converters == null)
            {
                throw new ArgumentNullException("converters");
            }

            foreach (var messageConverter in converters)
            {
                AddMessageConverter(messageConverter);
            }

            return this;
        }

        public ProxyBuilder AddMessageConverter(IMessageConverter messageConverter)
        {
            if (messageConverter == null)
            {
                throw new ArgumentNullException("messageConverter");
            }

            _messageConverters.Add(messageConverter);
            return this;
        }

        public ProxyBuilder AddFilters(params IProxyFilter[] filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException("filters");
            }

            foreach (var filter in filters)
            {
                AddFilter(filter);
            }

            return this;
        }

        public ProxyBuilder AddFilter(IProxyFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            _proxyfilters.Add(filter);
            return this;
        }

        public IProxy Build()
        {
            return new Proxy(_messageConverters, _proxyfilters, _caller);
        }
    }
}