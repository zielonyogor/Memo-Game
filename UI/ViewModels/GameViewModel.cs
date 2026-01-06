using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using NR155910155992.MemoGame.UI.Models;
using NR155910155992.MemoGame.UI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class GameViewModel : ViewModelBase
	{
		private readonly IGameManager _gameManager;
		private readonly INavigationService _menuNavigationService; 
		private readonly INavigationService _gameFinishedNavigationService;

		private readonly DispatcherTimer _uiTimer;
		private string _elapsedTimeString = "00:00";
		public string ElapsedTimeString 
		{
            get => _elapsedTimeString;
            set
            {
                _elapsedTimeString = value;
                OnPropertyChanged(nameof(ElapsedTimeString));
            }
        }

		public ICommand BackToMenu { get; }

		public int Rows { get; }
		public int Columns { get; }

		public ObservableCollection<CardViewModel> Cards { get; set; }
		private bool _isBusy = false;

		public GameViewModel(
			GameSettings gameSettings,
			IGameManager gameManager, 
			INavigationService menuNavigationService, 
			INavigationService gameFinishedNavigationService
		)
		{
			_gameManager = gameManager;
			_menuNavigationService = menuNavigationService;
			_gameFinishedNavigationService = gameFinishedNavigationService;
			
			Rows = gameSettings.Rows;
			Columns = gameSettings.Columns;

			BackToMenu = new RelayCommand((_) => Back());

			_gameManager.ResetGame();
			SetupCards();
			_gameManager.StartNewGame(GameMode.Pairs, GameType.Solo);

			_uiTimer = new DispatcherTimer();
			_uiTimer.Interval = TimeSpan.FromSeconds(1);
			_uiTimer.Tick += (s, e) => UpdateTime();
			_uiTimer.Start();
		}

        private void SetupCards()
		{
            ICard [,] cards = _gameManager.GetRandomCardsPositionedOnBoard(Rows, Columns); 
            Cards = new ObservableCollection<CardViewModel>(); 
			for( int r = 0; r < Rows; r++)
            {
				for (int c = 0; c < Columns; c++) {
					ICard card = cards[r,c];
                    if ( card != null)
					{
						var cardViewModel = new CardViewModel(card, r, c, OnCardClicked, () => !_isBusy);
						Cards.Add(cardViewModel);

					}
				}
			}
		}

		private async void OnCardClicked(int row, int col)
		{
			if (_isBusy)
				return;

			Debug.WriteLine($"Card clicked at {row}, {col}");
			var boardStateNow = _gameManager.OnCardClicked(row, col);
			var cardsToHide = new List<CardViewModel>(); // mismatched cards to hide later

			// update looks
			for (int r = 0; r < Rows; r++)
			{
				for (int c = 0; c < Columns; c++)
				{
					var fieldState = boardStateNow.Fields[r, c];
					var cardVM = Cards[r * Columns + c];

					if (fieldState.State == ClickResult.Match)
					{
						cardVM.IsMatched = true;
					}
					else if (fieldState.State == ClickResult.FirstCard)
					{
						cardVM.IsRevealed = true;
					}
					else if (fieldState.State == ClickResult.Hidden && cardVM.IsRevealed) // mismatched card to hide
					{
						cardsToHide.Add(cardVM);
					}
				}
			}

			if (cardsToHide.Count != 0)
			{
				_isBusy = true;
				await Task.Delay(1000); // 1 sec

				foreach (var card in cardsToHide)
				{
					card.IsRevealed = false;
				}
				_isBusy = false;
			}

			// Check for game finished
			if (boardStateNow.IsFinished)
			{
				_uiTimer.Stop();
				_gameFinishedNavigationService.Navigate();
			}

		}
		private void UpdateTime()
		{
			var duration = _gameManager.GetTimeElapsed();
			ElapsedTimeString = duration.ToString(@"mm\:ss");
		}


		private void Back()
		{
			_uiTimer.Stop()
			_menuNavigationService.Navigate();
		}
	}
}