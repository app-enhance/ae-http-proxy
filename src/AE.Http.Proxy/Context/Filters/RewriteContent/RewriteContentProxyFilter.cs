namespace AE.Http.Proxy.Context.Filters.RewriteContent
{
    using System;
    using System.Text.RegularExpressions;

    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Abstractions.Context.Filters;

    public class RewriteContentProxyFilter : IProxyFilter
    {
        private const string EmptyRoute = "/";

        private readonly Regex regex;

        private readonly RewriteContentConfiguration config;

        private readonly Lazy<string> realRoutePath;

        public RewriteContentProxyFilter(RewriteContentConfiguration configuration)
        {
            this.config = configuration;
            var regexpPattern = string.Format(@"(?:href|src)=([""'])({0}|(?!mailto|http))\/\w.*?([""'])", this.config.SourceBaseAuthority);
            this.regex = new Regex(regexpPattern, RegexOptions.Compiled);

            this.realRoutePath = new Lazy<string>(() => Combine(this.config.HostVirtualPath, this.config.RoutePath));
        }

        public void OnRequest(RequestContext requestContext)
        {
            requestContext.Path = this.RewriteRequestPath(requestContext.Path);
        }

        public void OnResponse(ResponseContext responseContext)
        {
            if (responseContext.ContentDescriptor == ContentDescriptor.PlainText)
            {
                var rewroteContent = this.RewriteContent(responseContext.Content);
                responseContext.SetContent(rewroteContent);
            }
        }

        private static string Combine(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return string.Format("{0}/{1}", uri1, uri2);
        }

        private string RewriteRequestPath(string path)
        {
            var route = this.config.RoutePath;
            return route == EmptyRoute ? path : path.Substring(route.Length);
        }

        private string RewriteContent(string content)
        {
            var route = this.realRoutePath.Value;
            return route == EmptyRoute ? content : this.regex.Replace(content, this.Evaluator);
        }

        private string Evaluator(Match match)
        {
            var mark = match.Value[match.Value.IndexOf('=') + 1];

            var parts = match.Value.Split(mark);
            var route = this.realRoutePath.Value;

            if (parts[1].Contains(this.config.SourceBaseAuthority))
            {
                return match.Value.Replace(this.config.SourceBaseAuthority, Combine(this.config.HostBaseAuthority, route));
            }

            return string.Format(@"{0}{2}{1}{2}", parts[0], Combine(route, parts[1]), mark);
        }
    }
}