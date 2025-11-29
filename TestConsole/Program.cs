using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;

internal class Program
{
	private static void Main(string[] args)
	{
		var daoObject = LibraryLoader.LoadObjectFromLibrary<IDataAccessObject>(LibraryKey.Dao);

		var cards = daoObject.GetAllCards();
		Console.WriteLine("Result " + cards.Count());
		foreach (var item in cards)
		{
			Console.WriteLine($"test: {item.Id}, {item.Name}, {item.ImagePath}");
		}

		var gameSessions = daoObject.GetAllGameSessions();
		foreach (var session in gameSessions)
		{
			Console.WriteLine($"Game Session: {session.Id}, {session.GameDate}, {session.Duration}");
		}

		var userProfiles = daoObject.GetAllUserProfiles();
		foreach (var profile in userProfiles)
		{
			Console.WriteLine($"User Profile: {profile.Id}, {profile.UserName}");
		}

		var blObject = LibraryLoader.LoadObjectFromLibrary<IGameManager>(LibraryKey.Bl);
		Console.WriteLine(blObject.GetType());
	}
}