using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.MemoGame.JsonDao.Models
{
	internal class UserProfile : IUserProfile
	{
		public int Id { get; set; }
		public string UserName { get; set; }

		public ICollection<PlayerGameResult> PlayerResults { get; set; } = new List<PlayerGameResult>();
	}
}
