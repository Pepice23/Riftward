using System.Linq;
using Godot;

// ReSharper disable once CheckNamespace
public partial class Player
{
    private void AddEnemiesToAura(Node2D body)
    {
        if (body is CharacterBody2D character and (Enemy or EliteEnemy))
        {
            _enemiesInAura.Add(character);
            DamageAuraEnemies();
        }
    }

    private void RemoveEnemiesFromAura(Node2D body)
    {
        if (body is CharacterBody2D character and (Enemy or EliteEnemy))
        {
            _enemiesInAura.Remove(character);
        }
    }

    private void UpdateCooldowns(double delta)
    {
        //  Count down damage cooldown
        if (_damageCooldown > 0f) _damageCooldown -= (float)delta;

        // Count down the attack timer
        _attackTimer -= (float)delta;
        _auraDamageTimer -= (float)delta;
    }

    private void HandleAttacking()
    {
        // Time to shoot?
        if (_attackTimer <= 0f)
        {
            ShootAtNearestEnemy();
            _attackTimer = AttackCooldown; // Reset timer
        }
    }

    //  Find and shoot at nearest enemy
    private void ShootAtNearestEnemy()
    {
        // Make sure we have a projectile scene assigned
        if (ProjectileScene == null)
            return;

        // Find all enemies
        var enemies = GetTree().GetNodesInGroup("enemies");
        if (enemies.Count == 0)
            return; // No enemies to shoot

        // Find the N nearest enemies using LINQ
        var nearestEnemies = enemies
            .OfType<CharacterBody2D>() // Only CharacterBody2D nodes
            .Where(e => e is Enemy or EliteEnemy) // Only Enemy or EliteEnemy types
            .OrderBy(e => GlobalPosition.DistanceTo(e.GlobalPosition)) // Sort by distance (closest first)
            .Take(ProjectileCount) // Take only the first N enemies
            .ToList(); // Convert to a list

        // Shoot one projectile at each target
        foreach (var target in nearestEnemies)
        {
            SpawnProjectile(target.GlobalPosition);
        }
    }

    //  Actually create and fire the projectile
    private void SpawnProjectile(Vector2 targetPosition)
    {
        // Create the projectile
        var projectile = ProjectileScene.Instantiate<Projectile>();

        // Spawn it at player's position
        projectile.GlobalPosition = GlobalPosition;

        // Aim it at the target
        var direction = (targetPosition - GlobalPosition).Normalized();

        projectile.SetDirection(direction);

        // Set the projectile Damage and Speed
        projectile.Damage = Damage;
        projectile.Speed = ProjectileSpeed;

        // Add it to the scene (as child of main scene, not player!)
        GetParent().AddChild(projectile);
    }

    private void DamageAuraEnemies()
    {
        foreach (var enemy in _enemiesInAura)
        {
            switch (enemy)
            {
                case Enemy regularEnemy:
                    regularEnemy.TakeDamage(Damage);
                    _lifeLeechAccumulator += regularEnemy.Damage * AuraLifeLeech;
                    break;
                case EliteEnemy eliteEnemy:
                    eliteEnemy.TakeDamage(Damage);
                    _lifeLeechAccumulator += eliteEnemy.Damage * AuraLifeLeech;
                    break;
            }
        }
        // After damaging all enemies, check if we've accumulated enough to heal
        if (_lifeLeechAccumulator >= 1f)
        {
            var healAmount = (int)_lifeLeechAccumulator;
            Heal(healAmount);
            _lifeLeechAccumulator -= healAmount; // Keep the leftover fraction
        }
    }
}