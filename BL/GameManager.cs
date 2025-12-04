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

		//retrurns a board fileed with shuffled cards
		public ICard[,] GetRandomCardsPositionedOnBoard(int rows, int cols)
		{
			int uniqueCardsNeeded = (rows * cols) / 2; //making sure all pairs can fit, if odd one cell of grid will be empty
            var cardSet = GetRandomSetOfCards(uniqueCardsNeeded);
            
			var duplicatedCards = cardSet.Concat(cardSet).ToList();
            
			Random rnd = new Random();
			var shuffledCards = duplicatedCards.OrderBy(c => rnd.Next()).ToList();
			
			var board = new ICard[rows, cols];
			int index = 0;
			for (int r = 0; r < rows; r++)
			{
				for (int c = 0; c < cols; c++)
				{
					if (index < shuffledCards.Count)
					{
						board[r, c] = shuffledCards[index];
						index++;
					}
					else
					{
						board[r, c] = null; // In case of odd number of cells, leave last cell empty
					}
				}
			}
			return board;
		}
	}
}