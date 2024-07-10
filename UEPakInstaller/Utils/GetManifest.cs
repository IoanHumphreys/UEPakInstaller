using System;
using System.Text;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

public class GetManifest
{
    private readonly HttpClient _httpClient;

    public GetManifest()
    {
        _httpClient = new HttpClient();
    }

    public async Task OpenManifestAsync(string manifestId)
    {
        string rawManifestUrl = $"https://raw.githubusercontent.com/VastBlast/FortniteManifestArchive/main/Fortnite/Windows/{manifestId}.manifest";
        
        try
        {
            byte[] manifestBytes = await _httpClient.GetByteArrayAsync(rawManifestUrl);
            
            Console.WriteLine($"First 20 bytes: {BitConverter.ToString(manifestBytes.Take(20).ToArray())}");

            try 
            {
                using (var compressedStream = new MemoryStream(manifestBytes)) 
                using (var decompressStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var resultStream = new MemoryStream())
                {
                    decompressStream.CopyTo(resultStream);
                    manifestBytes = resultStream.ToArray();
                }
                Console.WriteLine("Successfully decompressed the data.");
            }
            catch (InvalidDataException)
            {
                Console.WriteLine("Data is not GZIP compressed. Proceeding with raw data.");
            }

            string rawManifest = Encoding.UTF8.GetString(manifestBytes);
            if (!IsValidUtf8(rawManifest))
            {
                rawManifest = Encoding.GetEncoding("ISO-8859-1").GetString(manifestBytes);
                Console.WriteLine("Used ISO-8859-1 encoding.");
            }

            // Convert Manifest
            ManifestConverter manifestConverter = new ManifestConverter();
            manifestConverter.GetManifest(rawManifest);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error Retrieving manifest: {e.Message}");
        }
    }

    private bool IsValidUtf8(string text)
    {
        return Encoding.UTF8.GetByteCount(text) == text.Length;
    }
}