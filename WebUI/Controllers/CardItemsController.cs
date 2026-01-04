using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.WebUI.Models;

namespace NR155910155992.MemoGame.WebUI.Controllers
{
	public class CardItemsController : Controller
	{
		private readonly IGameManager _gameManager;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public CardItemsController(IGameManager gameManager, IWebHostEnvironment webHostEnvironment)
		{
			_gameManager = gameManager;
			_webHostEnvironment = webHostEnvironment;
		}

		// GET: CardItemsController
		public ActionResult Index()
		{
			var cards = _gameManager.GetAllCards();
			var cardList = new List<CardItemModel>();
			foreach (var card in cards)
			{
				cardList.Add(new CardItemModel(card));
			}
			return View(cardList);
		}
		public IActionResult Create()
		{
			return View();
		}

		// POST: CardItems/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,ImagePath")] CardItemModel cardItem) // TODO: not working, should we store image the same as WPF app (copy to local)?
		{
			Console.WriteLine($"Creating card: {cardItem.Name}, {cardItem.ImagePath}");
			if (ModelState.IsValid)
			{
				_gameManager.CreateNewCard(cardItem.Name, cardItem.ImagePath);
				return RedirectToAction(nameof(Index));
			}
			return View(cardItem);
		}

		// GET: CardItemsController/Edit/5
		public ActionResult Edit(int id)
		{
			var card = _gameManager.GetAllCards().FirstOrDefault(c => c.Id == id);
			if (card == null)
			{
				return NotFound();
			}
			var cardItem = new CardItemModel(card);
			return View(cardItem);
		}

		// POST: CardItemsController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			string? name = collection["Name"];
			if (name == null)
			{
				return View();
			}

			_gameManager.UpdateCardName(id, name);
			return RedirectToAction(nameof(Index));
		}

		// POST: CardItems/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			try
			{
				_gameManager.DeleteCard(id);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View(nameof(Index));
			}
		}

		[HttpGet]
		public IActionResult GetImage(int id)
		{
			// 1. Get the card metadata
			var card = _gameManager.GetAllCards().FirstOrDefault(c => c.Id == id);
			if (card == null)
				return NotFound();

			string imagePath = card.ImagePath;
			string finalPath;
			string contentType = "image/png"; // You might want a helper to detect jpg/png

			// 2. Determine where the file actually lives
			if (System.IO.Path.IsPathFullyQualified(imagePath))
			{
				// CASE A: It's an absolute path (C:\Users\...)
				// This is your User Data
				finalPath = imagePath;
			}
			else
			{
				// CASE B: It's a relative path (Assets/Cards/...)
				// You should copy your "Assets" folder into "wwwroot" for the web project.
				// Example: wwwroot/Assets/Cards/image_1.png
				finalPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
			}

			// 3. Check if file exists and return it
			if (!System.IO.File.Exists(finalPath))
			{
				// Return a default placeholder image if the file is missing
				return NotFound("Image file not found on server.");
			}

			// 4. Serve the bytes
			var fileBytes = System.IO.File.ReadAllBytes(finalPath);
			return File(fileBytes, contentType);
		}
	}
}
