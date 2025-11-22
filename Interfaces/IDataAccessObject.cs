using NR155910155992.MemoGame.Core;


namespace NR155910155992.MemoGame.Interfaces
{
	public interface IDataAccessObject
	{
		public IEnumerable<ICard> GetAllCards();
		public IEnumerable<IGameSession> GetAllGameSessions();
		public IEnumerable<IGameSession> GetAllGameSessionsForUser(IUserProfile userProfile);
		public IEnumerable<IUserProfile> GetAllUserProfiles();
		public IEnumerable<IPlayerGameResult> GetAllPlayerGameResultsForGameSession(IGameSession gameSession);

		public ICard CreateNewCard(string imagePath, string name);
		public IUserProfile CreateNewUserProfile(string userName);
		public IGameSession CreateGameSession(DateTime date, TimeSpan duration, GameType gameType, GameMode gameMode);

	}
}
