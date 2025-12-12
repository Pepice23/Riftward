using Godot;
using System;
using System.Collections.Generic;

public partial class Trainer : PanelContainer
{
    [Export] public PackedScene HubUpgradeButtonScene; // Set in Inspector!

    public enum TrainerType
    {
        Paladin,
        Mage,
        Hunter
    }

    private VBoxContainer _upgradeContainer;
    private Label _trainerName;
    private TextureRect _trainerPortrait;
    private Label _availableGold;

    private readonly List<HubUpgrade> _hubUpgrades = [];

    public static TrainerType SelectedTrainer; // Set BEFORE scene loads

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _upgradeContainer = GetNode<VBoxContainer>("%UpgradeContainer");
        _trainerName = GetNode<Label>("%TrainerLabel");
        _trainerPortrait = GetNode<TextureRect>("%TrainerPicture");
        _availableGold = GetNode<Label>("%AvailableGoldNumber");

        _availableGold.Text = GameManager.Instance.HubGold.ToString();

        // Check which trainer we are
        if (SelectedTrainer == TrainerType.Paladin)
        {
            _hubUpgrades.AddRange(HubUpgradeManager.Instance.GenericUpgrades);
            _hubUpgrades.AddRange(HubUpgradeManager.Instance.PaladinUpgrades);

            _trainerName.Text = "Paladin Trainer";
            _trainerPortrait.Texture = GD.Load<Texture2D>("res://Assets/Art/paladin_trainer.png");
        }

        if (SelectedTrainer == TrainerType.Mage)
        {
            _hubUpgrades.AddRange(HubUpgradeManager.Instance.GenericUpgrades);
            _hubUpgrades.AddRange(HubUpgradeManager.Instance.MageUpgrades);
            _trainerName.Text = "Mage Trainer";
            _trainerPortrait.Texture = GD.Load<Texture2D>("res://Assets/Art/mage_trainer.png");
        }

        if (SelectedTrainer == TrainerType.Hunter)
        {
            _hubUpgrades.AddRange(HubUpgradeManager.Instance.GenericUpgrades);
            _hubUpgrades.AddRange(HubUpgradeManager.Instance.HunterUpgrades);
            _trainerName.Text = "Hunter Trainer";
            _trainerPortrait.Texture = GD.Load<Texture2D>("res://Assets/Art/hunter_trainer.png");
        }

        foreach (var hubUpgrade in _hubUpgrades)
        {
            // Create button
            var upgradeButton = HubUpgradeButtonScene.Instantiate<HubUpgradeButton>();

            // Figure out the cost
            var cost = 0;
            if (hubUpgrade.CurrentRank < hubUpgrade.MaxRank)
            {
                cost = hubUpgrade.CostPerRank[hubUpgrade.CurrentRank];
            }

            // Add to the container so it appears on screen
            _upgradeContainer.AddChild(upgradeButton);

            // Fill in the button data
            upgradeButton.SetUpgradeInfo(hubUpgrade.Name, hubUpgrade.Description,
                hubUpgrade.CurrentRank, hubUpgrade.MaxRank, cost);

            // Wire up the click to purchase logic
            upgradeButton.Pressed += () => OnUpgradePurchased(hubUpgrade, upgradeButton);
        }
    }

    private void OnUpgradePurchased(HubUpgrade upgrade, HubUpgradeButton button)
    {
        // Check if already maxed
        if (upgrade.CurrentRank >= upgrade.MaxRank)
        {
            button.Disabled = true;
            return;
        }

        // Get the cost
        var cost = upgrade.CostPerRank[upgrade.CurrentRank];

        // Check if player can afford it
        if (GameManager.Instance.HubGold < cost)
        {
            button.Disabled = true;
            return;
        }

        // Purchase it
        GameManager.Instance.HubGold -= cost;
        _availableGold.Text = GameManager.Instance.HubGold.ToString();
        upgrade.CurrentRank++;
        SaveManager.Instance.Save();

        GD.Print($"Purchased {upgrade.Name}! Rank {upgrade.CurrentRank}/{upgrade.MaxRank}");

        // Update the button display
        var newCost = 0;
        if (upgrade.CurrentRank < upgrade.MaxRank)
        {
            newCost = upgrade.CostPerRank[upgrade.CurrentRank];
        }

        button.SetUpgradeInfo(upgrade.Name, upgrade.Description, upgrade.CurrentRank, upgrade.MaxRank, newCost);
    }

    private void BackToHub()
    {
        GetTree().ChangeSceneToFile("res://UI/hub.tscn");
    }
}