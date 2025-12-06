using NR155910155992.MemoGame.Interfaces;
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

        public ObservableCollection<CardViewModel> Cards { get; set; }
        public GameFinishedViewModel GameFinishedViewModel { get; private set; }

        public int Rows { get; private set; } //propably move to some struct and maybe make setting where you can edit it
		public int Columns { get; private set; }
        private TimeSpan timeElapsed;
        public string ElapsedTimeString => timeElapsed.ToString(@"mm\:ss");
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

                OnPropertyChanged(nameof(FinishedOverlayVisibility));
            }
        }
        public GameViewModel(IGameManager gameManager, Action goBackToMainMenu)
        {
            GoBackToMainMenuAction = goBackToMainMenu;

            BackToMenu = new RelayCommand((_) => goBackToMainMenu());

            _gameManager = gameManager;


            SetupCards();
            _gameManager.StartNewGame();
            _gameManager.GameFinished += (s, e) =>
			{
				Debug.WriteLine("Game Finished!");
				OnGameFinished();
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
            int _matchedPairs = Cards.Count / 2;
            GameFinishedViewModel = new GameFinishedViewModel(GoBackToMainMenuAction, timeElapsed, _matchedPairs);
            OnPropertyChanged(nameof(GameFinishedViewModel));
            FinishedOverlayVisibility = Visibility.Visible;
        }

        

    }
}
