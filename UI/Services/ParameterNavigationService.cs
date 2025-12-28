using NR155910155992.MemoGame.UI.Stores;
using NR155910155992.MemoGame.UI.ViewModels;

namespace NR155910155992.MemoGame.UI.Services
{
	public  class ParameterNavigationService<TParameter, TViewModel>
		: IParameterNavigationService<TParameter>
		where TViewModel : ViewModelBase
	{
		private readonly NavigationStore _navigationStore;
		private readonly Func<TParameter, TViewModel> _createViewModel;

		public ParameterNavigationService(
			NavigationStore navigationStore,
			Func<TParameter, TViewModel> createViewModel)
		{
			_navigationStore = navigationStore;
			_createViewModel = createViewModel;
		}

		public void Navigate(TParameter parameter)
		{
			_navigationStore.CurrentViewModel = _createViewModel(parameter);
		}
	}
}
