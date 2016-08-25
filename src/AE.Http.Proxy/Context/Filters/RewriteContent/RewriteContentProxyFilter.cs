namespace AE.Http.Proxy.Context.Filters.RewriteContent
{
    using System;
    using System.Text.RegularExpressions;

    using AE.Http.Proxy.Abstractions.Context;
    using AE.Http.Proxy.Abstractions.Context.Filters;

    public class RewriteContentProxyFilter : IProxyFilter
    {
        private const string EmptyRoute = "/";

        private readonly RewriteContentConfiguration _config;

        private readonly Lazy<string> _realRoutePath;

        private readonly Regex _regex;

        public RewriteContentProxyFilter(RewriteContentConfiguration configuration)
        {
            _config = configuration;
            var regexpPattern = string.Format(@"(?:href|src)=([""'])({0}|(?!mailto|http))\/\w.*?([""'])", _config.SourceBaseAuthority);
            _regex = new Regex(regexpPattern, RegexOptions.Compiled);

            _realRoutePath = new Lazy<string>(() => Combine(_config.HostVirtualPath, _config.RoutePath));
        }

        public void OnRequest(RequestContext requestContext)
        {
            requestContext.Path = RewriteRequestPath(requestContext.Path);
        }

        public void OnResponse(ResponseContext responseContext)
        {
            if (responseContext.ContentDescriptor == ContentDescriptor.PlainText)
            {
                var rewroteContent = RewriteContent(responseContext.Content);
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
            var route = _config.RoutePath;
            if (route == EmptyRoute || path.StartsWith(route) == false)
            {
                return path;
            }

            return path.Substring(route.Length);
        }

        private string RewriteContent(string content)
        {
            var route = _realRoutePath.Value;
            return route == EmptyRoute ? content : _regex.Replace(content, Evaluator);
        }

        private string Evaluator(Match match)
        {
            var mark = match.Value[match.Value.IndexOf('=') + 1];

            var parts = match.Value.Split(mark);
            var route = _realRoutePath.Value;

            if (parts[1].Contains(_config.SourceBaseAuthority))
            {
                return match.Value.Replace(_config.SourceBaseAuthority, Combine(_config.HostBaseAuthority, route));
            }

            return string.Format(@"{0}{2}{1}{2}", parts[0], Combine(route, parts[1]), mark);
        }
    }
}