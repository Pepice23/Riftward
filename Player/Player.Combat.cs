using Godot;

// ReSharper disable once CheckNamespace
public partial class Player
{
    private void AddEnemiesToAura(Node2D body)
    {
        if (body is CharacterBody2D character and (Enemy or EliteEnemy))
        {
            _enemiesInAura.Add(character);
            GD.Print("Enemies entered the area");
        }
    }

    private void RemoveEnemiesFromAura(Node2D body)
    {
        if (body is CharacterBody2D character and (Enemy or EliteEnemy))
        {
            _enemiesInAura.Remove(character);
            GD.Print("Enemies exited the area");
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

        // Find the closest enemy
        CharacterBody2D nearestTarget = null;
        var nearestDistance = float.MaxValue;

        foreach (var node in enemies)
        {
            if (node is Enemy enemy)
            {
                var distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = enemy;
                }
            }

            if (node is EliteEnemy eliteEnemy)
            {
                var distance = GlobalPosition.DistanceTo(eliteEnemy.GlobalPosition);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = eliteEnemy;
                }
            }
        }


        // Shoot at the nearest enemy
        if (nearestTarget != null) SpawnProjectile(nearestTarget.GlobalPosition);
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
            if (enemy is Enemy regularEnemy)
                regularEnemy.TakeDamage(Damage);
            else if (enemy is EliteEnemy eliteEnemy) eliteEnemy.TakeDamage(Damage);
    }
}