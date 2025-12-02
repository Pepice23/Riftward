using Godot;

// ReSharper disable CheckNamespace

#pragma warning disable CA1050
public partial class EnemySpawner : Node2D
#pragma warning restore CA1050
{
    [Export] public PackedScene EnemyScene; // Assign in Inspector!
    [Export] public PackedScene EliteEnemyScene; // Assign in Inspector!
    [Export] public PackedScene BossEnemyScene; // Assign in Inspector!
    [Export] public float SpawnCooldown = 2.0f; // Spawn every 2 seconds
    [Export] public float EliteSpawnCooldown = 90.0f; // Spawn an elite in every 90 seconds

    private float _spawnTimer = 1.0f; //First spawn time
    private float _eliteSpawnTimer = 90.0f;
    private Player _player;


    public override void _Ready()
    {
        _player = GetTree().Root.FindChild("Player", true, false) as Player;
        if (GameManager.Instance != null) GameManager.Instance.BossTime += SpawnBossEnemy;
    }

// NEW: Spawn system
    public override void _Process(double delta)
    {
        // Count down the spawn timer
        _spawnTimer -= (float)delta;
        _eliteSpawnTimer -= (float)delta;

        // Time to spawn?
        if (_spawnTimer <= 0f)
        {
            var enemiesToSpawn = CalculateSpawnCount();

            for (var i = 0; i < enemiesToSpawn; i++)
            {
                SpawnInTheCorner();
            }

            _spawnTimer = SpawnCooldown; //Reset timer
        }

        if (_eliteSpawnTimer <= 0f)
        {
            for (var i = 0; i < 3; i++)
            {
                SpawnEliteEnemy();
            }

            _eliteSpawnTimer = EliteSpawnCooldown; // Reset timer
        }

        
    }

    private void SpawnInTheCorner()
    {
        var position = SetSpawnPosition();

        // Create the enemy
        var enemy = EnemyScene.Instantiate<Enemy>();
        enemy.AddToGroup("normal_enemies");

        // Set the enemy's health to the current scaled value
        enemy.MaxHealth = GameManager.Instance.CurrentEnemyMaxHealth;


        // Spawn it at player's position
        enemy.GlobalPosition = position;


        // Add it to the scene (as child of main scene, not player!)
        AddChild(enemy);
    }


    private void SpawnEliteEnemy()
    {
        var position = SetSpawnPosition();

        // Create the enemy
        var enemy = EliteEnemyScene.Instantiate<EliteEnemy>();
        enemy.AddToGroup("normal_enemies");

        enemy.MaxHealth = GameManager.Instance.CurrentEliteEnemyMaxHealth;


        // Spawn it at player's position
        enemy.GlobalPosition = position;


        // Add it to the scene (as child of main scene, not player!)
        AddChild(enemy);
    }
    
    private void SpawnBossEnemy()
    {
        var position = SetSpawnPosition();

        // Create the enemy
        var enemy = BossEnemyScene.Instantiate<BossEnemy>();
        
        // Spawn it at player's position
        enemy.GlobalPosition = position;


        // Add it to the scene (as child of main scene, not player!)
        AddChild(enemy);
    }

    private Vector2 SetSpawnPosition()
    {
        if (_player == null)
        {
            return  Vector2.Zero;
        }

        const float spawnDistance = 1000f; //Distance from player to spawn
        var spawnX = 0f;
        var spawnY = 0f;
        var edge = GD.RandRange(1, 4);
        switch (edge)
        {
            case 1: // Left of the player
                spawnX = _player.GlobalPosition.X - spawnDistance;
                spawnY = _player.GlobalPosition.Y + (float)GD.RandRange(-spawnDistance, spawnDistance);
                break;
            case 2: // Right of the player
                spawnX = _player.GlobalPosition.X + spawnDistance;
                spawnY = _player.GlobalPosition.Y + (float)GD.RandRange(-spawnDistance, spawnDistance);
                break;
            case 3: // Above the player
                spawnX = _player.GlobalPosition.X + (float)GD.RandRange(-spawnDistance, spawnDistance);
                spawnY = _player.GlobalPosition.Y - spawnDistance;
                break;
            case 4: // Below the player
                spawnX = _player.GlobalPosition.X + (float)GD.RandRange(-spawnDistance, spawnDistance);
                spawnY = _player.GlobalPosition.Y + spawnDistance;
                break;
        }

        var position = new Vector2(spawnX, spawnY);
        return position;
    }

    private int CalculateSpawnCount()
    {
        var minutes = GameManager.Instance.RunTime / 60f; // Convert seconds to minutes

        switch (minutes)
        {
            case < 2f:
                return 3;
            case < 5f:
                return 4;
        }

        return 0;
    }
    
    public override void _ExitTree()
    {
        if (GameManager.Instance != null) GameManager.Instance.BossTime -= SpawnBossEnemy;
    }
}