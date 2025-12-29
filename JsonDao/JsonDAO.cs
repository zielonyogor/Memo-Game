using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.JsonDao.Models;
using System.Diagnostics;

namespace NR155910155992.MemoGame.JsonDao
{
	public class JsonDAO : IDataAccessObject
	{
		private readonly string _folder;
		private readonly string _cardsPath;
		private readonly string _userProfilesPath;
		private readonly string _gameSessionsPath;

		private List<Card> _cards = new();
		private List<UserProfile> _userProfiles = new();
		private List<GameSession> _gameSessions = new();

		private List<PlayerGameResult> _playerGameResults = new();

		public JsonDAO()
		{
			_folder = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MemoGame/Data");

			Directory.CreateDirectory(_folder);

			_cardsPath = Path.Combine(_folder, "cards.json");
			_userProfilesPath = Path.Combine(_folder, "userProfiles.json");
			_gameSessionsPath = Path.Combine(_folder, "gameSessions.json");

			bool anyMissing =
				!File.Exists(_cardsPath) ||
				!File.Exists(_userProfilesPath) ||
				!File.Exists(_gameSessionsPath);

			if (anyMissing)
				SeedData();
			else
				LoadData();
		}

		public IEnumerable<ICard> GetAllCards() => _cards;
		public IUserProfile GetFirstUserProfile() => _userProfiles.FirstOrDefault();
		public IEnumerable<IUserProfile> GetAllUserProfiles() => _userProfiles;

		public IUserProfile CreateNewUserProfile(string userName)
		{
			var userProfile = new UserProfile { Id = Guid.NewGuid().GetHashCode(), UserName = userName };
			_userProfiles.Add(userProfile);
			SaveToFile(_userProfilesPath, _userProfiles);
			return userProfile;
		}

		public void DeleteUserProfile(int userProfileId)
		{
			_userProfiles.RemoveAll(up => up.Id == userProfileId);
			SaveToFile(_userProfilesPath, _userProfiles);
		}

		public void UpdateUserProfile(IUserProfile userProfile)
		{
			var index = _userProfiles.FindIndex(up => up.Id == userProfile.Id);
			if (index != -1)
			{
				_userProfiles[index] = userProfile as UserProfile ?? throw new ArgumentException("Invalid type", nameof(userProfile));
				SaveToFile(_userProfilesPath, _userProfiles);
			}
		}

		public ICard CreateNewCard(string imagePath, string name)
		{
			var path = ImageUtility.SaveImage(imagePath, name);
			var card = new Card { Id = Guid.NewGuid().GetHashCode(), Name = name, ImagePath = path };
			_cards.Add(card);
			SaveToFile(_cardsPath, _cards);
			return card;
		}

		public void DeleteCard(int cardId)
		{
			_cards.RemoveAll(c => c.Id == cardId);
			SaveToFile(_cardsPath, _cards);
		}

		public void UpdateCardName(int cardId, string newName)
		{
			var index = _cards.FindIndex(c => c.Id == cardId);
			if (index != -1)
			{
				_cards[index].Name = newName;
				SaveToFile(_cardsPath, _cards);
			}
		}

		public IEnumerable<IGameSession> GetAllGameSessions()
		{
			return _gameSessions;
		}

		public IEnumerable<IGameSession> GetAllGameSessionsForUser(IUserProfile userProfile)
		{
			return _gameSessions
				.Where(gs => gs.PlayerResults
				.Any(pr => pr.User.Id == userProfile.Id));
		}

		public IEnumerable<IPlayerGameResult> GetAllPlayerGameResultsForGameSession(IGameSession gameSession)
		{
			return gameSession.PlayerResults;
		}

		public IGameSession CreateGameSession(DateTime date, TimeSpan duration, GameType gameType, GameMode gameMode, IEnumerable<IUserProfile> users, int totalPairs)
		{
			int gameSessionId = Guid.NewGuid().GetHashCode();

			var gameSession = new GameSession
			{
				Id = gameSessionId,
				GameDate = date,
				Duration = duration,
				GameType = gameType,
				GameMode = gameMode,
				PlayerResultsConcrete = new List<PlayerGameResult>()
			};

			var playerResults = users.Select(user => new PlayerGameResult()
			{
				Id = Guid.NewGuid().GetHashCode(),
				CardsUncovered = totalPairs,
				IsWinner = true, // Logic for winner might need adjustment
				User = user as UserProfile ?? throw new ArgumentException("Invalid UserProfile type"),
				UserProfileId = user.Id,
				GameSession = gameSession,
				GameSessionId = gameSessionId
			}).ToList();

			gameSession.PlayerResultsConcrete.AddRange(playerResults);

			_gameSessions.Add(gameSession);
			_playerGameResults.AddRange(playerResults);

			SaveToFile(_gameSessionsPath, _gameSessions);

			return gameSession;
		}

