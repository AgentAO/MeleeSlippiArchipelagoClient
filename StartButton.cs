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
		var GameLayer = GetTree().GetCurrentScene().GetNode<CanvasLayer>("HUD/Game");
		var ErrorMessagesLabel = IntroLayer.GetNode<Label>("ErrorMessages");
		var HostField = IntroLayer.GetNode<TextEdit>("Host Field");
		var PortField = IntroLayer.GetNode<TextEdit>("Port Field");
		var PlayerField = IntroLayer.GetNode<TextEdit>("Player Name Field");
		var PasswordField = IntroLayer.GetNode<TextEdit>("Password Field");
		
		try
		{
			ArchipelagoHandler.CreateSession(HostField.Text, Convert.ToInt32(PortField.Text));
		}
		catch(Exception e)
		{
			ErrorMessagesLabel.Text = e.Message;
		}
		
		LoginResult result = ArchipelagoHandler.TryLogin(PlayerField.Text, PasswordField.Text);
		
		if( ArchipelagoHandler.IsSuccessful(result) )
		{
			//IntroLayer.RemoveChild();
			//IntroLayer.QueueFree();
			IntroLayer.Hide();
			GetTree().CallGroup("GameStartHandler", "OnGameStart");
			GameLayer.Show();
		}
		else
		{
			ErrorMessagesLabel.Text = ArchipelagoHandler.GetErrorMessages(result);
		}
	}
}
