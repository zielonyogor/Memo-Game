using Microsoft.AspNetCore.Mvc;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.WebUI.Models;
using System.Diagnostics;

namespace NR155910155992.WebUI.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IGameManager _gameManager;

		public HomeController(ILogger<HomeController> logger, IGameManager gameManager)
		{
			_logger = logger;
			_gameManager = gameManager;
		}

		public IActionResult Index()
		{
			var currentUser = _gameManager.GetCurrentUserProfile();
			return View(currentUser);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
