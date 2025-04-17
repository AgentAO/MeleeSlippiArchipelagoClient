using Godot;
using System;
using System.Linq;
using Archipelago.MultiClient.Net.Models;

public partial class Game : CanvasLayer
{
	public Label MessageHolder;
	public bool VictoryAchieved = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MessageHolder = GetNode<Label>("Message Holder/Message Label");
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
		
		if( Characters.CheckWinCondition() )
		{
			VictoryAchieved = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if( !ArchipelagoHandler.IsReady() )
		{
			return;
		}
		
		if( !VictoryAchieved && Characters.CheckWinCondition() )
		{
			VictoryAchieved = true;
		}
		
		if( VictoryAchieved )
		{
			MessageHolder.Text = "Victory achieved, congratuations!";
		}
		
		if( !VictoryAchieved && ArchipelagoHandler.GetSession().Items.Any() )
		{
			ItemInfo NewItem = ArchipelagoHandler.GetSession().Items.DequeueItem();
			
			Characters.UnlockCharacter(NewItem);
			string ItemName = NewItem.ItemDisplayName;
			string ItemFrom = NewItem.Player.Alias;
			MessageHolder.Text = $"Recieved {ItemName} from {ItemFrom}";
		}
	}
}
