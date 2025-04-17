using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

public partial class Locations
{
	public static void SendWinLocation(string name, int wins)
	{
		Characters.RegisterWins(name, wins);
		SendLocation($"{name} {wins} Wins");
	}
	
	public static void SendLocation(string name)
	{
		long LocationId = ArchipelagoHandler.GetSession().Locations.GetLocationIdFromName(ArchipelagoHandler.GameName, name);
		
		ArchipelagoHandler.GetSession().Locations.CompleteLocationChecks(LocationId);
	}
	
	public static string GetLocationName(long id)
	{
		return ArchipelagoHandler.GetSession().Locations.GetLocationNameFromId(id, ArchipelagoHandler.GameName);
	}
	
	public static void RegisterCharacterWinCounts(long id)
	{
		string name = GetLocationName(id);
		
		Regex CharWinRegex = new Regex("(.*) (\\d+) Wins");
		Match regexMatch = CharWinRegex.Match(name);
		if( regexMatch.Success )
		{
			string CharacterName = regexMatch.Groups[1].ToString();
			int Wins = Int32.Parse(regexMatch.Groups[2].ToString());
			
			Characters.RegisterWins(CharacterName, Wins);
		}
	}
	
	public static ReadOnlyCollection<long> GetAllLocationsChecked()
	{
		return ArchipelagoHandler.GetSession().Locations.AllLocationsChecked;
	}
}
