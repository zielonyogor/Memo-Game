using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
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
		public string ImageSource => ImageUtility.ResolvePath(_card.ImagePath);
		public string BackSideImage => ImageUtility.ResolvePath("Assets/Cards/card_backside.png");
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

		private async void OnClick()
		{
            if (IsRevealed || IsMatched || _gameManager.IsShowingChoosenCards)
				return;
            IsRevealed = true;
            var result = await _gameManager.OnCardClicked(Id);

			if (result == Core.ClickResult.Mismatch)
			{
				await Task.Delay(1000);
				_gameManager.ResolveMismatch();
			}
		}
	}
}
