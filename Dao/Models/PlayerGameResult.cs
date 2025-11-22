using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.MemoGame.Dao.Models
{
	internal class PlayerGameResult : IPlayerGameResult
	{
		public int Id { get; set; }

		public int CardsUncovered { get; set; }
		public bool IsWinner { get; set; }

		public int UserProfileId { get; set; }
		public UserProfile User { get; set; }

		public int GameSessionId { get; set; }
		public GameSession GameSession { get; set; }

		IUserProfile IPlayerGameResult.User => User;
		IGameSession IPlayerGameResult.GameSession => GameSession;
	}
}
