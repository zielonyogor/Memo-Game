namespace NR155910155992.MemoGame.Interfaces
{
	public interface IGameManager
	{
		public IEnumerable<ICard> GetRandomSetOfCards(int numberOfCards);
		public ICard[,] GetRandomCardsPositionsOnBoard(IEnumerable<ICard> cards); //different for different game modes

	}
}
