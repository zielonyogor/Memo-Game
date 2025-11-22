namespace NR155910155992.MemoGame.Interfaces
{
	public interface ICard
	{
		public int Id { get; }
		public string ImagePath { get; protected set; }
		public string Name { get; protected set; } // ??
	}
}
