using Godot;

// ReSharper disable once CheckNamespace
#pragma warning disable CA1050
public partial class Player
#pragma warning restore CA1050
{
    private void SetupPaladin()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/paladin/paladin_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/paladin/paladin_back.png");
        _sprite.Texture = FrontSprite;

        UpdateHammerPositions(AuraRadius);
    }

    private void SetupMage()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/mage/mage_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/mage/mage_back.png");
        _sprite.Texture = FrontSprite;
    }

    private void SetupHunter()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/hunter/hunter_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/hunter/hunter_back.png");
        _sprite.Texture = FrontSprite;
    }


    public void UpdateHammerPositions(float radius)
    {
        var hammers = _hammerAura.GetChildren();
        var hammerCount = hammers.Count;

        for (var i = 0; i < hammerCount; i++)
        {
            // Calculate angle for this hammer (evenly distributed)
            var angle = i * (Mathf.Tau / hammerCount);

            // Calculate position on circle
            var x = radius * Mathf.Cos(angle);
            var y = radius * Mathf.Sin(angle);

            // Set the hammer's position
            if (hammers[i] is Node2D hammer) hammer.Position = new Vector2(x, y);
        }
    }
}