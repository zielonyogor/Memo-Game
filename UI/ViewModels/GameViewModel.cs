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

        private DispatcherTimer _timer;
        private TimeSpan _timeElapsed;
        public TimeSpan TimeElapsed
        {
            get => _timeElapsed;
            private set
            {
                _timeElapsed = value;
                OnPropertyChanged(nameof(TimeElapsed));
            }
        }
        public string ElapsedTimeString => _timeElapsed.ToString(@"mm\:ss");
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
            TimerInit();
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
		
        private void TimerInit()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += TimerTick;
            _timer.Start();

        }
        private void TimerTick(object? sender, EventArgs e)
        {
            _timeElapsed = _timeElapsed.Add(TimeSpan.FromSeconds(1));
            OnPropertyChanged(nameof(ElapsedTimeString));
        }

        private void OnGameFinished()
		{
            _timer.Stop();
            Debug.WriteLine($"Matchedallcards! in {_timeElapsed} seconds" );
			GameFinishedViewModel = new GameFinishedViewModel(GoBackToMainMenuAction, _timeElapsed);
            OnPropertyChanged(nameof(GameFinishedViewModel));
            FinishedOverlayVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(FinishedOverlayVisibility));
        }

        

    }
}
