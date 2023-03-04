using Godot;
using System;

public partial class PlayerInput : MultiplayerSynchronizer
{
	[Export]
	public bool Jumping = false;

	// It seems that variables that are synchronized over the network need to be lowercase
	[Export]
	public Vector2 direction = new();

	public override void _Ready() =>
		// Only process for the local player. This means, that if we are the local player, this Node will receive the _Process call from the Engine
		SetProcess(GetMultiplayerAuthority() == Multiplayer.GetUniqueId());

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (Input.IsActionPressed("jump"))
			Rpc(nameof(Jump));
	}

	[Rpc(CallLocal = true)]
	private void Jump() => Jumping = true;
}
