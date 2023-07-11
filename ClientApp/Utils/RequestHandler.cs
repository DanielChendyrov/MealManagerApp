using System.Text;
using System.Text.Json;

namespace ClientApp.Utils;

public class RequestHandler : IRequestHandler
{
    private static readonly HttpClient Client = new();

    private const string _basePath = "http://localhost:5000/api/";

    //public RequestHandler()
    //{
    //    Client = new();
    //}

    public async Task<HttpResponseMessage> GetAsync(string requestUri, string token = "")
    {
        if (!string.IsNullOrEmpty(token))
        {
            Client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }

        var uri = new Uri(_basePath + requestUri);
        var response = await Client.GetAsync(uri);
        return response;
    }

    public async Task<HttpResponseMessage> PostAsync<T>(
        string requestUri,
        T body,
        string token = ""
    )
    {
        var jsonBody = JsonSerializer.Serialize(body);
        var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        // Setup header
        httpContent.Headers.ContentType = new("application/json");

        if (!string.IsNullOrEmpty(token))
        {
            Client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }

        var uri = new Uri(_basePath + requestUri);
        var response = await Client.PostAsync(uri, httpContent);

        return response;
    }

    public async Task<HttpResponseMessage> PutAsync<T>(string requestUri, T body, string token = "")
    {
        var jsonBody = JsonSerializer.Serialize(body);
        var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        // Setup header
        httpContent.Headers.ContentType = new("application/json");

        if (!string.IsNullOrEmpty(token))
        {
            Client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }

        var uri = new Uri(_basePath + requestUri);
        var response = await Client.PutAsync(uri, httpContent);

        return response;
    }

    public async Task<HttpResponseMessage> DeleteAsync(string requestUri, string token = "")
    {
        if (!string.IsNullOrEmpty(token))
        {
            Client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }

        var uri = new Uri(_basePath + requestUri);
        var response = await Client.DeleteAsync(uri);
        return response;
    }
}
