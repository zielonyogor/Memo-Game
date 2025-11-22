namespace NR155910155992.MemoGame.Interfaces
{
	public interface IPlayerGameResult
	{
		int Id { get; }
		int CardsUncovered { get; }
		bool IsWinner { get; }

		IUserProfile User { get; }
		IGameSession GameSession { get; }
	}
}
