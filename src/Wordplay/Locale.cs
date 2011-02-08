using MfGames.Utility;
using System.Resources;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Encapsulates the basic functionality of the translation and
	/// globalization elements of the them.
	/// </summary>
	public class Locale
	: Logable
	{
		private static ResourceManager manager;

		/// <summary>
		/// Contains the resource manager for this locale.
		/// </summary>
		public static ResourceManager Resources
		{
			get
			{
				if (manager == null)
					manager = new ResourceManager("locale",
						typeof(Locale).Assembly);

				return manager;
			}
		}

		/// <summary>
		/// Attempts to translate the given text and return the
		/// results. If it cannot be found, it marks a small error
		/// then returns the key.
		/// </summary>
		public static string Translate(string key)
		{
			string value = Resources.GetString(key);

			if (value == null)
				return key;
			else
				return value;
		}
	}
}
