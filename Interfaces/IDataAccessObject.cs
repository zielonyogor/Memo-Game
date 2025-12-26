using NR155910155992.MemoGame.Core;


namespace NR155910155992.MemoGame.Interfaces
{
	public interface IDataAccessObject
	{
		public IEnumerable<ICard> GetAllCards();
		public IEnumerable<IGameSession> GetAllGameSessions();
		public IEnumerable<IGameSession> GetAllGameSessionsForUser(IUserProfile userProfile);
		public IEnumerable<IPlayerGameResult> GetAllPlayerGameResultsForGameSession(IGameSession gameSession);


		public ICard CreateNewCard(string imagePath, string name);
		public IUserProfile CreateNewUserProfile(string userName);
		public IGameSession CreateGameSession(DateTime date, TimeSpan duration, GameType gameType, GameMode gameMode, IEnumerable<IUserProfile> users);
		public IPlayerGameResult CreatePlayerGameResult(IUserProfile userProfile, IGameSession gameSession, int cardsUncovered, bool isWinner);


		public IUserProfile GetFirstUserProfile();
		public IEnumerable<IUserProfile> GetAllUserProfiles();
		public void DeleteUserProfile(IUserProfile userProfile);
		public void UpdateUserProfile(IUserProfile userProfile);
	}
}
