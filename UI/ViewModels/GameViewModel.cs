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

		public GameViewModel(IGameManager gameManager, Action goBackToMainMenu)
		{
			BackToMenu = new RelayCommand((_) => goBackToMainMenu());
			_gameManager = gameManager;

			SetupCards();
		}

		private void SetupCards()
		{
			var cards = _gameManager.GetRandomSetOfCards(2);
			Cards = new ObservableCollection<CardViewModel>();
			foreach (var card in cards)
			{
				var cardViewModel = new CardViewModel(card, OnCardClicked);
				Cards.Add(cardViewModel);
			}
		}

		private async void OnCardClicked(CardViewModel clickedCard)
		{
			if (clickedCard.IsRevealed || clickedCard.IsMatched)
				return;
			clickedCard.IsRevealed = true;
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
				}
				else
				{
					await Task.Delay(1000);
					_firstRevealedCard.IsRevealed = false;
					clickedCard.IsRevealed = false;
				}
				_firstRevealedCard = null;
			}
		}
	}
}
