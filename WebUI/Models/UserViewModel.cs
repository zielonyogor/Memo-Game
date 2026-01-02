using NR155910155992.MemoGame.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NR155910155992.MemoGame.WebUI.Models
{
	public class UserViewModel : IUserProfile
	{
		public int Id { get; }

		[Required]
		[MaxLength(50)]
		public string UserName { get; set; } = string.Empty;

		public UserViewModel(IUserProfile userProfile)
		{
			Id = userProfile.Id;
			UserName = userProfile.UserName;
		}
	}
}
