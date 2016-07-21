namespace AE.Http.Proxy.Abstractions
{
    public interface IProxy
    {
        TResponse Execute<TRequest, TResponse>(TRequest request);

        void Execute<TRequest, TResponse>(TRequest request, TResponse response);
    }
}