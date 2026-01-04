using NR155910155992.MemoGame.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NR155910155992.MemoGame.WebUI.Models
{
	public class CardItemModel : ICard
	{
		public int Id { get; set; }

		[MaxLength(100)]
		[Required]
		[Display(Name = "Card's name")]
		public string Name { get; set; }
		
		[Required]
		[Display(Name = "Path to your image")]
		public string ImagePath { get; set; }

		public CardItemModel() { }

		public CardItemModel(ICard card)
		{
			Id = card.Id;
			Name = card.Name;
			ImagePath =	card.ImagePath;
		}
	}
}
