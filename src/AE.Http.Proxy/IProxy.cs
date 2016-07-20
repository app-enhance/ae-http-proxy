namespace AE.Http.Proxy
{
    public interface IProxy
    {
        TResponse Execute<TRequest, TResponse>(TRequest request);

        void Execute<TRequest, TResponse>(TRequest request, TResponse response);
    }
}