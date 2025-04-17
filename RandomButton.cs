using Godot;
using System;

public partial class RandomButton : TextureButton
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
		var RandomCharacterLabel = GetParent().GetNode<Label>("RandomCharacterSelection");
		
		var allCharacters = Characters.GetUnlockedCharacters();
		
		if( allCharacters.Count == 0 )
		{
			return;
		}
		
		Random random = new Random();
		int index = random.Next(allCharacters.Count);
		
		RandomCharacterLabel.Text = allCharacters[index];
	}
}
