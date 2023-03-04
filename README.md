# Multiplayer in Godot 4.0
I read the blog entry https://godotengine.org/article/multiplayer-in-godot-4-0-scene-replication/ from the Godot website and it intrigued me. I only had some (IMHO bad) experiences in Unity using Mirror. I knew the basics but all the recompiling manual restart annoyed and frustrated me. So I thought why not go through that experience again.
And I must say the Godot devs did an amazing job at simplifying the setup and process.

# The Setup
Back in the day I was writing a 2D platformer and wanted to have a multiplayer experience. So I decided to take the forementioned blog entry and not copy but adapt it for 2D purposes.
- Up to 4 players can spawn
- The level it automatically loaded by the server
- If the server is dedicated it should not spawn a player (not tested by the way)

# Gotchas
Some things are not yet fully implemented in C# I guess e.g. the relay server. But Godot has a generic setter for their internals so that is totally fine.
