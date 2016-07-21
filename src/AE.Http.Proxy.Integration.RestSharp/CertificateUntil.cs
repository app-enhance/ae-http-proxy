namespace AE.Http.Proxy.Integration.RestSharp
{
    using System.Security.Cryptography.X509Certificates;

    public static class CertificateUntil
    {
        public static X509CertificateCollection GetCertificatesFromStorage(string certificateThumbprint)
        {
            var store = new X509Store(StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, true);
                return certificates;
            }
            finally
            {
                store.Close();
            }
        }
    }
}