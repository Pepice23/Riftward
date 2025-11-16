using Godot;

public partial class EnemySpawner : Node2D
{
    [Export] public PackedScene EnemyScene; // Assign in Inspector!
    [Export] public float SpawnCooldown = 2.0f; // Spawn every 2 seconds

    private float _spawnTimer = 1.0f; //Track time until next spawn

    // NEW: Spawn system
    public override void _Process(double delta)
    {
        // Count down the spawn timer
        _spawnTimer -= (float)delta;

        // Time to spawn?
        if (_spawnTimer <= 0f)
        {
            SpawnInTheCorner();
            _spawnTimer = SpawnCooldown; // Reset timer
        }
    }

    private void SpawnInTheCorner()
    {
        var spawnX = 0;
        var spawnY = 0;
        var edge = GD.RandRange(1, 4);
        switch (edge)
        {
            case 1:
                spawnX = -50;
                spawnY = (int)GD.RandRange(0, GetViewportRect().Size.Y);
                break;
            case 2:
                spawnX = (int)GetViewportRect().Size.X + 50; // Just off-screen to the right
                spawnY = (int)GD.RandRange(0, GetViewportRect().Size.Y); // Random along height
                break;
            case 3:
                spawnX = (int)GD.RandRange(0, GetViewportRect().Size.X);
                spawnY = -50; // Just off-screen above
                break;
            case 4:
                spawnX = (int)GD.RandRange(0, GetViewportRect().Size.X); // Random along width
                spawnY = (int)GetViewportRect().Size.Y + 50; // Just off-screen below
                break;
        }

        var position = new Vector2(spawnX, spawnY);

        // Create the enemy
        var enemy = EnemyScene.Instantiate<Enemy>();

        // Spawn it at player's position
        enemy.GlobalPosition = position;

        // Add it to the scene (as child of main scene, not player!)
        AddChild(enemy);
    }
}