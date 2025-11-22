using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Dao.Models;
using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.MemoGame.Dao
{
	public class SqliteDAO : IDataAccessObject
	{
		private readonly SqliteDbContext _db = new SqliteDbContext();

		public IEnumerable<ICard> GetAllCards()
		{ 
			return _db.Cards.ToList(); 
		}

		public ICard CreateNewCard(string imagePath, string name)
		{
			var card = new Card { Name = name, ImagePath = imagePath };
			_db.Cards.Add(card);
			_db.SaveChanges();

			return card;
		}

		public IEnumerable<IGameStatistics> GetAllGameStatistics()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IUserProfile> GetAllUserProfiles()
		{
			throw new NotImplementedException();
		}

		IEnumerable<ICard> IDataAccessObject.GetAllCards()
		{
			throw new NotImplementedException();
		}

		IEnumerable<IGameStatistics> IDataAccessObject.GetAllGameStatistics()
		{
			throw new NotImplementedException();
		}

		IEnumerable<IUserProfile> IDataAccessObject.GetAllUserProfiles()
		{
			throw new NotImplementedException();
		}

		IGameStatistics IDataAccessObject.CreateGameStatistics(DateTime date, TimeSpan duration, int cardsUncovered, GameType gameType)
		{
			throw new NotImplementedException();
		}

		ICard IDataAccessObject.CreateNewCard(string imagePath, string name)
		{
			throw new NotImplementedException();
		}

		IUserProfile IDataAccessObject.CreateNewUserProfile(string userName)
		{
			throw new NotImplementedException();
		}
	}
}
