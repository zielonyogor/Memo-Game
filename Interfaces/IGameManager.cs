namespace NR155910155992.MemoGame.Interfaces
{
	public interface IGameManager
	{
		public IEnumerable<ICard> GetRandomSetOfCards(int numberOfCards);
		public ICard[,] GetRandomCardsPositionedOnBoard(int rows, int columns); //different for different game modes

	}
}
