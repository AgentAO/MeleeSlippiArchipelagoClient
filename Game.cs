using Godot;
using System;
using System.Linq;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Enums;

public partial class Game : CanvasLayer
{	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public void OnGameStart()
	{
		if( Characters.GetUnlockedCharacters().Count == 0 )
		{
			Characters.UnlockCharacters(ArchipelagoHandler.GetSession().Items.AllItemsReceived.ToArray());
		}
		
		foreach( long Location in Locations.GetAllLocationsChecked() )
		{
			Locations.RegisterCharacterWinCounts(Location);
		}
		
		string WinsNeeded = string.Join(",",ArchipelagoHandler.GetSlotData()["required_wins_per_character"]);
		GD.Print( $"required_wins_per_character {WinsNeeded}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if( !ArchipelagoHandler.IsReady() )
		{
			return;
		}
	}
}
