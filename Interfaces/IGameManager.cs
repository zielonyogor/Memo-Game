using NR155910155992.MemoGame.Core;

namespace NR155910155992.MemoGame.Interfaces
{
	public interface IGameManager
	{
		public bool IsShowingChoosenCards { get;}
        public IEnumerable<ICard> GetRandomSetOfCards(int numberOfCards);
        public int GetCardsCount();
		public ICard[,] GetRandomCardsPositionedOnBoard(int rows, int columns); //different for different game modes
        public void StartNewGame(GameMode gameMode, GameType gameType);
        public IEnumerable<IGameSession> GetAllGameSessionsForCurrentUser(); //for game history screen

        public Task<ClickResult> OnCardClicked(int clickedCardId);
        public void ResolveMismatch();

        public event EventHandler<int> CardsMatched;
		public event EventHandler CardsMismatched;
        public event EventHandler GameFinished;
        public event EventHandler<TimeSpan> TimeUpdated;

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
	}
}
