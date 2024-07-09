using System.Text;

public class GetManifestData
{
    private readonly HttpClient _httpClient;

    public GetManifestData()
    {
        _httpClient = new HttpClient();
    }

    public async Task OpenManifestAsync(string manifestId)
    {
        // Github Manifest Archive
        string rawManifestUrl = $"https://raw.githubusercontent.com/VastBlast/FortniteManifestArchive/main/Fortnite/Windows/{manifestId}.manifest";
        
        try
        {
            byte[] manifestBytes = await _httpClient.GetByteArrayAsync(rawManifestUrl);
            string rawManifest = Encoding.UTF8.GetString(manifestBytes);

            // Convert Manifest
            ManifestConverter manifestConverter = new ManifestConverter();
            manifestConverter.GetManifestData(rawManifest);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error Retrieving manifest: {e.Message}");
        }
    }
}