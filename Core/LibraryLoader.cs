using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace NR155910155992.MemoGame.Core
{
	public class LibraryLoader
	{
		private readonly IConfiguration _configuration;

		public LibraryLoader(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public T LoadObjectFromLibrary<T>(LibraryKey libraryKey, object[]? constructorArgs = null)
		{
			string key = GetKeyFromLibraryEnum(libraryKey);

			var dllName = _configuration[$"LibrarySettings:{key}"];

			if (string.IsNullOrWhiteSpace(dllName))
				throw new Exception($"Nie skonfigurowano '{key}' w appsettings.json (LibrarySettings:{key})!");

			string basePath = AppDomain.CurrentDomain.BaseDirectory;
			string fullPath = Path.Combine(basePath, dllName);

			if (!File.Exists(fullPath))
			{
				throw new FileNotFoundException($"Nie znaleziono biblioteki {key}. \nSzukano w: {fullPath}", fullPath);
			}

			var assembly = Assembly.LoadFrom(fullPath);

			var implementationType = assembly
				.GetTypes()
				.FirstOrDefault(t =>
					typeof(T).IsAssignableFrom(t) &&
					!t.IsInterface &&
					!t.IsAbstract);

			if (implementationType == null)
				throw new Exception($"Nie znaleziono implementacji {typeof(T).Name} w {dllName}");

			return (T)Activator.CreateInstance(implementationType, constructorArgs);
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
