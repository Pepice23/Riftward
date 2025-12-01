using System.Collections.Generic;
using Godot;

#pragma warning disable CA1050
// ReSharper disable once CheckNamespace
public partial class Player : CharacterBody2D
#pragma warning restore CA1050
{
    #region Signals

    [Signal]
    public delegate void XPUpdatedEventHandler(int xpNeededForNextLevel, int currentXP);

    [Signal]
    public delegate void LevelUpdatedEventHandler(int currentLevel);

    [Signal]
    public delegate void GamePausedEventHandler();

    #endregion

    #region Exports

    // Movement
    [Export] public float Speed = 300.0f;
    [Export] public Texture2D FrontSprite;
    [Export] public Texture2D BackSprite;


    // Combat
    [Export] public PackedScene ProjectileScene; // Assign in Inspector!
    [Export] public float AttackCooldown = 1.0f; // Shoot every 1 second
    [Export] public float AuraDamageCooldown = 0.5f;
    [Export] public int Damage = 3;
    [Export] public int ProjectileSpeed = 400;
    [Export] public int ProjectileCount = 1;


    // Health
    [Export] public int MaxHealth = 100;
    [Export] public float DamageFlashDuration = 0.1f; // How long to flash red when hit
    [Export] public float HealthRegen = 0.01f;

    // XP
    [Export] public int CurrentLevel = 1;
    [Export] public int CurrentXP;
    [Export] public int BaseXPNeeded = 10; // XP needed for level 2
    [Export] public LevelUpUi LevelUpUi;
    [Export] public Hud Hud;

    // Paladin Aura
    [Export] public float HammerRotationSpeed = 2.0f; // Rotations per second
    [Export] public int AuraRadius = 100;
    [Export] public float AuraLifeLeech;

    #endregion

    #region Private Fields

    // References
    private ProgressBar _healthBar;
    private Sprite2D _sprite;
    private Area2D _area;
    private Area2D _hitboxArea;
    private Node2D _hammerAura;
    private GpuParticles2D _snowFall;

    // Combat state
    private float _attackTimer = 1.0f; //Track time until next shot
    private float _auraDamageTimer = 0.5f;

    // Health State
    private int _currentHealth;
    private float _damageCooldown;
    private const float DamageCooldownTime = 0.5f;
    private bool _isDead;
    private float _regenAccumulator;
    private float _lifeLeechAccumulator;

    // XP
    private int _xpNeededForNextLevel;

    private readonly List<BaseEnemy> _enemiesInAura = [];
    private readonly List<BaseEnemy> _enemiesInHitbox = [];
    private CollisionShape2D _collisionShape;

    #endregion


    #region Lifecycle Methods

    public override void _Ready()
    {
        // Cache the sprite reference
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _healthBar = GetNode<ProgressBar>("%ProgressBar");
        _area = GetNode<Area2D>("%PaladinAura");
        _hitboxArea = GetNode<Area2D>("%Hitbox");
        _hammerAura = GetNode<Node2D>("HammerAura");
        _collisionShape = GetNode<CollisionShape2D>("%AuraCollision");
        _snowFall = GetNode<GpuParticles2D>("%SnowFall");
        // Initialize health
        _currentHealth = MaxHealth;
        UpdatePlayerHP();
        // Initialize XP
        CalculateXPNeeded();

        if (GameManager.Instance.IsWinterModeEnabled)
        {
            _snowFall.Emitting = true;
        }
        else
        {
            _snowFall.Emitting = false;
        }

        switch (GameManager.Instance.SelectedClass)
        {
            case GameManager.PlayerClass.Paladin:
                SetupPaladin();
                break;
            case GameManager.PlayerClass.Mage:
                SetupMage();
                break;
            case GameManager.PlayerClass.Hunter:
                SetupHunter();
                break;
            default:
                SetupPaladin();
                break;
        }


        GameManager.Instance.StartRun();
        _area.BodyEntered += AddEnemiesToAura;
        _area.BodyExited += RemoveEnemiesFromAura;
        _hitboxArea.BodyEntered += AddEnemyToHitbox;
        _hitboxArea.BodyExited += RemoveEnemyFromHitbox;
    }

    public override void _ExitTree()
    {
        // Clean up signal connections to prevent memory leaks
        _area.BodyEntered -= AddEnemiesToAura;
        _area.BodyExited -= RemoveEnemiesFromAura;
        _hitboxArea.BodyEntered -= AddEnemyToHitbox;
        _hitboxArea.BodyExited -= RemoveEnemyFromHitbox;
    }

    // This runs every physics frame (60 times per second)
    public override void _PhysicsProcess(double delta)
    {
        // NEW: Don't move if dead
        if (_isDead)
            return;

        HandleMovement(delta);
        // Clamping player to the scene
        // Position = new Vector2(
        //     Mathf.Clamp(Position.X, ArenaMinX, ArenaMaxX),
        //     Mathf.Clamp(Position.Y, ArenaMinY, ArenaMaxY)
        // );
        // Continuously check for collisions with enemies
        CheckEnemyCollisions();
    }

    public override void _Process(double delta)
    {
        // NEW: Don't attack if dead
        if (_isDead)
            return;

        if (_currentHealth < MaxHealth)
        {
            HPRegeneration();
        }

        UpdateCooldowns(delta);
        // Only shoot projectiles if NOT Paladin
        if (GameManager.Instance.SelectedClass != GameManager.PlayerClass.Paladin)
            HandleAttacking(); // Your projectile shooting

        // Only do aura damage if IS Paladin
        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Paladin)
        {
            if (_auraDamageTimer <= 0f)
            {
                DamageAuraEnemies();
                _auraDamageTimer = AuraDamageCooldown;
            }


            // Show hammer aura for Paladin
            if (_hammerAura != null)
            {
                _hammerAura.Visible = true;
            }
            else
            {
                // Hide hammer for non-Paladin
                if (_hammerAura != null)
                    _hammerAura.Visible = false;
            }

            // Rotate hammer aura
            if (_hammerAura != null) _hammerAura.Rotation += HammerRotationSpeed * Mathf.Tau * (float)delta;
        }
    }

    #endregion
}