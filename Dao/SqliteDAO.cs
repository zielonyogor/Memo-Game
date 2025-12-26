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

		public IEnumerable<IGameSession> GetAllGameSessions()
		{
			return _db.GameSessions.ToList();
		}

		public IEnumerable<IUserProfile> GetAllUserProfiles()
		{
			return _db.UserProfiles.ToList();
		}

		public IUserProfile GetFirstUserProfile()
		{
			return _db.UserProfiles.First();
		}

		public IEnumerable<IGameSession> GetAllGameSessionsForUser(IUserProfile userProfile)
		{
			return _db.GameSessions
				.Where(gs => gs.PlayerResults
				.Any(pr => pr.UserProfileId == userProfile.Id))
				.ToList();
		}

		public IEnumerable<IPlayerGameResult> GetAllPlayerGameResultsForGameSession(IGameSession gameSession)
		{
			return _db.PlayerGameResults
				.Where(pgr => pgr.GameSessionId == gameSession.Id)
				.ToList();
		}

		public IGameSession CreateGameSession(DateTime date, TimeSpan duration, GameType gameType, GameMode gameMode)
		{
			var gameSession = new GameSession
			{
				GameDate = date,
				Duration = duration,
				GameType = gameType,
				GameMode = gameMode
			};

			_db.GameSessions.Add(gameSession);
			_db.SaveChanges();

			return gameSession;
		}

		public IUserProfile CreateNewUserProfile(string userName)
		{
			var userProfile = new UserProfile { UserName = userName };
			_db.UserProfiles.Add(userProfile);
			_db.SaveChanges();

			return userProfile;
		}

		public ICard CreateNewCard(string imagePath, string name)
		{
			var card = new Card { Name = name, ImagePath = imagePath };
			_db.Cards.Add(card);
			_db.SaveChanges();

			return card;
		}

        public IPlayerGameResult CreatePlayerGameResult(IUserProfile userProfile, IGameSession gameSession, int cardsUncovered, bool isWinner)
		{
			var playerGameResult = new PlayerGameResult
			{
				CardsUncovered = cardsUncovered,
				IsWinner = isWinner,
				GameSession = gameSession as GameSession ?? throw new ArgumentException("gameSession must be of type GameSession", nameof(gameSession)),
				User = userProfile as UserProfile ?? throw new ArgumentException("userProfile must be of type UserProfile", nameof(userProfile))
			};
			_db.PlayerGameResults.Add(playerGameResult);

			return playerGameResult;
		}

		public void DeleteUserProfile(IUserProfile userProfile)
		{
			var user = userProfile as UserProfile ?? throw new ArgumentException("userProfile must be of type UserProfile", nameof(userProfile));
			_db.UserProfiles.Remove(user);
			_db.SaveChanges();
		}

		public void UpdateUserProfile(IUserProfile userProfile)
		{
			var user = userProfile as UserProfile ?? throw new ArgumentException("userProfile must be of type UserProfile", nameof(userProfile));
			_db.UserProfiles.Update(user);
			_db.SaveChanges();
		}
	}
}
