using System.Text.Json;

class ManifestSelector
{
    public async void ReadManifest()
    {
        try
        {
            string manifestData = File.ReadAllText("Resources/manifests.json");
            List<ManifestEntry> manifestEntries = JsonSerializer.Deserialize<List<ManifestEntry>>(manifestData);

            // By Season
            var seasons = new Dictionary<string, List<ManifestEntry>>();
            foreach (var entry in manifestEntries)
            {
                string season = entry.SeasonNumber;
                if (!seasons.ContainsKey(season))
                {
                    seasons[season] = new List<ManifestEntry>();
                }
                seasons[season].Add(entry);
            }

            while (true)
            {
                // Display Available Seasons
                Console.WriteLine("Available Seasons:");
                int seasonIndex = 0;
                foreach (var season in seasons)
                {
                    Console.WriteLine($"[{seasonIndex}] {season.Key}");
                    seasonIndex++;
                }

                Console.WriteLine("\nChoose Number for Season:");
                string chosenSeasonNumber = Console.ReadLine();

                // Return to Seasons
                if (chosenSeasonNumber.ToLower() == "back")
                {
                    Console.Clear();
                    continue;
                }

                if (int.TryParse(chosenSeasonNumber, out int selectedSeasonIndex) && selectedSeasonIndex >= 0 && selectedSeasonIndex < seasons.Count)
                {
                    // Display Seasons
                    string chosenSeason = seasons.Keys.ElementAt(selectedSeasonIndex);
                    List<ManifestEntry> chosenSeasonManifests = seasons[chosenSeason];

                    Console.Clear();
                    Console.WriteLine("UEPakInstaller Created by Ioan and CreationBender\n");
                    Console.WriteLine($"Chosen: {chosenSeason}\n");

                    // Display Versions
                    Console.WriteLine("Available Versions:");
                    for (int i = 0; i < chosenSeasonManifests.Count; i++)
                    {
                        Console.WriteLine($"[{i}] {chosenSeasonManifests[i].Version}");
                    }

                    Console.WriteLine("\nChoose Version or type 'back' to choose different season:");
                    string chosenVersionNumber = Console.ReadLine();

                    if (chosenVersionNumber.ToLower() == "back")
                    {
                        Console.Clear();
                        continue;
                    }

                    if (int.TryParse(chosenVersionNumber, out int selectedVersionIndex) && selectedVersionIndex >= 0 && selectedVersionIndex < chosenSeasonManifests.Count)
                    {
                        string chosenVersion = chosenSeasonManifests[selectedVersionIndex].Version;
                        Console.Clear();
                        Console.WriteLine("UEPakInstaller Created by Ioan and CreationBender\n");
                        Console.WriteLine($"Chosen: {chosenSeason}\n");

                        // Display Versions
                        Console.WriteLine("Available Versions:");
                        for (int i = 0; i < chosenSeasonManifests.Count; i++)
                        {
                            Console.WriteLine($"[{i}] {chosenSeasonManifests[i].Version}");
                        }

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"\nChosen Version: {chosenVersion}\n");
                        Console.ResetColor();

                        // Chosen Manifest ID
                        string chosenManifestID = chosenSeasonManifests[selectedSeasonIndex].Manifest_ID;
                        GetManifest getManifest = new GetManifest();
                        await getManifest.OpenManifestAsync(chosenManifestID);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input. Please enter a valid number.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please enter a valid number.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

public class ManifestEntry
{
    public string Version { get; set; }
    public string Manifest_ID { get; set; }
    public string SeasonNumber { get; set; }
}
