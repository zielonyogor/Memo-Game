using Microsoft.Extensions.Configuration;
using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NR155910155992.MemoGame.BL
{
	public class GameManager : IGameManager
	{
		public IDataAccessObject _dao;
		private int? _firstRevealedCardId = null;

        public event EventHandler<int> CardsMatched;
        public event EventHandler CardsMismatched;

		private bool _showingCards = false;
		public bool IsShowingChoosenCards => _showingCards;

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

		public async Task OnCardClicked(int clickedCardId)
		{
			if (_showingCards)
				return;
			
			if (_firstRevealedCardId == null)
			{
				_firstRevealedCardId = clickedCardId;
			}
			else
			{
				if (_firstRevealedCardId == clickedCardId)
				{
					CardsMatched?.Invoke(this, clickedCardId);
                    Debug.WriteLine($"Matched cards: {clickedCardId}");
				}
				else
				{
					_showingCards = true;
					await Task.Delay(1000);
					CardsMismatched?.Invoke(this, EventArgs.Empty);
                    Debug.WriteLine($"No match: {_firstRevealedCardId} and {clickedCardId}");
					_showingCards = false;
                }
				_firstRevealedCardId = null;
			}

			
		}
	}
}