		public IPlayerGameResult CreatePlayerGameResult(IUserProfile userProfile, IGameSession gameSession, int cardsUncovered, bool isWinner)
		{
			var concreteSession = _gameSessions.FirstOrDefault(gs => gs.Id == gameSession.Id);
			if (concreteSession == null)
				throw new ArgumentException("Game session not found");

			var playerGameResult = new PlayerGameResult
			{
				Id = Guid.NewGuid().GetHashCode(),
				CardsUncovered = cardsUncovered,
				IsWinner = isWinner,
				GameSession = concreteSession,
				GameSessionId = concreteSession.Id,
				User = userProfile as UserProfile ?? throw new ArgumentException("Invalid UserProfile type"),
				UserProfileId = userProfile.Id
			};

			concreteSession.PlayerResultsConcrete.Add(playerGameResult);
			_playerGameResults.Add(playerGameResult);
			SaveToFile(_gameSessionsPath, _gameSessions);

			return playerGameResult;
		}

		private void LoadData()
		{
			_cards = LoadFromFile<Card>(_cardsPath);
			_userProfiles = LoadFromFile<UserProfile>(_userProfilesPath);
			_gameSessions = LoadFromFile<GameSession>(_gameSessionsPath);

			_playerGameResults = _gameSessions
				.SelectMany(gs => gs.PlayerResultsConcrete)
				.ToList();

			HydrateReferences();
		}

		private void SeedData()
		{
			Directory.CreateDirectory(_folder);

			_cards = new List<Card>
			{
				new Card { Id = 1, Name = "Card Flower", ImagePath = "Assets/Cards/card1.png" },
				new Card { Id = 2, Name = "Card House", ImagePath = "Assets/Cards/card2.png" },
				new Card { Id = 3, Name = "Card Cloud", ImagePath = "Assets/Cards/card3.png" },
				new Card { Id = 4, Name = "Card Bee", ImagePath = "Assets/Cards/card4.png" },
				new Card { Id = 5, Name = "Card Sun", ImagePath = "Assets/Cards/card5.png" }
			};

			_userProfiles = new List<UserProfile>
			{
				new UserProfile { Id = 1, UserName = "PlayerX" },
				new UserProfile { Id = 2, UserName = "PlayerY" }
			};

			// Create Sessions
			var s1 = new GameSession { Id = 1, GameDate = new DateTime(2025, 1, 1), Duration = TimeSpan.FromSeconds(90), GameMode = GameMode.Pairs, GameType = GameType.Solo };
			var s2 = new GameSession { Id = 2, GameDate = new DateTime(2025, 1, 2), Duration = TimeSpan.FromSeconds(150), GameMode = GameMode.Pairs, GameType = GameType.Solo };
			var s3 = new GameSession { Id = 3, GameDate = new DateTime(2025, 1, 3), Duration = TimeSpan.FromSeconds(200), GameMode = GameMode.Pairs, GameType = GameType.Multiplayer };

			// Create Results
			var r1 = new PlayerGameResult { Id = 1, GameSessionId = 1, UserProfileId = 1, IsWinner = true, CardsUncovered = 12 };
			var r2 = new PlayerGameResult { Id = 2, GameSessionId = 2, UserProfileId = 2, IsWinner = false, CardsUncovered = 8 };
			var r3 = new PlayerGameResult { Id = 3, GameSessionId = 3, UserProfileId = 1, IsWinner = true, CardsUncovered = 10 };
			var r4 = new PlayerGameResult { Id = 4, GameSessionId = 3, UserProfileId = 2, IsWinner = false, CardsUncovered = 7 };

			// Nest Results into Sessions
			s1.PlayerResultsConcrete.Add(r1);
			s2.PlayerResultsConcrete.Add(r2);
			s3.PlayerResultsConcrete.Add(r3);
			s3.PlayerResultsConcrete.Add(r4);

			_gameSessions = new List<GameSession> { s1, s2, s3 };

			_playerGameResults = new List<PlayerGameResult> { r1, r2, r3, r4 };

			HydrateReferences();

			SaveToFile(_cardsPath, _cards);
			SaveToFile(_userProfilesPath, _userProfiles);
			SaveToFile(_gameSessionsPath, _gameSessions);
		}

		private void HydrateReferences()
		{
			foreach (var session in _gameSessions)
			{
				foreach (var result in session.PlayerResultsConcrete)
				{
					result.User = _userProfiles.FirstOrDefault(u => u.Id == result.UserProfileId);
					result.GameSession = session;
				}
			}
		}

		private List<T> LoadFromFile<T>(string path)
		{
			if (!File.Exists(path))
				return new List<T>();

			var json = File.ReadAllText(path);
			return System.Text.Json.JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
		}

		private void SaveToFile<T>(string path, List<T> data)
		{
			var json = System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions
			{
				WriteIndented = true
			});
			File.WriteAllText(path, json);
		}
	}
}