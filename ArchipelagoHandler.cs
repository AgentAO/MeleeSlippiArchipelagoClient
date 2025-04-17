using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;

public static class ArchipelagoHandler
{
	private static ArchipelagoSession Session {get;set;}
	public static string GameName {get;} = "Melee Slippi";
	
	public static ArchipelagoSession GetSession()
	{
		return Session;
	}
	
	public static void CreateSession(string Host = "localhost", int Port = 38281)
	{
		Session = ArchipelagoSessionFactory.CreateSession(Host, Port);
	}
	
	public static LoginResult TryLogin(string PlayerName = "Player1", string Password = null)
	{
		if( Session == null )
		{
			new LoginFailure("Session not initialized");
		}
		
		LoginResult result = Session.TryConnectAndLogin(
			GameName,
			PlayerName,
			ItemsHandlingFlags.AllItems,
			null, // version
			null, // tags
			null, // UUID
			Password // Password that was set when the room was created
		);
		
		if( !result.Successful )
		{
			LoginFailure failure = (LoginFailure)result;
			return failure;
		}
		
		return result;
	}
	
	public static bool IsSuccessful(LoginResult result)
	{
		return result.Successful;
	}
	
	public static string GetErrorMessages(LoginResult result)
	{
		LoginFailure failure = (LoginFailure)result;
		string errorMessage = $"Failed to Connect";
		foreach (string error in failure.Errors)
		{
			errorMessage += $"\n    {error}";
		}
		foreach (ConnectionRefusedError error in failure.ErrorCodes)
		{
			errorMessage += $"\n    {error}";
		}
		
		return errorMessage;
	}
	
	public static bool IsReady()
	{
		try
		{
			if( Session == null )
			{
				return false;
			}
			
			bool hasGame = (Session.ConnectionInfo.Game != "");
			
			return hasGame;
		}
		catch(Exception e)
		{
			Console.WriteLine(e);
			return false;
		}
	}
}
