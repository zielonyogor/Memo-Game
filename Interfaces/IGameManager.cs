using NR155910155992.MemoGame.Core;

namespace NR155910155992.MemoGame.Interfaces
{
	public interface IGameManager
	{
        public int GetCardsCount();
        public ICard GetCardById(int cardId);
		public ICard[,] GetRandomCardsPositionedOnBoard(int rows, int columns); //different for different game modes
        public void ResetGame();
		public void StartNewGame(GameMode gameMode, GameType gameType);
        public IEnumerable<IGameSession> GetAllGameSessionsForCurrentUser(); //for game history screen

        public BoardState OnCardClicked(int row, int col);

		public IUserProfile? GetCurrentUserProfile();
        public IEnumerable<IUserProfile> GetAllUserProfiles();
		public void SetCurrentUserProfile(IUserProfile userProfile);
        public IUserProfile CreateNewUserProfile(string userName);
        public void DeleteUserProfile(int userProfileId);
        public void UpdateUserProfile(IUserProfile userProfile, string newUsername);

        public IEnumerable<ICard> GetAllCards();
        public ICard CreateNewCard(string imagePath, string name);
        public void DeleteCard(int cardId);
        public void UpdateCardName(int cardId, string newName);

        public event EventHandler<TimeSpan> TimeUpdated;
        public TimeSpan GetTimeElapsed();
        public GameResult GetCurrentGameResult();
	}
}
