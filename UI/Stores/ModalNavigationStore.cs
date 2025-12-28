using NR155910155992.MemoGame.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR155910155992.MemoGame.UI.Stores
{
	public class ModalNavigationStore
	{
		private ViewModelBase _currentViewModel;
		public ViewModelBase CurrentViewModel {
			get => _currentViewModel;
			set
			{
				_currentViewModel?.Dispose();
				_currentViewModel = value;
				OnCurrentViewModelChanged();
			}
		}

		public bool IsOpen => CurrentViewModel != null;

		public event Action? CurrentViewModelChanged;

		public void Close()
		{
			CurrentViewModel = null;
		}

		private void OnCurrentViewModelChanged()
		{
			CurrentViewModelChanged?.Invoke();
		}
	}
}
