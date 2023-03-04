using System.Diagnostics;
using Godot;

public partial class Player : CharacterBody2D
{
	private const float jumpVelocity = 4.5f;
	
	private Vector2 ScreenSize => GetViewportRect().Size;
	private PlayerInput Input => GetNode<PlayerInput>("PlayerInput");

	private long internalId;

	[Export]
	public int Speed = 400;

	[Export]
	public long id
	{
		get => internalId;
		set
		{
			internalId = value;
			Input.SetMultiplayerAuthority((int)value);
		}
	}

	public override void _Ready()
	{
		if (id == Multiplayer.GetUniqueId())
			Trace.WriteLine("We are a local player");
	}

	public override void _PhysicsProcess(double delta)
	{
		var velocity = Input.direction;

		velocity = velocity.Normalized() * Speed;

		UpdatePosition(velocity * (float)delta);
	}

	private void UpdatePosition(Vector2 deltaPosition)
	{
		Position += deltaPosition;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
	}
}
