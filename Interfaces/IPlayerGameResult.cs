namespace NR155910155992.MemoGame.Interfaces
{
	public interface IPlayerGameResult
	{
		int Id { get; }
		public int CardsUncovered { get; set; }
		public bool IsWinner { get; set; }

		public IUserProfile User { get; set; }
		public IGameSession GameSession { get; set; }
	}
}
