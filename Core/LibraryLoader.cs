using System.Reflection;
using System.Configuration;

namespace NR155910155992.MemoGame.Core
{
	public class LibraryLoader
	{
		public static T LoadObjectFromLibrary<T>(LibraryKey libraryKey)
		{
			string key = GetKeyFromLibraryEnum(libraryKey);
			// Load config of the entry assembly (UI project)
			var config = ConfigurationManager.OpenExeConfiguration(
				ConfigurationUserLevel.None
			);

			var dllName = config.AppSettings.Settings[key]?.Value;

			if (string.IsNullOrWhiteSpace(dllName))
				throw new Exception($"Nie skonfigurowano '{key}' w App.config!");

			if (!File.Exists(dllName))
				throw new FileNotFoundException($"Nie znaleziono biblioteki {key}: {dllName}");

			var assembly = Assembly.LoadFrom(dllName);
			
			var daoType = assembly
				.GetTypes()
				.FirstOrDefault(t =>
					typeof(T).IsAssignableFrom(t) &&
					!t.IsInterface &&
					!t.IsAbstract);

			if (daoType == null)
				throw new Exception($"Nie znaleziono implementacji {typeof(T).Name} w {dllName}");

			return (T)Activator.CreateInstance(daoType);
		}

		private static string GetKeyFromLibraryEnum(LibraryKey libraryKey)
		{
			return libraryKey switch
			{
				LibraryKey.Dao => "DaoLibraryName",
				LibraryKey.Bl => "BlLibraryName",
				_ => throw new ArgumentOutOfRangeException(nameof(libraryKey), libraryKey, null)
			};
		}
	}
}
