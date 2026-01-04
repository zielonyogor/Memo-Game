using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.WebUI.Models;

namespace NR155910155992.MemoGame.WebUI.Controllers
{
	public class HistoryController : Controller
	{
		private readonly IGameManager _gameManager;
		public HistoryController(IGameManager gameManager) 
		{
			_gameManager = gameManager;
		}

		// GET: HistoryController
		public ActionResult Index()
		{
			var sessions = _gameManager.GetAllGameSessionsForCurrentUser();
			var history = new List<GameSessionModel>();
			foreach (var session in sessions)
			{
				var gameHistory = new GameSessionModel(session);
				history.Add(gameHistory);
			}
			return View(history);
		}

		// GET: HistoryController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}
	}
}
