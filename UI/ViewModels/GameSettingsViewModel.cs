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

        public bool IsInputValid
        {
            get
            {
                var rows = _rows ?? 0;
                var cols = _columns ?? 0;
                if (rows == 0 || cols == 0) return false;
                var cells = rows * cols;
                if (cells < amountMatchingCards) return false; // needs at least one pair
				if (cells / amountMatchingCards > totalCardCount) return false; // not enough cards in library
                return true;
            }
        }

		public string LibraryStatus =>
			$"Library size: {totalCardCount} images.\n" +
			$"Current Grid: {(_rows ?? 0) * (_columns ?? 0)} cells ({((_rows ?? 0) * (_columns ?? 0)) / amountMatchingCards} pairs).";

		private int? _rows = 2;
        public int? Rows 
        { 
            get => _rows; 
            set
            {
                var proposed = value ?? 0;
                var cols = _columns ?? 0;

                _rows = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(LibraryStatus));
                OnPropertyChanged(nameof(IsInputValid));
			} 
        }

        private int? _columns = 1;
        public int? Columns
        {
            get => _columns;
            set
            {
				var proposed = value ?? 0;
                var rows = _rows ?? 0;

                _columns = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(LibraryStatus));
                OnPropertyChanged(nameof(IsInputValid));
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
                var rows = _rows ?? 2;
                var cols = _columns ?? 1;
                navigateToGame.Navigate(new GameSettings(rows, cols));
                closeModal.Navigate();
            });
        }
    }
}
