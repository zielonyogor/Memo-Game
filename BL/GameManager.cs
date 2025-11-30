using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using Microsoft.Extensions.Configuration;

namespace NR155910155992.MemoGame.BL
{
	public class GameManager : IGameManager
	{
		public IDataAccessObject _dao;

		public GameManager(IConfiguration configuration)
		{
			var loader = new LibraryLoader(configuration);
			_dao = loader.LoadObjectFromLibrary<IDataAccessObject>(LibraryKey.Dao);
		}

		public IEnumerable<ICard> GetRandomSetOfCards(int numberOfCards)
		{
			var cards = _dao.GetAllCards();

			Random rnd = new Random();
			IEnumerable<ICard> randomCards = cards.OrderBy(c => rnd.Next()).Take(numberOfCards);
			return randomCards;
		}

		public ICard[,] GetRandomCardsPositionsOnBoard(IEnumerable<ICard> cards)
		{
			return new ICard[4, 4];
		}
	}
}