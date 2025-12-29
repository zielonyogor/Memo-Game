using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.MemoGame.WebUI.Models
{
	public class CardItem : ICard
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ImagePath { get; set; }

		public CardItem(ICard card)
		{
			Id = card.Id;
			Name = card.Name;
			ImagePath =	card.ImagePath;
		}
	}
}
