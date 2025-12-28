using NR155910155992.MemoGame.UI.Stores;
using NR155910155992.MemoGame.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR155910155992.MemoGame.UI.Services
{
	public class ParameterModalNavigationService<TParameter, TViewModel>
		: IParameterNavigationService<TParameter>
		where TViewModel : ViewModelBase
	{
		private readonly ModalNavigationStore _modalNavigationStore;
		private readonly Func<TParameter, TViewModel> _createViewModel;

		public ParameterModalNavigationService(
			ModalNavigationStore modalNavigationStore,
			Func<TParameter, TViewModel> createViewModel)
		{
			_modalNavigationStore = modalNavigationStore;
			_createViewModel = createViewModel;
		}

		public void Navigate(TParameter parameter)
		{
			_modalNavigationStore.CurrentViewModel = _createViewModel(parameter);
		}
	}
}
