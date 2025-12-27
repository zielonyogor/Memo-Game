using NR155910155992.MemoGame.Interfaces;
using System.Text.Json.Serialization;

namespace NR155910155992.MemoGame.JsonDao.Models
{
    internal class PlayerGameResult : IPlayerGameResult
	{
		public int Id { get; set; }

		public int CardsUncovered { get; set; }
		public bool IsWinner { get; set; }

		public int UserProfileId { get; set; }

		[JsonIgnore]
		public UserProfile User { get; set; }

		public int GameSessionId { get; set; }

		[JsonIgnore]
		public GameSession GameSession { get; set; }

		[JsonIgnore]
		IUserProfile IPlayerGameResult.User {
			get => User;
			set => User = value as UserProfile 
				?? throw new ArgumentNullException(
					nameof(value), "Assigned IUserProfile must be of type UserProfile and not null.");
		}
		[JsonIgnore]
		IGameSession IPlayerGameResult.GameSession {
			get => GameSession;
			set => GameSession = value as GameSession
				?? throw new ArgumentNullException(
					nameof(value), "Assigned IGameSession must be of type GameSession and not null.");
		}
	}
}