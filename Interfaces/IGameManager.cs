namespace NR155910155992.MemoGame.Interfaces
{
	public interface IGameManager
	{
		public bool IsShowingChoosenCards { get;}
		public IEnumerable<ICard> GetRandomSetOfCards(int numberOfCards);
		public ICard[,] GetRandomCardsPositionedOnBoard(int rows, int columns); //different for different game modes

		public Task OnCardClicked(int clickedCardId);
        public event EventHandler<int> CardsMatched;
		public event EventHandler CardsMismatched; 

    }
}
