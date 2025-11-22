internal class Program
{
	private static void Main(string[] args)
	{
		var daoObject = new NR155910155992.MemoGame.Dao.SqliteDAO(); // this is not proper way for our assignment, this is just for testing purpose
		var cards = daoObject.GetAllCards();
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
	}
}