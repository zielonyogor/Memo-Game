using NR155910155992.MemoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR155910155992.MemoGame.BL
{
	internal class CardManager
	{
		IDataAccessObject _dao;

		public CardManager(IDataAccessObject dao)
		{
			_dao = dao;
		}

		public ICard GetCardById(int cardId)
		{
			return _dao.GetAllCards().First(c => c.Id == cardId);
		}

		public IEnumerable<ICard> GetAllCards()
		{
			return _dao.GetAllCards();
		}

		public ICard CreateNewCard(string imagePath, string name)
		{
			return _dao.CreateNewCard(imagePath, name);
		}

		public void DeleteCard(int cardId)
		{
			_dao.DeleteCard(cardId);
		}

		public void UpdateCardName(int cardId, string newName)
		{
			_dao.UpdateCardName(cardId, newName);
		}
	}
}
