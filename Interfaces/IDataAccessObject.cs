using NR155910155992.MemoGame.Core;


namespace NR155910155992.MemoGame.Interfaces
{
	public interface IDataAccessObject
	{
		public IEnumerable<IGameSession> GetAllGameSessions();
		public IEnumerable<IGameSession> GetAllGameSessionsForUser(IUserProfile userProfile);


		public IUserProfile CreateNewUserProfile(string userName);
		public IGameSession CreateGameSession(DateTime date, TimeSpan duration, GameType gameType, GameMode gameMode, IEnumerable<IUserProfile> users, int totalCards);
		public IPlayerGameResult CreatePlayerGameResult(IUserProfile userProfile, IGameSession gameSession, int cardsUncovered, bool isWinner);


		public IUserProfile GetFirstUserProfile();
		public IEnumerable<IUserProfile> GetAllUserProfiles();
		public void DeleteUserProfile(int userProfileId);
		public void UpdateUserProfile(IUserProfile userProfile);

		public IEnumerable<ICard> GetAllCards();
		public ICard CreateNewCard(string imagePath, string name);
		public ICard CreateNewCard(Stream fileStream, string fileName, string name);
		public void DeleteCard(int cardId);
		public void UpdateCardName(int cardId, string newName);
	}
}
