using Godot;
using System;
using Archipelago.MultiClient.Net;

public partial class StartButton : Godot.Button
{	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnPressed()
	{
		var IntroLayer = GetTree().GetCurrentScene().GetNode<CanvasLayer>("HUD/Intro");
		var ErrorMessagesLabel = IntroLayer.GetNode<Label>("ErrorMessages");
		var HostField = IntroLayer.GetNode<TextEdit>("Host Field");
		var PortField = IntroLayer.GetNode<TextEdit>("Port Field");
		var PlayerField = IntroLayer.GetNode<TextEdit>("Player Name Field");
		var PasswordField = IntroLayer.GetNode<TextEdit>("Password Field");
		
		this.Text = "Connecting";
		
		try
		{
			ArchipelagoHandler.CreateSession(
				!string.IsNullOrEmpty(HostField.Text) ? HostField.Text : "archipelago.gg", 
				!string.IsNullOrEmpty(PortField.Text) ? Convert.ToInt32(PortField.Text) : 38281
			);
		}
		catch(Exception e)
		{
			this.Text = "Connect";
			ErrorMessagesLabel.Text = e.Message;
			return;
		}
		
		LoginResult result = ArchipelagoHandler.TryLogin(
			!string.IsNullOrEmpty(PlayerField.Text) ? PlayerField.Text : "Player1", 
			PasswordField.Text
		);
		
		if( ArchipelagoHandler.IsSuccessful(result) )
		{
			var GameLayer = GetTree().GetCurrentScene().GetNode<CanvasLayer>("HUD/Game");
			//IntroLayer.RemoveChild();
			//IntroLayer.QueueFree();
			IntroLayer.Hide();
			GetTree().CallGroup("GameStartHandler", "OnGameStart");
			GameLayer.Show();
		}
		else
		{
			this.Text = "Connect";
			ErrorMessagesLabel.Text = ArchipelagoHandler.GetErrorMessages(result);
		}
	}
}
