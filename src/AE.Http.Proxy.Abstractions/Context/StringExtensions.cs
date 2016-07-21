namespace AE.Http.Proxy.Abstractions.Context
{
    using System;

    public static class StringExtensions
    {
        public static RequestMethod ToRequestMethod(this string method)
        {
            return (RequestMethod)Enum.Parse(typeof(RequestMethod), method);
        }
    }
}