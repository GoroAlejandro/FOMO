namespace Fomo.Infrastructure.ExternalServices
{
    public interface IExternalApiHelper
    {
        Task<T> GetAsync<T>(string endpoint);
    }
}
