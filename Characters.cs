using System;
using System.Collections.Generic;
using Godot;
using Archipelago.MultiClient.Net.Models;
using System.Linq;
using Archipelago.MultiClient.Net.Packets;

public static class Characters
{
	public static Dictionary<string, int> CharacterHighestWinCount = new Dictionary<string, int> {};
	public static Dictionary<string, List<int>> CharacterWinChecksUnlocked = new() { };
	public static List<string> UnlockedCharacters = new List<string> {};
	public static List<string> CharacterNames = new List<string> {
		"Mario",
		"Donkey Kong",
		"Link",
		"Samus",
		"Yoshi",
		"Kirby",
		"Fox",
		"Pikachu",
		"Ness",
		"Captain Falcon",
		"Bowser",
		"Peach",
		"Ice Climbers",
		"Zelda",
		"Sheik",
		"Luigi",
		"Jigglypuff",
		"Mewtwo",
		"Marth",
		"Mr. Game & Watch",
		"Dr. Mario",
		"Ganondorf",
		"Falco",
		"Young Link",
		"Pichu",
		"Roy",
	};
	public static Dictionary<string, string> CharacterMapping = new Dictionary<string, string> {
		{"Mario", "MARIO"},
		{"Donkey Kong", "DK"},
		{"Link", "LINK"},
		{"Samus", "SAMUS"},
		{"Yoshi", "YOSHI"},
		{"Kirby", "KIRBY"},
		{"Fox", "FOX"},
		{"Pikachu", "PIKACHU"},
		{"Ness", "NESS"},
		{"Captain Falcon", "CF"},
		{"Bowser", "BOWSER"},
		{"Peach", "PEACH"},
		{"Ice Climbers", "IC"},
		{"Zelda", "ZELDA"},
		{"Sheik", "SHEIK"},
		{"Luigi", "LUIGI"},
		{"Jigglypuff", "JIGGLYPUFF"},
		{"Mewtwo", "MEWTWO"},
		{"Marth", "MARTH"},
		{"Mr. Game & Watch", "GW"},
		{"Dr. Mario", "DRMARIO"},
		{"Ganondorf", "GANONDORF"},
		{"Falco", "FALCO"},
		{"Young Link", "YLINK"},
		{"Pichu", "PICHU"},
		{"Roy", "ROY"},
	};
	
	public static List<string> GetCharacterNames()
	{
		return CharacterNames;
	}
	
	public static List<string> GetUnlockedCharacters()
	{
		return UnlockedCharacters;
	}
	
	public static bool UnlockCharacters(ItemInfo[] Characters)
	{
		bool CharacterWasUnlocked = false;
		foreach(ItemInfo item in Characters)
		{
			if( UnlockCharacter(item) && !CharacterWasUnlocked)
			{
				CharacterWasUnlocked = true;
			}
		}
		
		return CharacterWasUnlocked;
	}
	
	public static bool UnlockCharacter(ItemInfo Character)
	{
		if( CharacterNames.IndexOf(Character.ItemDisplayName) > -1 )
		{
			UnlockedCharacters.Add(Character.ItemDisplayName);
			return true;
		}
		return false;
	}
	
	public static void RegisterWins(string CharacterName, int Wins)
	{
		if( !CharacterWinChecksUnlocked.ContainsKey(CharacterName) )
		{
			CharacterWinChecksUnlocked[CharacterName] = new List<int> {Wins};
		}
		else
		{
			CharacterWinChecksUnlocked[CharacterName].Add(Wins);
		}
	}
	
	public static int GetCharacterHighestWins(string CharacterName)
	{
		// Check our session data loaded win counts
		if( ArchipelagoHandler.GetDataStorageLocal($"{CharacterName} Wins") > 0 )
		{
			return ArchipelagoHandler.GetDataStorageLocal($"{CharacterName} Wins");
		}

		return 0;

		// Old code
		// if( !CharacterWinChecksUnlocked.ContainsKey(CharacterName) )
		// {
		// 	return 0;
		// }
		// else
		// {
		// 	int maxWins = CharacterWinChecksUnlocked[CharacterName].Max(t => t);
		// 	for(int check = 1; check <= maxWins; check++)
		// 	{
		// 		if( !CharacterWinChecksUnlocked[CharacterName].Contains(check) )
		// 		{
		// 			// If we haven't gotten a lower check yet - we mark up to the last highest check acquired.
		// 			// If we don't have check 1, this should be 0.
		// 			return check-1;
		// 		}
		// 	}

		// 	return maxWins;
		// }
	}
	
	public static bool CheckWinCondition()
	{
		if( CharacterWinChecksUnlocked.Count >= (long)ArchipelagoHandler.GetSlotData()["total_character_wins_needed"] )
		{
			int EligibleCharacters = 0;
			foreach(var (Character, Wins) in CharacterWinChecksUnlocked)
			{
				if( GetCharacterHighestWins(Character) >= (long)ArchipelagoHandler.GetSlotData()["required_wins_per_character"] )
				{
					EligibleCharacters++;
				}
			}
			return EligibleCharacters >= (long)ArchipelagoHandler.GetSlotData()["total_character_wins_needed"];
		}
		return false;
	}
}
