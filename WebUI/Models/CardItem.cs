using NR155910155992.MemoGame.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NR155910155992.MemoGame.WebUI.Models
{
	public class CardItem : ICard
	{
		public int Id { get; set; }

		[MaxLength(100)]
		[Required]
		public string Name { get; set; }
		
		[Required]
		public string ImagePath { get; set; }

		public CardItem() { }

		public CardItem(ICard card)
		{
			Id = card.Id;
			Name = card.Name;
			ImagePath =	card.ImagePath;
		}
	}
}
