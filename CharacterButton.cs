using Godot;
using System;

public partial class CharacterButton : TextureButton
{	
	[Export]
	public string CharacterName {get;set;}
	public int WinCount {get;set;} = 0;
	private Label Counter {get;set;}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Counter = GetNode<Label>("Counter");
	}
	
	public void OnGameStart()
	{
		if( Characters.GetCharacterHighestWins(CharacterName) > 0 )
		{
			WinCount = Characters.GetCharacterHighestWins(CharacterName);
			Counter.Text = WinCount.ToString();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if( Characters.GetUnlockedCharacters().IndexOf(CharacterName) > -1 )
		{
			this.Disabled = false;
		}
	}
	
	public void OnPressed()
	{
		WinCount++;
		Counter.Text = WinCount.ToString();
		Locations.SendWinLocation(CharacterName, WinCount);
		if( Characters.CheckWinCondition() )
		{
			ArchipelagoHandler.GetSession().SetGoalAchieved();
		}
	}
}
