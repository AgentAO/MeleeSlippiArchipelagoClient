using System;
using System.Collections.Generic;
using Godot;
using Archipelago.MultiClient.Net.Models;

public static class Characters
{
	public static Dictionary<string, int> CharacterHighestWinCount = new Dictionary<string, int> {};
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
	
	public static void UnlockCharacters(ItemInfo[] Characters)
	{
		foreach(ItemInfo item in Characters)
		{
			UnlockCharacter(item);
		}
	}
	
	public static void UnlockCharacter(ItemInfo Character)
	{
		if( CharacterNames.IndexOf(Character.ItemDisplayName) > -1 )
		{
			UnlockedCharacters.Add(Character.ItemDisplayName);
		}
	}
	
	public static void RegisterWins(string CharacterName, int Wins)
	{
		if( !CharacterHighestWinCount.ContainsKey(CharacterName) )
		{
			CharacterHighestWinCount[CharacterName] = Wins;
		}
		else if( CharacterHighestWinCount[CharacterName] < Wins )
		{
			CharacterHighestWinCount[CharacterName] = Wins;
		}
	}
	
	public static int GetCharacterHighestWins(string CharacterName)
	{
		if( !CharacterHighestWinCount.ContainsKey(CharacterName) )
		{
			return 0;
		}
		else
		{
			return CharacterHighestWinCount[CharacterName];
		}
	}
	
	public static bool CheckWinCondition()
	{
		if( CharacterHighestWinCount.Count == CharacterNames.Count )
		{
			foreach(var (Character, Wins) in CharacterHighestWinCount)
			{
				if( Wins < 1 )
				{
					return false;
				}
				return true;
			}
		}
		return false;
	}
}
