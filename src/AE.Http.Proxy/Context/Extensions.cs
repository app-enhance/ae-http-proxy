namespace SelfServiceProxy.Api.PFM.Proxy.Context
{
    using System;

    public static class Extensions
    {
        public static RequestMethod ToRequestMethod(this string method)
        {
            return (RequestMethod)Enum.Parse(typeof(RequestMethod), method);
        }
    }
}