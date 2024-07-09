using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class UEPakInstaller
{
    static async Task Main()
    {
        try
        {
            // Set Directory
            Console.WriteLine("UEPakInstaller Created by @Ioan_123 and @CreationBender\n");
            Console.WriteLine("Enter builds output directory:");
            string directoryPath = (Console.ReadLine() ?? string.Empty).Trim();

            // Check if Directory Exists
            if (Directory.Exists(directoryPath))
            {
                Console.Clear();
                Console.WriteLine("UEPakInstaller Created by @Ioan_123 and @CreationBender\n");
                Console.WriteLine($"Builds Directory: {directoryPath}\n");
                GetManifestData getManifestData = new GetManifestData();
                
                Console.WriteLine("Enter manifest ID:");
                string manifestId = Console.ReadLine() ?? string.Empty;
                
                await getManifestData.OpenManifestAsync(manifestId);
            }
            else
            {
                Console.WriteLine("Directory does not exist or is inaccessible.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}