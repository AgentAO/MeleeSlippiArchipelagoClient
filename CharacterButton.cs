using Godot;
using System;
using Newtonsoft.Json.Linq;

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
		this.FocusMode = FocusModeEnum.None;
	}
	
	public void OnGameStart()
	{	
		string[] ValidCharacters = ((JArray)ArchipelagoHandler.GetSlotData()["valid_characters"]).ToObject<string[]>();
		if( Array.IndexOf(ValidCharacters, CharacterName) < 0 )
		{
			Texture2D DisabledTexture = GD.Load<Texture2D>("res://art/disabled.png");
			this.TextureDisabled = DisabledTexture;
		}
		
		// Check our session data loaded win counts
		ArchipelagoHandler.GetSession().DataStorage[$"{CharacterName} Wins"].Initialize(0);
		if( ArchipelagoHandler.GetSession().DataStorage[$"{CharacterName} Wins"] > 0 )
		{
			WinCount = ArchipelagoHandler.GetSession().DataStorage[$"{CharacterName} Wins"];
		}
		
		// Check our location loaded win counts in case we unlocked more somehow
		if( Characters.GetCharacterHighestWins(CharacterName) > WinCount )
		{
			WinCount = Characters.GetCharacterHighestWins(CharacterName);
			ArchipelagoHandler.GetSession().DataStorage[$"{CharacterName} Wins"] = Characters.GetCharacterHighestWins(CharacterName);
		}
		
		if( WinCount > 0 )
		{
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
		ArchipelagoHandler.GetSession().DataStorage[$"{CharacterName} Wins"].Initialize(0);
		ArchipelagoHandler.GetSession().DataStorage[$"{CharacterName} Wins"] = WinCount;
		if( Characters.CheckWinCondition() )
		{
			ArchipelagoHandler.GetSession().SetGoalAchieved();
		}
	}
}
