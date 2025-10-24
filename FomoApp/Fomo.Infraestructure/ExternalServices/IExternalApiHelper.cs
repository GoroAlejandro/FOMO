namespace Fomo.Infraestructure.ExternalServices
{
    public interface IExternalApiHelper
    {
        Task<T> GetAsync<T>(string endpoint);
    }
}
