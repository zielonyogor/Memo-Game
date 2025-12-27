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
		private readonly string _playerGameResultsPath;

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
			_playerGameResultsPath = Path.Combine(_folder, "playerGameResults.json");

			bool anyMissing =
				!File.Exists(_cardsPath) ||
				!File.Exists(_userProfilesPath) ||
				!File.Exists(_gameSessionsPath) ||
				!File.Exists(_playerGameResultsPath);

			if (anyMissing)
				SeedData();
			else
				LoadData();
		}

		public IEnumerable<ICard> GetAllCards()
		{
			return _cards;
		}

		public IEnumerable<IGameSession> GetAllGameSessions()
		{
			return _gameSessions;
		}

		public IEnumerable<IGameSession> GetAllGameSessionsForUser(IUserProfile userProfile)
		{
			return _gameSessions
				.Where(gs => gs.PlayerResults
				.Any(pr => pr.UserProfileId == userProfile.Id));
		}
		public IEnumerable<IPlayerGameResult> GetAllPlayerGameResultsForGameSession(IGameSession gameSession)
		{
			return _playerGameResults
				.Where(pgr => pgr.GameSessionId == gameSession.Id)
				.ToList();
		}

		public IGameSession CreateGameSession(DateTime date, TimeSpan duration, GameType gameType, GameMode gameMode, IEnumerable<IUserProfile> users)
		{
			var gameSession = new GameSession
			{
				Id = Guid.NewGuid().GetHashCode(),
				GameDate = date,
				Duration = duration,
				GameType = gameType,
				GameMode = gameMode,
			};

            Debug.WriteLine($"Before adding number of game sessions in dao list: {_gameSessions.Count()}");
            _gameSessions.Add(gameSession);
			Debug.WriteLine($"Created GameSession with Id: {gameSession.Id}, number of game sessions in dao list: {_gameSessions.Count()}");
            SaveToFile(_gameSessionsPath, _gameSessions);

			return gameSession;
		}

		public IUserProfile CreateNewUserProfile(string userName)
		{
			var userProfile = new UserProfile { Id = Guid.NewGuid().GetHashCode(), UserName = userName };
			_userProfiles.Add(userProfile);
			SaveToFile(_userProfilesPath, _userProfiles);

			return userProfile;
		}

		public ICard CreateNewCard(string imagePath, string name)
		{
			var card = new Card { Id = Guid.NewGuid().GetHashCode(), Name = name, ImagePath = imagePath };
			_cards.Add(card);
			SaveToFile(_cardsPath, _cards);

			return card;
		}

		public IPlayerGameResult CreatePlayerGameResult(IUserProfile userProfile, IGameSession gameSession, int cardsUncovered, bool isWinner)
		{
			var playerGameResult = new PlayerGameResult
			{
				Id = Guid.NewGuid().GetHashCode(),
				CardsUncovered = cardsUncovered,
				IsWinner = isWinner,
				GameSession = gameSession as GameSession ?? throw new ArgumentException("gameSession must be of type GameSession", nameof(gameSession)),
				User = userProfile as UserProfile ?? throw new ArgumentException("userProfile must be of type UserProfile", nameof(userProfile))
			};
			_playerGameResults.Add(playerGameResult);
			SaveToFile(_playerGameResultsPath, _playerGameResults);

			return playerGameResult;
		}

		public IUserProfile GetFirstUserProfile()
		{
			return _userProfiles[0];
		}

		public IEnumerable<IUserProfile> GetAllUserProfiles()
		{
			return _userProfiles;
		}

		public void DeleteUserProfile(IUserProfile userProfile)
		{
			_userProfiles.RemoveAll(up => up.Id == userProfile.Id);
			SaveToFile(_userProfilesPath, _userProfiles);
		}

		public void UpdateUserProfile(IUserProfile userProfile)
		{
			var index = _userProfiles.FindIndex(up => up.Id == userProfile.Id);
			if (index != -1)
			{
				_userProfiles[index] = userProfile as UserProfile ?? throw new ArgumentException("userProfile must be of type UserProfile", nameof(userProfile));
				SaveToFile(_userProfilesPath, _userProfiles);
			}
		}

		private void LoadData()
		{
			_cards = LoadFromFile<Card>(_cardsPath);
			_userProfiles = LoadFromFile<UserProfile>(_userProfilesPath);
			_gameSessions = LoadFromFile<GameSession>(_gameSessionsPath);
			_playerGameResults = LoadFromFile<PlayerGameResult>(_playerGameResultsPath);
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
			_gameSessions = new List<GameSession>
			{
				new GameSession
				{
					Id = 1,
					GameDate = new DateTime(2025, 1, 1, 12, 0, 0),
					Duration = TimeSpan.FromSeconds(90),
					GameMode = GameMode.Pairs,
					GameType = GameType.Solo
				},
				new GameSession
				{
					Id = 2,
					GameDate = new DateTime(2025, 1, 2, 15, 30, 0),
					Duration = TimeSpan.FromSeconds(150),
					GameMode = GameMode.Pairs,
					GameType = GameType.Solo
				},
				new GameSession
				{
					Id = 3,
					GameDate = new DateTime(2025, 1, 3, 18, 45, 0),
					Duration = TimeSpan.FromSeconds(200),
					GameMode = GameMode.Pairs,
					GameType = GameType.Multiplayer
				}
			};
			_playerGameResults = new List<PlayerGameResult>
			{
				new PlayerGameResult { Id = 1, GameSessionId = 1, UserProfileId = 1, IsWinner = true, CardsUncovered = 12 },
				new PlayerGameResult { Id = 2, GameSessionId = 2, UserProfileId = 2, IsWinner = false, CardsUncovered = 8 },
				new PlayerGameResult { Id = 3, GameSessionId = 3, UserProfileId = 1, IsWinner = true, CardsUncovered = 10 },
				new PlayerGameResult { Id = 4, GameSessionId = 3, UserProfileId = 2, IsWinner = false, CardsUncovered = 7 }
			};

			foreach (var result in _playerGameResults)
			{
				result.User = _userProfiles.FirstOrDefault(u => u.Id == result.UserProfileId);
				result.GameSession = _gameSessions.FirstOrDefault(gs => gs.Id == result.GameSessionId);
				var session = _gameSessions.FirstOrDefault(gs => gs.Id == result.GameSessionId);
				if (session != null)
				{
					session.PlayerResults.Add(result);
				}
			}

			HydrateReferences();

			SaveToFile(_cardsPath, _cards);
			SaveToFile(_userProfilesPath, _userProfiles);
			SaveToFile(_gameSessionsPath, _gameSessions);
			SaveToFile(_playerGameResultsPath, _playerGameResults);
		}

		/// <summary>
		/// Updates the references between PlayerGameResults, UserProfiles, and GameSessions after loading from JSON.
		/// </summary>
		private void HydrateReferences()
		{
			foreach (var result in _playerGameResults)
			{
				result.User = _userProfiles.FirstOrDefault(u => u.Id == result.UserProfileId);
				var session = _gameSessions.FirstOrDefault(gs => gs.Id == result.GameSessionId);
				result.GameSession = session;

				if (session != null)
				{
					if (!session.PlayerResults.Any(pr => pr.Id == result.Id))
					{
						session.PlayerResults.Add(result);
					}
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
