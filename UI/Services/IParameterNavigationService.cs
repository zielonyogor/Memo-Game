
namespace NR155910155992.MemoGame.UI.Services
{
	public interface IParameterNavigationService<in TParameter>
	{
		void Navigate(TParameter parameter);
	}
}
