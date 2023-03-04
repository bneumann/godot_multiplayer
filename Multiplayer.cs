using Godot;
using System.Diagnostics;

public partial class Multiplayer : Node
{
	private const int Port = 4433;
	private Control Ui => GetNode<Control>("UI");
	private Control RemoteAddressField => GetNode<LineEdit>("UI/Net/Options/Remote");
	private Node Level => GetNode<Node>("Level");

	public override void _Ready()
	{
		// https://godotengine.org/article/multiplayer-in-godot-4-0-scene-replication/
		// The article sets the state to paused which causes the callbacks to not being called!
		//GetTree().Paused = true;

		

		Multiplayer.Set("server_relay", false);

		if (DisplayServer.GetName() == "headless")
			CallDeferred(nameof(OnConnectPressed));
	}

	public void OnHostPressed()
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateServer(Port);
		if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
		{
			OS.Alert("Failed to start multiplayer server.");
			return;
		}

		Multiplayer.MultiplayerPeer = peer;
		StartGame();
	}

	public void OnConnectPressed()
	{
		var address = RemoteAddressField.Get("text").AsString();
		if (string.IsNullOrWhiteSpace(address))
		{
			OS.Alert("Need a remote to connect to.");
			return;
		}

		var peer = new ENetMultiplayerPeer();
		peer.CreateClient(address, Port);
		if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
		{
			OS.Alert("Failed to connect to server");
			return;
		}

		Multiplayer.MultiplayerPeer = peer;
		StartGame();
	}
	
	private void StartGame()
	{
		Ui.Hide();
		GetTree().Paused = false;

		if (Multiplayer.IsServer())
			CallDeferred(nameof(ChangeLevel), ResourceLoader.Load("res://Level.tscn"));
	}

	private void ChangeLevel(PackedScene scene)
	{
		Trace.WriteLine("Calling ChangeLevel");
		RemoveOldLevel();
		Level.AddChild(scene.Instantiate());
	}

	private void RemoveOldLevel()
	{
		foreach (var child in Level.GetChildren())
		{
			Level.RemoveChild(child);
			child.QueueFree();
		}
	}
}
