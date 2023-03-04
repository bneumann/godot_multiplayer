using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class Level : Node2D
{
	const float SpawnRandom = 5.0f;
	private Node Players => GetNode<Node>("Players");
	public override void _Ready()
	{
		Trace.WriteLine("Level loaded");
		if (!Multiplayer.IsServer()) return;

		Multiplayer.PeerConnected += AddPlayer;
		Multiplayer.PeerDisconnected += RemovePlayer;

		SpawnConnectedPlayers();

		SpawnLocalPlayer();
	}

	public override void _ExitTree()
	{
		if (!Multiplayer.IsServer())
			return;

		Multiplayer.PeerConnected -= AddPlayer;
		Multiplayer.PeerDisconnected -= RemovePlayer;
	}

	private void SpawnConnectedPlayers()
	{
		foreach (var id in Multiplayer.GetPeers())
			AddPlayer(id);
	}

	private void SpawnLocalPlayer()
	{
		if (!OS.HasFeature("dedicated_server"))
			AddPlayer(1);
	}

	private void AddPlayer(long id)
	{
		var packedScene = (ResourceLoader.Load("res://Player.tscn") as PackedScene);
		var character = (Player)packedScene?.Instantiate();
		if (character == null)
		{
			OS.Alert("Cannot instantiate player.");
			return;
		}
		Trace.WriteLine($"Adding player {id}");
		character.id = id;
		character.Position = GetViewportRect().Size * GD.Randf();
		character.Name = GetNameFromId(id);
		Players.AddChild(character, true);

	}

	private static string GetNameFromId(long id) => $"{id}";

	private void RemovePlayer(long id)
	{
		if (!Players.HasNode(GetNameFromId(id))) return;
		Players.GetNode(GetNameFromId(id)).QueueFree();
	}
}
