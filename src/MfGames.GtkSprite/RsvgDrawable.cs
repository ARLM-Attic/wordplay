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

using System.IO;

using Gdk;

#endregion

namespace MfGames.Sprite
{
	/// <summary>
	/// This class represents a single SVG image in memory. It has no
	/// animation or processing elements, just the basic display and
	/// rendering code.
	/// </summary>
	public class RsvgDrawable : PixbufDrawable
	{
		/// <summary>
		/// Constructs the drawable with a given FileInfo object.
		/// </summary>
		public RsvgDrawable(FileInfo file)
			: base(file)
		{
		}

		/// <summary>
		/// An internal function to actually create the rsvg. This
		/// method also adds it to the cache.
		/// </summary>
		protected override Pixbuf CreatePixbuf(
			DrawableState state,
			string cacheKey)
		{
			// Render the image
			Pixbuf p = Rsvg.Pixbuf.FromFileAtSize(
				file.FullName, state.Width, state.Height);
			PixbufCache[cacheKey] = p;
			return p;
		}
	}
}