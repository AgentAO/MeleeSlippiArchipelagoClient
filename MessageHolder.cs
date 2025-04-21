using Godot;
using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Enums;

public partial class MessageHolder : TextureRect
{
	[Signal]
	public delegate void IncomingTextEventHandler(string line);
	
	private List<string> AllMessages {get;set;} = new List<string>();
	private int CurMessage {get;set;} = 0;
	private Label MessageLabel {get;set;}
	private Label ScrollLabel {get;set;}
	private TextureButton PrevButton {get;set;}
	private TextureButton NextButton {get;set;}
	private AudioStreamPlayer2D SoundEffects {get;set;}
	private AudioStream TrophyUnlockSound {get;set;}
	private AudioStream CharacterUnlockSound {get;set;}
	private AudioStream VictorySound {get;set;}
	public bool VictoryAchieved = false;
	private bool ForceAutoScrollOff = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MessageLabel = GetNode<Label>("Message Label");
		ScrollLabel = GetNode<Label>("Scroll Label");
		PrevButton = GetNode<TextureButton>("Prev Message");
		NextButton = GetNode<TextureButton>("Next Message");
		SoundEffects = GetNode<AudioStreamPlayer2D>("Sound Effects");
		TrophyUnlockSound = GD.Load<AudioStream>("res://sounds/s_info1.hps.wav");
		CharacterUnlockSound = GD.Load<AudioStream>("res://sounds/s_info2.hps.wav");
		VictorySound = GD.Load<AudioStream>("res://sounds/s_info3.hps.wav");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if( !ArchipelagoHandler.IsReady() )
		{
			return;
		}
		
		if( AllMessages.Count > 0 )
		{
			MessageLabel.Text = AllMessages[CurMessage];
		}
		
		while( Locations.HasUnhandledSentLocation() )
		{
			Write(Locations.DequeueSentLocation());
		}
		
		PrevButton.Disabled = ( CurMessage == 0 );
		PrevButton.Visible = !( CurMessage == 0 );
		
		NextButton.Disabled = ( CurMessage == AllMessages.Count-1 );
		NextButton.Visible = !( CurMessage == AllMessages.Count-1 );
		
		ScrollLabel.Text = $"{CurMessage+1} of {AllMessages.Count}";
		
		if( !Locations.IsCheckingForSent() && ArchipelagoHandler.GetSession().Items.Any() )
		{
			bool CharacterWasUnlocked = false;
			while( ArchipelagoHandler.GetSession().Items.Any() ) {
				ItemInfo NewItem = ArchipelagoHandler.GetSession().Items.DequeueItem();
				
				if( Characters.UnlockCharacter(NewItem) && !CharacterWasUnlocked )
				{
					CharacterWasUnlocked = true;
				}
				string ItemName = NewItem.ItemDisplayName;
				string ItemFrom = NewItem.Player.Alias;
				Write($"Recieved {ItemName} from {ItemFrom}!");
			}
			
			if( !SoundEffects.IsPlaying() )
			{
				SoundEffects.Stream = CharacterWasUnlocked ? CharacterUnlockSound : TrophyUnlockSound;
				SoundEffects.Play();
			}
		}
		
		if( !ArchipelagoHandler.GetSession().Items.Any() && !Locations.IsCheckingForSent() && !VictoryAchieved && Characters.CheckWinCondition() )
		{
			VictoryAchieved = true;
			
			Write("Victory achieved, congratuations!");
			
			ForceAutoScrollOff = true;
			
			SoundEffects.Stop();
			if( !SoundEffects.IsPlaying() )
			{
				SoundEffects.Stream = VictorySound;
				SoundEffects.Play();
			}
		}
	}
	
	public void Write(string line)
	{
		bool autoScroll = false;
		if( CurMessage == AllMessages.Count-1 )
		{
			autoScroll = true;
		}
		
		AllMessages.Add(line);
		
		if( autoScroll && !ForceAutoScrollOff )
		{
			CurMessage++;
		}
	}
	
	public void NextMessage()
	{
		if( CurMessage < AllMessages.Count-1 )
		{
			CurMessage++;
		}
		
		ForceAutoScrollOff = false;
	}
	
	public void PrevMessage()
	{
		if( CurMessage > 0 )
		{
			CurMessage--;
		}
		
		ForceAutoScrollOff = false;
	}
}
