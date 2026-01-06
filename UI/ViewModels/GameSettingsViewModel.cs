using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using NR155910155992.MemoGame.UI.Models;
using NR155910155992.MemoGame.UI.Services;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    public class GameSettingsViewModel : ViewModelBase
    {
        private readonly IGameManager _gameManager;
		private readonly int totalCardCount;
		private readonly int amountMatchingCards = 2; // this should be differently handled if there would be more game modes (triplets etc.)

		public string LibraryStatus =>
			$"Library size: {totalCardCount} images.\n" +
			$"Current Grid: {_rows * _columns} cells ({_rows * _columns / 2} pairs).";

		private int _rows = 2;
        public int Rows 
        { 
            get => _rows; 
            set
            {
                if (value * _columns / amountMatchingCards > totalCardCount)
                    return;

                _rows = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(LibraryStatus));
			} 
        }

        private int _columns = 1;
        public int Columns
        {
            get => _columns;
            set
            {
				if (value * _rows / amountMatchingCards > totalCardCount)
					return;

                _columns = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(LibraryStatus));
			}
        }

        public ICommand PlayGame { get; }
        public GameSettingsViewModel(IGameManager gameManager,
	        IParameterNavigationService<GameSettings> navigateToGame,
            INavigationService closeModal)
		{
            _gameManager = gameManager;
            totalCardCount = _gameManager.GetCardsCount();
            
            PlayGame = new RelayCommand((_) => {
                navigateToGame.Navigate(new GameSettings(_rows, _columns));
                closeModal.Navigate();
            });
        }
    }
}
