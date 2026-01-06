using Microsoft.EntityFrameworkCore;
using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Dao.Models;
using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.MemoGame.Dao
{
	public class SqliteDAO : IDataAccessObject
	{
		private readonly SqliteDbContext _db;

		public SqliteDAO()
		{
			_db = new SqliteDbContext();
			_db.Database.EnsureCreated();
		}

		public IEnumerable<IGameSession> GetAllGameSessions()
		{
			return _db.GameSessions.ToList();
		}

		public IEnumerable<IGameSession> GetAllGameSessionsForUser(IUserProfile userProfile)
		{
			var res = _db.GameSessions
				.Include(gs => gs.PlayerResultsConcrete)
				.ThenInclude(pgr => pgr.User)
				.Where(gs => gs.PlayerResultsConcrete
					.Any(pr => pr.UserProfileId == userProfile.Id))
				.ToList();
			
			return res;
		}

		public IGameSession CreateGameSession(DateTime date, TimeSpan duration, GameType gameType, GameMode gameMode, IEnumerable<IUserProfile> users, int totalPairs)
		{
			var gameSession = new GameSession
			{
				GameDate = date,
				Duration = duration,
				GameType = gameType,
				GameMode = gameMode,
				PlayerResultsConcrete = new List<PlayerGameResult>()
			};

			foreach (var user in users)
			{
				var result = new PlayerGameResult
				{
					UserProfileId = user.Id,
					GameSession = gameSession,
					CardsUncovered = totalPairs,
					IsWinner = true
				};
				gameSession.PlayerResultsConcrete.Add(result);
			}

			_db.GameSessions.Add(gameSession);
			_db.SaveChanges();

			return gameSession;
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


		// User
		public IEnumerable<IUserProfile> GetAllUserProfiles()
		{
			return _db.UserProfiles.ToList();
		}

		public IUserProfile GetFirstUserProfile()
		{
			return _db.UserProfiles.First();
		}

		public IUserProfile CreateNewUserProfile(string userName)
		{
			var userProfile = new UserProfile { UserName = userName };
			_db.UserProfiles.Add(userProfile);
			_db.SaveChanges();

			return userProfile;
		}

		public void DeleteUserProfile(int userProfileId)
		{
			using (var transaction = _db.Database.BeginTransaction())
			{
				try
				{
					// delete also in GameSessions and PlayerGameResults
					_db.GameSessions
						.Where(gs => gs.PlayerResultsConcrete.Any() &&
									 gs.PlayerResultsConcrete.All(pr => pr.UserProfileId == userProfileId))
						.ExecuteDelete();

					_db.PlayerGameResults
						.Where(pgr => pgr.UserProfileId == userProfileId)
						.ExecuteDelete();

					int rows = _db.UserProfiles
						.Where(u => u.Id == userProfileId)
						.ExecuteDelete();

					if (rows == 0)
					{
						transaction.Rollback();
					}
					else
					{
						transaction.Commit();
					}
				}
				catch (Exception)
				{
					transaction.Rollback();
				}
			}
		}

		public void UpdateUserProfile(IUserProfile userProfile)
		{
			var user = userProfile as UserProfile ?? throw new ArgumentException("userProfile must be of type UserProfile", nameof(userProfile));
			_db.UserProfiles.Update(user);
			_db.SaveChanges();
		}

		// Cards
		public IEnumerable<ICard> GetAllCards()
		{
			return _db.Cards.ToList();
		}

		public ICard CreateNewCard(string imagePath, string name)
		{
			var srcPath = ImageUtility.SaveImage(imagePath, name);
			var card = new Card { Name = name, ImagePath = srcPath };
			_db.Cards.Add(card);
			_db.SaveChanges();

			return card;
		}

		public ICard CreateNewCard(Stream fileStream, string fileName, string name)
		{
			var srcPath = ImageUtility.SaveImage(fileStream, fileName, name);
			var card = new Card { Name = name, ImagePath = srcPath };
			_db.Cards.Add(card);
			_db.SaveChanges();

			return card;
		}

		public void UpdateCardName(int cardId, string newName)
		{
			var dbCard = _db.Cards.Find(cardId);
			if (dbCard != null)
			{
				dbCard.Name = newName;
				_db.Cards.Update(dbCard);
				_db.SaveChanges();
			}
		}

		public void DeleteCard(int cardId)
		{
			var dbCard = _db.Cards.Find(cardId);
			if (dbCard != null)
			{
				_db.Cards.Remove(dbCard);
				_db.SaveChanges();
			}
		}
	}
}
