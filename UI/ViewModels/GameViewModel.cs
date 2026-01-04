using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using NR155910155992.MemoGame.UI.Models;
using NR155910155992.MemoGame.UI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class GameViewModel : ViewModelBase
	{
		private readonly IGameManager _gameManager;
		private readonly INavigationService _menuNavigationService; 
		private readonly IParameterNavigationService<GameResult> _gameFinishedNavigationService;

		public ObservableCollection<CardViewModel> Cards { get; set; }

        private TimeSpan timeElapsed;
        public string ElapsedTimeString => timeElapsed.ToString(@"mm\:ss");

		public ICommand BackToMenu { get; }
		public Action GoBackToMainMenuAction;

		private readonly GameSettings _gameSettings;

		public int Rows => _gameSettings.Rows;
		public int Columns => _gameSettings.Columns;

		public GameViewModel(
			GameSettings gameSettings,
			IGameManager gameManager, 
			INavigationService menuNavigationService, 
			IParameterNavigationService<GameResult> gameFinishedNavigationService
		)
		{
			_gameSettings = gameSettings;
			_gameManager = gameManager;
			_menuNavigationService = menuNavigationService;
			_gameFinishedNavigationService = gameFinishedNavigationService;

			BackToMenu = new RelayCommand((_) => Back());

			SetupCards();
			_gameManager.StartNewGame(GameMode.Pairs, GameType.Solo);
		}

        private void SetupCards()
		{
            ICard [,] cards = _gameManager.GetRandomCardsPositionedOnBoard(_gameSettings.Rows, _gameSettings.Columns); 
            Cards = new ObservableCollection<CardViewModel>(); 
			for( int r = 0; r < _gameSettings.Rows; r++)
            {
				for (int c = 0; c < _gameSettings.Columns; c++) {
					ICard card = cards[r,c];
                    if ( card != null)
					{
						var cardViewModel = new CardViewModel(card, _gameManager, r, c, OnCardClicked);
						Cards.Add(cardViewModel);

					}
				}
			}
		}

		private void OnCardClicked(int row, int col)
		{
			Debug.WriteLine($"Card clicked at {row}, {col}");
			var boardStateNow = _gameManager.OnCardClicked(row, col);

			// update looks
			for (int r = 0; r < _gameSettings.Rows; r++)
			{
				for (int c = 0; c < _gameSettings.Columns; c++)
				{
					var fieldState = boardStateNow.Fields[r, c];
					var cardViewModel = Cards[r * _gameSettings.Columns + c];
					if (fieldState.State == ClickResult.Match)
					{
						cardViewModel.IsMatched = true;
					}
					else if (fieldState.State == ClickResult.FirstCard)
					{
						cardViewModel.IsRevealed = true;
					}
					else if (fieldState.State == ClickResult.Hidden)
					{
						_ = cardViewModel.Hide(); // later disable whole input on every card while this is happening;
					}
				}
			}


			// Check for game finished
			if (boardStateNow.IsFinished)
			{
				_gameFinishedNavigationService.Navigate(
					new GameResult(
						_gameSettings.Rows * _gameSettings.Columns / 2,
						timeElapsed));
			}

		}

		private void Back()
		{
			_menuNavigationService.Navigate();
		}
	}
}