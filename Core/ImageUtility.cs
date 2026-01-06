using System.IO;

namespace NR155910155992.MemoGame.Core
{
	public static class ImageUtility
	{
		private readonly static string _imagesFolder = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MemoGame/Images");

		public static string SaveImage(string imagePath, string imageName)
		{
			if(!File.Exists(imagePath))
				throw new ArgumentException("Image file does not exist.", nameof(imagePath));
			if(!Directory.Exists(_imagesFolder))
				Directory.CreateDirectory(_imagesFolder);

			string imageExt = Path.GetExtension(imageName);
			string destPath = Path.Combine(_imagesFolder, Guid.NewGuid().ToString() + imageExt);
			File.Copy(imagePath, destPath, true);
			return destPath;
		}
		public static string SaveImage(Stream sourceStream, string originalFileName, string name)
		{
			string extension = Path.GetExtension(originalFileName);
			string destFileName = $"{Guid.NewGuid()}{extension}";

			string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MemoGame", "Cards");
			Directory.CreateDirectory(folder);
			string destinationPath = Path.Combine(folder, destFileName);

			using (var fileStream = new FileStream(destinationPath, FileMode.Create))
			{
				sourceStream.CopyTo(fileStream);
			}

			return destinationPath;
		}

		public static bool DeleteImage(string imageName)
		{
			string imagePath = Path.Combine(_imagesFolder, imageName);
			if (File.Exists(imagePath))
			{
				File.Delete(imagePath);
				return true;
			}
			return false;
		}

		public static string GetImagePath(string imageName)
		{
			return Path.Combine(GetImagesFolderPath(), imageName);
		}

		public static string ResolvePath(string imagePath)
		{
			if (Path.IsPathRooted(imagePath))
				return imagePath;

			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
		}

		public static string GetImagesFolderPath()
		{
			Directory.CreateDirectory(_imagesFolder);
			return _imagesFolder;
		}
	}
}
