using NR155910155992.MemoGame.Core;

namespace NR155910155992.MemoGame.Interfaces
{
	public interface IGameManager
	{
		public bool IsShowingChoosenCards { get;}
        public TimeSpan TimeElapsed { get; }
        public IEnumerable<ICard> GetRandomSetOfCards(int numberOfCards);
		public ICard[,] GetRandomCardsPositionedOnBoard(int rows, int columns); //different for different game modes
        public void StartNewGame(GameMode gameMode, GameType gameType);
        public void FinishGame();
        public IEnumerable<IGameSession> GetAllGameSessions(); //for game history screen

        public Task OnCardClicked(int clickedCardId);
        public event EventHandler<int> CardsMatched;
		public event EventHandler CardsMismatched;
        public event EventHandler GameFinished;
        public event EventHandler<TimeSpan> TimeUpdated;

		public IUserProfile? GetCurrentUserProfile();
        public IEnumerable<IUserProfile> GetAllUserProfiles();

		public void SetCurrentUserProfile(IUserProfile userProfile);
	}
}
