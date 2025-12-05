using NR155910155992.MemoGame.Interfaces;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    public class CardViewModel : ViewModelBase
    {
		private readonly ICard _card;
		private readonly IGameManager _gameManager;

		private bool _isRevealed;
		public bool IsRevealed
		{
			get => _isRevealed;
			set { _isRevealed = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentSideImage)); }
		}

		private bool _isMatched;
		public bool IsMatched
		{
			get => _isMatched;
			set { _isMatched = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentSideImage)); }
		}

		public int Id => _card.Id;
		public string ImageSource => ResolvePath(_card.ImagePath);
		public string BackSideImage => ResolvePath("Assets/Cards/card_backside.png");
        public string? CurrentSideImage => IsRevealed || IsMatched ? ImageSource : BackSideImage;

		public ICommand ClickCommand { get; }

		public CardViewModel(ICard card, IGameManager gameManager)
		{
			_card = card;
			Debug.WriteLine($"Got card: {card.ImagePath} {card.Id}");
			_isRevealed = false;
			_isMatched = false;

			ClickCommand = new RelayCommand((_) => OnClick());

			_gameManager = gameManager;

			_gameManager.CardsMatched += (s, matchedCardId) =>
			{
				if (matchedCardId == Id)
				{
					IsMatched = true;
				}
			};
			_gameManager.CardsMismatched += (s, e) =>
			{
				if (!IsMatched)
				{
					IsRevealed = false;
				}
			};

        }

		private void OnClick()
		{
            if (IsRevealed || IsMatched || _gameManager.IsShowingChoosenCards)
				return;
            IsRevealed = true;
            _gameManager.OnCardClicked(Id);
		}


		private string ResolvePath(string path)
		{
			if (System.IO.Path.IsPathRooted(path))
				return path;

			return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
		}
	}
}
