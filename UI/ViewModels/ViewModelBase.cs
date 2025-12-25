using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		public virtual void Dispose() { }
	}
}
