using Godot;
using System;

public partial class FormInput : TextEdit
{
	private Button SubmitButton {get;set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var IntroLayer = GetTree().GetCurrentScene().GetNode<CanvasLayer>("HUD/Intro");
		SubmitButton = IntroLayer.GetNode<Button>("StartButton");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnInput(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
		{
			if( eventKey.Pressed && Input.IsKeyPressed(Key.Shift) && eventKey.Keycode == Key.Tab && HasFocus() )
			{
				if( FocusPrevious != "" )
				{
					GetNode(FocusPrevious).CallDeferred(TextEdit.MethodName.GrabFocus);
				}
				GetViewport().SetInputAsHandled();
			}
			else if( eventKey.Pressed && eventKey.Keycode == Key.Tab && HasFocus() )
			{
				if( FocusNext != "" )
				{
					GetNode(FocusNext).CallDeferred(TextEdit.MethodName.GrabFocus);
				}
				GetViewport().SetInputAsHandled();
			}
			else if( eventKey.Pressed && eventKey.Keycode == Key.Enter && HasFocus() )
			{
				SubmitButton.EmitSignal("pressed");
				GetViewport().SetInputAsHandled();
			}
		}
		
	}
}
