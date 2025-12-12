using Godot;
using Godot.Collections;

public partial class SaveManager : Node
{
    public static SaveManager Instance { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        Load();
    }

    public void Save()
    {
        // Create the upgrades dictionary
        var upgrades = new Dictionary<string, Variant>();

        // Loop through GenericUpgrades and add each one
        foreach (var upgrade in HubUpgradeManager.Instance.GenericUpgrades)
        {
            upgrades[upgrade.Name] = upgrade.CurrentRank;
        }

        // Repeat for each upgrade list
        foreach (var upgrade in HubUpgradeManager.Instance.PaladinUpgrades)
        {
            upgrades[upgrade.Name] = upgrade.CurrentRank;
        }

        foreach (var upgrade in HubUpgradeManager.Instance.MageUpgrades)
        {
            upgrades[upgrade.Name] = upgrade.CurrentRank;
        }

        foreach (var upgrade in HubUpgradeManager.Instance.HunterUpgrades)
        {
            upgrades[upgrade.Name] = upgrade.CurrentRank;
        }

        var saveData = new Dictionary<string, Variant>
            { { "hubGold", GameManager.Instance.HubGold }, { "upgrades", upgrades } };


        // Convert dictionary to JSON string
        var json = Json.Stringify(saveData);

        // Open a file for writing (creates it if it doesn't exist)
        var file = FileAccess.Open("user://savegame.json", FileAccess.ModeFlags.Write);

        // Write the JSON string to the file
        file.StoreString(json);

        // Close the file
        file.Close();

        GD.Print("Game saved successfully!");
    }

    private void Load()
    {
        // Check if file exists
        if (!FileAccess.FileExists("user://savegame.json"))
        {
            GD.Print("No save file found");
            return;
        }

        // Open file for reading
        var file = FileAccess.Open("user://savegame.json", FileAccess.ModeFlags.Read);

        // Read the text
        var json = file.GetAsText();

        // Close file
        file.Close();

        // Convert JSON back to dictionary
        var saveData = Json.ParseString(json).AsGodotDictionary();

        // Get the upgrades dictionary from save data
        var upgrades = saveData["upgrades"].AsGodotDictionary();

        // Apply hub gold
        GameManager.Instance.HubGold = saveData["hubGold"].AsInt32();

        // Apply upgrade ranks - loop through all upgrades and find matches
        foreach (var upgrade in HubUpgradeManager.Instance.GenericUpgrades)
        {
            if (upgrades.ContainsKey(upgrade.Name))
            {
                upgrade.CurrentRank = upgrades[upgrade.Name].AsInt32();
            }
        }

        foreach (var upgrade in HubUpgradeManager.Instance.PaladinUpgrades)
        {
            if (upgrades.ContainsKey(upgrade.Name))
            {
                upgrade.CurrentRank = upgrades[upgrade.Name].AsInt32();
            }
        }

        foreach (var upgrade in HubUpgradeManager.Instance.MageUpgrades)
        {
            if (upgrades.ContainsKey(upgrade.Name))
            {
                upgrade.CurrentRank = upgrades[upgrade.Name].AsInt32();
            }
        }

        foreach (var upgrade in HubUpgradeManager.Instance.HunterUpgrades)
        {
            if (upgrades.ContainsKey(upgrade.Name))
            {
                upgrade.CurrentRank = upgrades[upgrade.Name].AsInt32();
            }
        }

        // Print it to verify
        GD.Print("Loaded save data: ", saveData);
    }
}