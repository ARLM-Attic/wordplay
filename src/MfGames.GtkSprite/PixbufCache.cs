using C5;
using Gdk;
using System.IO;

namespace MfGames.Sprite
{
	/// <summary>
	/// The pixbuf cache is used to store pixbufs that are
	/// generated or loaded. The primary intent is to speed up
	/// processing on heavily used pixbufs.
	///
	/// In most cases, the pixbuf is used as a singleton instance (via
	/// the Instance property), but a pixbuf may have its own cache
	/// for specialized purposes (such as backgrounds which have
	/// different cache rules).
	///
	/// The default cache implemention is an infinite cache that makes
	/// no effort to reduce its memory footprint.
	/// </summary>
	public class PixbufCache
	{
#region Singleton
		// Contains the static singleton instance
		private static PixbufCache instance = new PixbufCache();

		/// <summary>
		/// Contains the register singleton. The default
		/// implementation is given when this class is first created.
		/// </summary>
		public static PixbufCache Instance
		{
			get { return instance; }
			set { instance = value; }
		}
#endregion

#region Caching
		protected HashDictionary <string, Pixbuf> cache =
			new HashDictionary <string, Pixbuf> ();

		/// <summary>
		/// Returns a cached pixbuf. This assumes the Contains() has
		/// already been called.
		/// </summary>
		public virtual Pixbuf this[string key]
		{
			get { return cache[key]; }
			set { cache[key] = value; }
		}

		/// <summary>
		/// Returns true if the given key is located in memory.
		/// </summary>
		public virtual bool Contains(string key)
		{
			return cache.Contains(key);
		}

		/// <summary>
		/// Clears out the cache.
		/// </summary>
		public void Clear()
		{
			cache.Clear();
		}
#endregion
	}
}
