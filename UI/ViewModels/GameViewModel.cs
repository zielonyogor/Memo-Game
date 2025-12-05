using NR155910155992.MemoGame.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class GameViewModel : ViewModelBase
	{
		private readonly IGameManager _gameManager;

		public ObservableCollection<CardViewModel> Cards { get; set; }
        public GameFinishedViewModel GameFinishedViewModel { get; private set; }

        public int Rows { get; private set; }
		public int Columns { get; private set; }

		public ICommand BackToMenu{ get; }
        public Action GoBackToMainMenuAction;

        private Visibility finishedOverlayVisibility;
        public Visibility FinishedOverlayVisibility
        {
            get
            {
                return finishedOverlayVisibility;
            }
            set
            {
                finishedOverlayVisibility = value;

                OnPropertyChanged("Visibility");
            }
        }
        public GameViewModel(IGameManager gameManager, Action goBackToMainMenu)
        {
            GoBackToMainMenuAction = goBackToMainMenu;

            BackToMenu = new RelayCommand((_) => goBackToMainMenu());

            _gameManager = gameManager;


            SetupCards();

			_gameManager.GameFinished += (s, e) =>
			{
				Debug.WriteLine("Game Finished!");
				OnGameFinished();
			};

        }

		private void SetupCards()
		{
            FinishedOverlayVisibility = Visibility.Hidden;
			Rows = 2;
			Columns = 2;
            ICard [,] cards = _gameManager.GetRandomCardsPositionedOnBoard(Rows, Columns); 
            Cards = new ObservableCollection<CardViewModel>(); 
			for( int r = 0; r < Rows; r++)
            {
				for (int c = 0; c < Columns; c++) {
					ICard card = cards[r,c];
                    if ( card != null)
					{
						var cardViewModel = new CardViewModel(card, _gameManager);
						Cards.Add(cardViewModel);

					}
				}
			}
		}
		private void OnGameFinished()
		{
            Debug.WriteLine("Matchedallcards!");
			GameFinishedViewModel = new GameFinishedViewModel(GoBackToMainMenuAction);
            OnPropertyChanged(nameof(GameFinishedViewModel));
            FinishedOverlayVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(FinishedOverlayVisibility));
            // Logic to handle game completion, e.g., notify the user or navigate back to the menu
        }

		//private void OnBackToMenu()
		//{
  //          Debug.WriteLine("Back to menu clicked");
  //          FinishedOverlayVisibility = Visibility.Collapsed;
  //          OnPropertyChanged(nameof(FinishedOverlayVisibility));


  //      }
    }
}
