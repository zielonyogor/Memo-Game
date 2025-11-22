using NR155910155992.MemoGame.Core;


namespace NR155910155992.MemoGame.Interfaces
{
	public interface IDataAccessObject
	{
		public IEnumerable<ICard> GetAllCards();
		public IEnumerable<IGameStatistics> GetAllGameStatistics();
		public IEnumerable<IUserProfile> GetAllUserProfiles();

		public ICard CreateNewCard(string imagePath, string name);
		public IUserProfile CreateNewUserProfile(string userName);
		public IGameStatistics CreateGameStatistics(DateTime date, TimeSpan duration, int cardsUncovered, GameType gameType);

	}
}
