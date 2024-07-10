using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

public class EpicGamesAPI
{
    private const string AccountServiceURL = "https://account-public-service-prod03.ol.epicgames.com";
    private const string LauncherServiceURL = "https://launcher-public-service-prod06.ol.epicgames.com";
    private const string EglUserAgent = "UELauncher/14.2.4-22208432+++Portal+Release-Live Windows/10.0.22000.1.256.64bit";
    private const string EglCredentials = "MzRhMDJjZjhmNDQxNGUyOWIxNTkyMTg3NmRhMzZmOWE6ZGFhZmJjY2M3Mzc3NDUwMzlkZmZlNTNkOTRmYzc2Y2Y=";

    private string _bearerToken = "";
    private readonly HttpClient _httpClient;

    public EpicGamesAPI()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(EglUserAgent);
    }

    // Perform OAuth authentication
    public async Task<string> AuthenticateAsync()
    {
        var form = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("token_type", "eg1")
        });

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", EglCredentials);

        var response = await _httpClient.PostAsync($"{AccountServiceURL}/account/api/oauth/token", form);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Invalid status code {response.StatusCode}");
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        var respBody = JsonSerializer.Deserialize<JsonElement>(responseBody);

        _bearerToken = respBody.GetProperty("access_token").GetString();
        return _bearerToken;
    }

    // Fetch a catalog
    public async Task<string> FetchCatalogAsync(string platform, string namespace_, string item, string app, string label)
    {
        if (string.IsNullOrEmpty(_bearerToken))
        {
            await AuthenticateAsync();
        }

        var url = $"{LauncherServiceURL}/launcher/api/public/assets/v2/platform/{platform}/namespace/{namespace_}/catalogItem/{item}/app/{app}/label/{label}";

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Invalid status code {response.StatusCode}");
        }

        return await response.Content.ReadAsStringAsync();
    }
}