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

		private Action<int, int> _onCardClicked;
		private readonly Func<bool> _canClick;

		// TODO: should it be public for game view to identify position later?
		public readonly int row;
		public readonly int column;

		public CardViewModel(ICard card, int row, int column, 
			Action<int, int> onCardClicked,
			Func<bool> canClick)
		{
			_card = card;
			_canClick = canClick;

			this.row = row;
			this.column = column;

			_onCardClicked = onCardClicked;

			_isRevealed = false;
			_isMatched = false;

			ClickCommand = new RelayCommand((_) => OnClick());
        }

		private void OnClick()
		{
			if (!_canClick())
				return;

			if (IsRevealed || IsMatched)
				return;

            IsRevealed = true;
			_onCardClicked.Invoke(row, column);
		}
	}
}
