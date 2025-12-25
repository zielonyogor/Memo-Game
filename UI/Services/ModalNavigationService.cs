using NR155910155992.MemoGame.UI.Stores;
using NR155910155992.MemoGame.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR155910155992.MemoGame.UI.Services
{
	public class ModalNavigationService<TViewModel> : INavigationService
		where TViewModel : ViewModelBase
	{
		private readonly ModalNavigationStore _navigationStore;
		private readonly Func<TViewModel> _createViewModel;

		public ModalNavigationService(ModalNavigationStore navigationStore, Func<TViewModel> createViewModel)
		{
			_navigationStore = navigationStore;
			_createViewModel = createViewModel;
		}

		public void Navigate()
		{
			_navigationStore.CurrentViewModel = _createViewModel();
		}
	}
}
