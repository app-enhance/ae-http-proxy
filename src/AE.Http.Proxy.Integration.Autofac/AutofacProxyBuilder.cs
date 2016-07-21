namespace AE.Http.Proxy.Integration.Autofac
{
    using System.Collections.Generic;
    using System.Linq;

    using AE.Http.Proxy.Abstractions;
    using AE.Http.Proxy.Abstractions.Context.Converters;
    using AE.Http.Proxy.Abstractions.Context.Filters;

    using global::Autofac;

    public class AutofacProxyBuilder : ProxyBuilder
    {
        private readonly IComponentContext resolver;

        public AutofacProxyBuilder(IComponentContext context)
            : base(context.Resolve<IProxyCaller>())
        {
            resolver = context;
        }

        public AutofacProxyBuilder AddFilter<T>() where T : IProxyFilter
        {
            AddFilter(resolver.Resolve<T>());
            return this;
        }

        public AutofacProxyBuilder AddFilters()
        {
            AddFilters(resolver.Resolve<IEnumerable<IProxyFilter>>().ToArray());
            return this;
        }

        public AutofacProxyBuilder AddMessageConverter<T>() where T : IMessageConverter
        {
            AddMessageConverter(resolver.Resolve<T>());
            return this;
        }

        public AutofacProxyBuilder AddMessageConverters()
        {
            AddMessageConverters(resolver.Resolve<IEnumerable<IMessageConverter>>().ToArray());
            return this;
        }
    }
}