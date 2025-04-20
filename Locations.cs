using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using Godot;

public partial class Locations
{
	private static Queue<string> UnhandledSentLocations {get;set;} = new Queue<string>();
	private static bool DoingLookup = false;

	public static void SendWinLocation(string name, int wins)
	{
		Characters.RegisterWins(name, wins);
		SendLocation($"{name} {wins} Wins");
	}
	
	public static void SendLocation(string name)
	{
		long LocationId = ArchipelagoHandler.GetSession().Locations.GetLocationIdFromName(ArchipelagoHandler.GameName, name);
		
		ArchipelagoHandler.GetSession().Locations.CompleteLocationChecks(LocationId);

		LookupLocationSent(LocationId);
	}

	public static void LookupLocationSent(long LocationId)
	{
		DoingLookup = true;
		long[] locations = new long[]{LocationId};
		ArchipelagoHandler.GetSession().Locations.ScoutLocationsAsync(HintCreationPolicy.None, locations)
			.ContinueWith(UnlockInfo => {
				foreach((long id, ScoutedItemInfo info) in UnlockInfo.Result)
				{
					UnhandledSentLocations.Enqueue($"Sent {info.ItemDisplayName} to {info.Player}");
				}
				DoingLookup = false;
			});		
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

	public static bool HasUnhandledSentLocation()
	{
		return UnhandledSentLocations.Count > 0;
	}

	public static string DequeueSentLocation()
	{
		return UnhandledSentLocations.Dequeue();
	}
	
	public static bool IsCheckingForSent()
	{
		return DoingLookup;
	}
}
