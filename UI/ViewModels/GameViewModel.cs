using NR155910155992.MemoGame.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class GameViewModel : ViewModelBase
	{
		private readonly IGameManager _gameManager;
		public ICommand BackToMenu { get; }

		public ObservableCollection<CardViewModel> Cards { get; set; }
		private CardViewModel? _firstRevealedCard;

		private bool _isShowingCards;
		public int Rows { get; private set; }
		public int Columns { get; private set; }

        public GameViewModel(IGameManager gameManager, Action goBackToMainMenu)
		{
			BackToMenu = new RelayCommand((_) => goBackToMainMenu());
			_gameManager = gameManager;

			SetupCards();
		}

		private void SetupCards()
		{
			Rows = 4;
			Columns = 4;
            ICard [,] cards = _gameManager.GetRandomCardsPositionedOnBoard(Rows, Columns); 
            Cards = new ObservableCollection<CardViewModel>(); 
			for( int r = 0; r < Rows; r++)
            {
				for (int c = 0; c < Columns; c++) {
					ICard card = cards[r,c];
                    if ( card != null)
					{
						var cardViewModel = new CardViewModel(card, OnCardClicked);
						Cards.Add(cardViewModel);

					}
				}
			}
		}

		private async void OnCardClicked(CardViewModel clickedCard)
		{
			if (clickedCard.IsRevealed || clickedCard.IsMatched || _isShowingCards)
				return;
			clickedCard.IsRevealed = true;
			Debug.WriteLine($"Card clicked: {clickedCard.Id}");
            if (_firstRevealedCard == null)
			{
				_firstRevealedCard = clickedCard;
			}
			else
			{
				if (_firstRevealedCard.Id == clickedCard.Id)
				{
					_firstRevealedCard.IsMatched = true;
					clickedCard.IsMatched = true;
					Debug.WriteLine($"Matched cards: {clickedCard.Id}");
				}
				else
				{
					_isShowingCards = true;
					await Task.Delay(1000);
					_firstRevealedCard.IsRevealed = false;
					clickedCard.IsRevealed = false;
					_isShowingCards = false;
				}
				_firstRevealedCard = null;
			}
		}
	}
}
