using System.ComponentModel.DataAnnotations;

namespace NR155910155992.MemoGame.WebUI.Models
{
	public class CreateCardViewModel
	{
		[Required]
		[MaxLength(100)]
		[Display(Name = "Card Name")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Image File")]
		public IFormFile ImageFile { get; set; }
	}
}
