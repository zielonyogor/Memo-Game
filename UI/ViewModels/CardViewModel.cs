using NR155910155992.MemoGame.Interfaces;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    public class CardViewModel : ViewModelBase
    {
		private readonly ICard _card;
		private readonly Action<CardViewModel> _onClickAction;

		private bool _isRevealed;
		public bool IsRevealed
		{
			get => _isRevealed;
			set { _isRevealed = value; OnPropertyChanged(); }
		}

		private bool _isMatched;
		public bool IsMatched
		{
			get => _isMatched;
			set { _isMatched = value; OnPropertyChanged(); }
		}

		public int Id => _card.Id;
		public string ImageSource => ResolvePath(_card.ImagePath);

		public string? CurrentSideImage => IsRevealed || IsMatched ? ImageSource : ImageSource;

		public ICommand ClickCommand { get; }

		public CardViewModel(ICard card, Action<CardViewModel> onClickAction)
		{
			_card = card;
			Debug.WriteLine($"Got card: {card.ImagePath} {card.Id}");
			_isRevealed = false;
			_isMatched = false;

			_onClickAction = onClickAction;
			ClickCommand = new RelayCommand((_) => OnClick());
		}

		private void OnClick()
		{
			_onClickAction?.Invoke(this);
		}

		private string ResolvePath(string path)
		{
			if (System.IO.Path.IsPathRooted(path))
				return path;

			return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
		}
	}
}
