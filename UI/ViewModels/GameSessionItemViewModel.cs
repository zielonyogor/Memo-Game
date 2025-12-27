using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    public class GameSessionItemViewModel
    {
		private readonly IGameSession _session;

		public GameSessionItemViewModel(IGameSession session)
		{
			_session = session;
			Debug.WriteLine($"Got session: {session.Id}");
		}

		public DateTime GameDate => _session.GameDate;
		public string DateFormatted => GameDate.ToString("yyyy-MM-dd HH:mm");

		public string DurationFormatted => _session.Duration.ToString(@"mm\:ss");

		public GameType GameType => _session.GameType;
		public GameMode GameMode => _session.GameMode;

		public int PlayerCount => _session.PlayerResults.Count();

		public string PlayerNames =>
			string.Join(", ",
				_session.PlayerResults.Select(p => p.User.UserName));

		public string Winner =>
			_session.PlayerResults
				.Where(p => p.IsWinner)
				.Select(p => p.User.UserName)
				.FirstOrDefault() ?? "?";

		public int TotalCardsUncovered =>
			_session.PlayerResults.Sum(p => p.CardsUncovered);
	}
}
