using NR155910155992.MemoGame.Interfaces;
using System.Collections.Generic;

namespace NR155910155992.MemoGame.BL
{
	public class GameManager : IGameManager
	{
		public IDataAccessObject dao;

		public GameManager()
		{
			
		}

		public IEnumerable<ICard> GetRandomSetOfCards(int numberOfCards)
		{
			var cards = dao.GetAllCards();

			//not sure if it works, didnt test
			Random rnd = new Random();
			IEnumerable<ICard> randomCards = cards.OrderBy(c => rnd.Next())
					   .Take(numberOfCards);
			return randomCards;
		}

		public ICard[,] GetRandomCardsPositionsOnBoard(IEnumerable<ICard> cards)
		{
			return new ICard[4, 4];
		}
	}
}