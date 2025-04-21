using Godot;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;

public partial class CharacterButton : TextureButton
{	
	[Export]
	public string CharacterName {get;set;}
	public int WinCount {get;set;} = 0;
	public int MaxWinsNeeded = 0;
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
			Texture2D DisabledTexture = GD.Load<Texture2D>("res://art/disabled-x.png");
			this.TextureDisabled = null;
			TextureRect XNode = new TextureRect();
			XNode.Texture = DisabledTexture;
			Vector2 Size = new Vector2();
			Size.X = 152;
			Size.Y = 133;
			XNode.SetSize(Size);
			AddChild(XNode);
			Modulate = new Color("#707070");
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

		int[] WinsNeeded = ((JArray)ArchipelagoHandler.GetSlotData()["wins_needed"]).ToObject<int[]>();
		MaxWinsNeeded = WinsNeeded.Max();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if( !ArchipelagoHandler.IsReady() )
		{
			return;
		}
		
		if( Characters.GetUnlockedCharacters().IndexOf(CharacterName) > -1 )
		{
			this.Disabled = false;
		}
		
		if( WinCount >= (long)ArchipelagoHandler.GetSlotData()["required_wins_per_character"] )
		{
			SelfModulate = new Color("#00b637");
		}
		
		 if( WinCount >= MaxWinsNeeded )
		 {
		 	Counter.AddThemeColorOverride("font_color",new Color("#00ea00"));
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
