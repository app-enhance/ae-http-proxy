namespace AE.Http.Proxy.Integration.Autofac
{
    using System;
    using System.Reflection;

    using AE.Http.Proxy.Abstractions;
    using AE.Http.Proxy.Abstractions.Context.Converters;
    using AE.Http.Proxy.Abstractions.Context.Filters;

    using global::Autofac;
    using global::Autofac.Builder;
    using global::Autofac.Features.Scanning;

    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<IProxy, SimpleActivatorData, SingleRegistrationStyle> RegisterProxy(this ContainerBuilder builder)
        {
            return builder.Register(BuildProxy);
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterProxyFilters(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            return builder.RegisterAssemblyTypesAssignableFrom(assembly, typeof(IProxyFilter));
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterProxyMessageConverters(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            return builder.RegisterAssemblyTypesAssignableFrom(assembly, typeof(IMessageConverter));
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterProxyCaller(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            return builder.RegisterAssemblyTypesAssignableFrom(assembly, typeof(IProxyCaller));
        }

        private static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAssemblyTypesAssignableFrom(
            this ContainerBuilder builder,
            Assembly assembly,
            Type type)
        {
            return builder.RegisterAssemblyTypes(assembly).Where(type.IsAssignableFrom);
        }

        private static IProxy BuildProxy(IComponentContext c)
        {
            return new AutofacProxyBuilder(c).AddFilters().AddMessageConverters().Build();
        }
    }
}