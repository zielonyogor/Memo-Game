using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.MemoGame.WebUI.Models
{
	public class CardViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ImagePath { get; set; }

		public bool IsRevealed { get; set; }
		public bool IsMatched { get; set; }

		public CardViewModel(ICard card)
		{
			Id = card.Id;
			Name = card.Name;
			ImagePath = card.ImagePath;

			IsRevealed = false;
			IsMatched = false;
		}
	}
}
