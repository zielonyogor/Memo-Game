using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.WebUI.Models;

namespace NR155910155992.MemoGame.WebUI.Controllers
{
	public class CardItemsController : Controller
	{
		private readonly IGameManager _gameManager;

		public CardItemsController(IGameManager gameManager)
		{
			_gameManager = gameManager;
		}

		// GET: CardItemsController
		public ActionResult Index()
		{
			var cards = _gameManager.GetAllCards();
			var cardList = new List<CardItem>();
			foreach (var card in cards)
			{
				cardList.Add(new CardItem(card));
			}
			return View(cardList);
		}

		// GET: CardItemsController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: CardItemsController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: CardItemsController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: CardItemsController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: CardItemsController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: CardItemsController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: CardItemsController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				_gameManager.DeleteCard(id);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
