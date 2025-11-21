using Godot;

// ReSharper disable once CheckNamespace
public partial class Player
{
    private void HandleMovement(double delta)
    {
        // Get input direction (WASD or arrow keys)
        // Returns Vector2 like (-1, 0) for left, (1, 0) for right, etc.
        var direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        // Set velocity based on direction and speed
        // Normalized() makes diagonal movement same speed as straight
        if (direction != Vector2.Zero)
        {
            // Move the character
            Velocity = direction.Normalized() * Speed;
            UpdateSpriteDirection(direction);
        }

        else
        {
            Velocity = Vector2.Zero; // Stop when no input
        }

        // Actually move the character (Godot handles collision automatically)
        MoveAndSlide();
    }

    private void UpdateSpriteDirection(Vector2 direction)
    {
        // Change Sprite based on vertical movement
        if (direction.Y < -0.1f) // Moving up (away from camera)
            _sprite.Texture = BackSprite;
        else // Moving down, left, right, or diagonal
            _sprite.Texture = FrontSprite;

        // Flip sprite based on horizontal movement
        if (direction.X > 0.01f)
            _sprite.FlipH = false; // Moving right
        else if (direction.X < -0.01f) _sprite.FlipH = true; // Moving left
        // If only moving up/down, keep current flip state
    }
}