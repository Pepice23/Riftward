using System.Threading.Tasks;
using Godot;

// ReSharper disable once CheckNamespace
public partial class Player
{
    //  Separate method to keep things organized
    private void CheckEnemyCollisions()
    {
        // Can't take damage yet? Skip checking
        if (_damageCooldown > 0f)
            return;

        // Loop through everything we just collided with
        for (var i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            var collider = collision.GetCollider();

            // Is it an enemy?
            if (collider is Enemy enemy)
            {
                TakeDamage(10);
                return; // Stop checking - we already got hurt this frame
            }

            // Is it an elite enemy?
            if (collider is EliteEnemy eliteEnemy)
            {
                TakeDamage(20);
                return; // Stop checking - we already got hurt this frame
            }
        }
    }

    // Take damage method
    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _damageCooldown = DamageCooldownTime; // Start invulnerability

        _ = FlashDamage(); // Start the flash dont wait
        UpdatePlayerHP(); // Update immediately

        // Did we die?
        if (_currentHealth <= 0) Die();
    }

    // Death method
    private void Die()
    {
        if (_isDead) return; // Already dead dont die twice

        _isDead = true;
        GameManager.Instance.EndRun(false);
        GD.Print("Player died!");

        // Stop moving
        Velocity = Vector2.Zero;
    }

    private void UpdatePlayerHP()
    {
        _healthBar.MaxValue = MaxHealth;
        _healthBar.Value = _currentHealth;
    }

    private async Task FlashDamage()
    {
        // Turn red
        _sprite.Modulate = new Color(1, 0, 0); // Pure red (R=1, G=0, B=0)

        // Wait 0.1 seconds
        await ToSignal(GetTree().CreateTimer(DamageFlashDuration), SceneTreeTimer.SignalName.Timeout);

        // Return to normal (white)
        _sprite.Modulate = new Color(1, 1, 1); // White (R=1, G=1, B=1)
    }

    private void HPRegeneration()
    {
        _regenAccumulator += HealthRegen;

        // Only heal when we've accumulated at least 1 HP
        if (_regenAccumulator >= 1f)
        {
            var healAmount = (int)_regenAccumulator;
            _currentHealth += healAmount;
            UpdatePlayerHP();
            _regenAccumulator -= healAmount; // Keep the leftover fraction
        }

        // Don't overheal
        if (_currentHealth > MaxHealth)
            _currentHealth = MaxHealth;
    }
}