using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.MemoGame.WebUI.Models
{
	public class UserViewModel : IUserProfile
	{
		public int Id { get; }
		public string UserName { get; set; } = string.Empty;

		public UserViewModel(IUserProfile userProfile)
		{
			Id = userProfile.Id;
			UserName = userProfile.UserName;
		}
	}
}
