namespace ClientApp.Utils;

public interface IRequestHandler
{
    Task<HttpResponseMessage> GetAsync(string requestUri, string token = "");
    Task<HttpResponseMessage> PostAsync<T>(string requestUri, T body, string token = "");
    Task<HttpResponseMessage> PutAsync<T>(string requestUri, T body, string token = "");
    Task<HttpResponseMessage> DeleteAsync(string requestUri, string token = "");
}
