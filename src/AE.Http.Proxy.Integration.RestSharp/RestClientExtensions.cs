namespace AE.Http.Proxy.Integration.RestSharp
{
    using System.Security.Cryptography.X509Certificates;

    using global::RestSharp;

    public static class RestClientExtensions
    {
        public static RestClient SetCertificateByThumbprint(this RestClient client, string certificateThumbprint)
        {
            var certificates = CertificateUntil.GetCertificatesFromStorage(certificateThumbprint);
            client.SetCertificates(certificates);

            return client;
        }

        public static RestClient SetCertificates(this RestClient client, X509CertificateCollection certificates)
        {
            if (certificates != null && certificates.Count != 0)
            {
                client.ClientCertificates = certificates;
            }

            return client;
        }
    }
}