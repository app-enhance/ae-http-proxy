namespace SelfServiceProxy.Api.PFM.Proxy.Context.Filters.RewriteContent
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

            this.RoutePath = FixRoutePath(routePath);

            try
            {
                this.HostBaseAddress = new Uri(hostBaseAddress, UriKind.Absolute);
                this.SourceBaseAddress = new Uri(sourceBaseAddress, UriKind.Absolute);
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
            get { return this.HostBaseAddress.LocalPath; }
        }

        public string SourceBaseAuthority
        {
            get { return this.SourceBaseAddress.GetLeftPart(UriPartial.Authority); }
        }

        public string HostBaseAuthority
        {
            get { return this.HostBaseAddress.GetLeftPart(UriPartial.Authority); }
        }

        private static string FixRoutePath(string routePath)
        {
            var slash = routePath[0] != '/' ? '/' : (char?)null;
            return string.Format("{0}{1}", slash, routePath);
        }
    }
}