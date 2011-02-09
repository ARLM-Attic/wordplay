#region Copyright and License

// Copyright (c) 2009-2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using C5;

using Gdk;

#endregion

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

		protected HashDictionary<string, Pixbuf> cache =
			new HashDictionary<string, Pixbuf>();

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
		/// Clears out the cache.
		/// </summary>
		public void Clear()
		{
			cache.Clear();
		}

		/// <summary>
		/// Returns true if the given key is located in memory.
		/// </summary>
		public virtual bool Contains(string key)
		{
			return cache.Contains(key);
		}

		#endregion
	}
}