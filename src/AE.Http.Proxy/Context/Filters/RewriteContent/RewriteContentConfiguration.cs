namespace AE.Http.Proxy.Context.Filters.RewriteContent
{
    using System;

    public class RewriteContentConfiguration
    {
        public RewriteContentConfiguration(string routePath, string hostBaseAddress, string sourceBaseAddress)
        {
            if (string.IsNullOrWhiteSpace(routePath))
            {
                throw new ArgumentNullException("routePath");
            }

            if (string.IsNullOrWhiteSpace(hostBaseAddress))
            {
                throw new ArgumentNullException("hostBaseAddress");
            }

            if (string.IsNullOrWhiteSpace(sourceBaseAddress))
            {
                throw new ArgumentNullException("sourceBaseAddress");
            }

            RoutePath = FixRoutePath(routePath);

            try
            {
                HostBaseAddress = new Uri(hostBaseAddress, UriKind.Absolute);
                SourceBaseAddress = new Uri(sourceBaseAddress, UriKind.Absolute);
            }
            catch (Exception e)
            {
                throw new ArgumentException("sourceBaseAddress or destinationBaseAddress has incorrect Uri Absolute format", e);
            }
        }

        public string RoutePath { get; private set; }

        public Uri HostBaseAddress { get; private set; }

        public Uri SourceBaseAddress { get; private set; }

        public string HostVirtualPath
        {
            get { return HostBaseAddress.LocalPath; }
        }

        public string SourceBaseAuthority
        {
            get { return SourceBaseAddress.GetLeftPart(UriPartial.Authority); }
        }

        public string HostBaseAuthority
        {
            get { return HostBaseAddress.GetLeftPart(UriPartial.Authority); }
        }

        private static string FixRoutePath(string routePath)
        {
            var slash = routePath[0] != '/' ? '/' : (char?)null;
            return string.Format("{0}{1}", slash, routePath);
        }
    }
}