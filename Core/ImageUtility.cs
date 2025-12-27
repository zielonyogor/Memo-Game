using System.IO;

namespace NR155910155992.MemoGame.Core
{
	public static class ImageUtility
	{
		private readonly static string _imagesFolder = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MemoGame/Images");

		public static string SaveImage(byte[] imageData, string imageName)
		{
			if(imageData == null || imageData.Length == 0)
				throw new ArgumentException("Image data cannot be null or empty.", nameof(imageData));

			if(!Directory.Exists(_imagesFolder))
				Directory.CreateDirectory(_imagesFolder);

			string imagePath = Path.Combine(_imagesFolder, imageName);
			File.WriteAllBytes(imagePath, imageData);
			return imagePath;
		}
		public static string SaveImage(string imagePath, string imageName)
		{
			if(!File.Exists(imagePath))
				throw new ArgumentException("Image file does not exist.", nameof(imagePath));
			if(!Directory.Exists(_imagesFolder))
				Directory.CreateDirectory(_imagesFolder);

			string destPath = Path.Combine(_imagesFolder, imageName);
			File.Copy(imagePath, destPath, true);
			return destPath;
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
