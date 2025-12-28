using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using NR155910155992.MemoGame.UI.Models;
using NR155910155992.MemoGame.UI.Services;
using System.Collections.ObjectModel;
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
			_gameManager.StartNewGame(Core.GameMode.Pairs, Core.GameType.Solo);
			_gameManager.GameFinished += (_, __) =>
			{
				_gameFinishedNavigationService.Navigate(
					new GameResult(
						_gameSettings.Rows * _gameSettings.Columns / 2,
						timeElapsed));
			};

			_gameManager.TimeUpdated += (s, timeElapsed) =>
            {
                this.timeElapsed = timeElapsed;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnPropertyChanged(nameof(ElapsedTimeString)); 
                });
            };
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
						var cardViewModel = new CardViewModel(card, _gameManager);
						Cards.Add(cardViewModel);

					}
				}
			}
		}

		private void Back()
		{
			_menuNavigationService.Navigate();
		}
	}
}