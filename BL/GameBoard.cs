using NR155910155992.MemoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR155910155992.MemoGame.BL
{
	internal class GameBoard
	{
		private readonly IDataAccessObject _dao;
		public int TotalPairs { get; private set; } = 0;

		public GameBoard(IDataAccessObject dao)
		{
			_dao = dao;
		}

		public IEnumerable<ICard> GetRandomSetOfCards(int numberOfCards)
		{
			var cards = _dao.GetAllCards();

			Random rnd = new Random();
			var randomCards = cards.OrderBy(c => rnd.Next())
								   .Take(numberOfCards);
			return randomCards;
		}

		public int GetTotalCardsCount()
		{
			return _dao.GetAllCards().Count();
		}

		public ICard[,] GenerateBoard(int rows, int cols)
		{
			int uniqueCardsNeeded = (rows * cols) / 2; //making sure all pairs can fit, if odd one cell of grid will be empty
			TotalPairs = uniqueCardsNeeded;

			var cardSet = GetRandomSetOfCards(uniqueCardsNeeded).ToList();

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
