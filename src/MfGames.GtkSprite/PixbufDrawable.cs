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

using System;
using System.IO;

using Gdk;

using GC=Gdk.GC;

#endregion

namespace MfGames.Sprite
{
	/// <summary>
	/// This class represents a single pixbuf in memory. It has no
	/// animation or processing elements, just the basic display and
	/// rendering code.
	/// </summary>
	public class PixbufDrawable : IDrawable
	{
		protected FileInfo file;

		/// <summary>
		/// Constructs the drawable with a given FileInfo object.
		/// </summary>
		public PixbufDrawable(FileInfo file)
		{
			this.file = file;
		}

		/// <summary>
		/// Contains the pixbuf cache to use for drawing.
		/// </summary>
		public virtual PixbufCache PixbufCache
		{
			get { return PixbufCache.Instance; }
		}

		/// <summary>
		/// An internal function to actually create the pixbuf. This
		/// method also adds it to the cache.
		/// </summary>
		protected virtual Pixbuf CreatePixbuf(
			DrawableState state,
			string cacheKey)
		{
			// Create the image from the disk. If we are exactly the
			// right size, return it immediately.
			Pixbuf image = new Pixbuf(file.FullName);

			if (image.Width == state.Width && image.Height == state.Height)
			{
				return image;
			}

			// Create a pixbuf of the given size
			Colorspace colorspace = image.Colorspace;
			bool hasAlpha = image.HasAlpha;
			int bitsPerSample = image.BitsPerSample;
			Pixbuf p = new Pixbuf(
				colorspace, hasAlpha, bitsPerSample, state.Width, state.Height);

			// Scale copy it in
			image.Composite(
				p,
				0,
				0,
				state.Width,
				state.Height,
				0,
				0,
				image.Width / state.Width,
				image.Height / state.Height,
				InterpType.Bilinear,
				255);

			// Cache and return
			PixbufCache[cacheKey] = p;
			return p;
		}

		/// <summary>
		/// Contains the cache key for this drawable.
		/// </summary>
		public virtual string GetCacheKey(DrawableState state)
		{
			return String.Format(
				"file://{0}:{1}x{2}", file.FullName, state.Width, state.Height);
		}

		/// <summary>
		/// Contains the rendered pixbuf of this element.
		/// </summary>
		public virtual Pixbuf GetPixbuf(DrawableState state)
		{
			// Figure out our key for the cache
			string cacheKey = GetCacheKey(state);

			// Check the cache
			if (PixbufCache.Contains(cacheKey))
			{
				return PixbufCache[cacheKey];
			}
			else
			{
				return CreatePixbuf(state, cacheKey);
			}
		}

		/// <summary>
		/// Randomizes the drawable state based on this specific
		/// drawable.
		/// </summary>
		public virtual void Randomize(DrawableState state)
		{
		}

		/// <summary>
		/// This method is called when the drawable has to draw itself
		/// to a Gdk.Drawable area. This handles the scaling and does
		/// the actual image loading.
		/// </summary>
		public void Render(
			Drawable dest,
			GC gc,
			DrawableState state)
		{
			Pixbuf pixbuf = GetPixbuf(state);

			dest.DrawPixbuf(
				gc,
				pixbuf,
				0,
				0,
				state.X,
				state.Y,
				pixbuf.Width,
				pixbuf.Height,
				RgbDither.None,
				0,
				0);
		}

		/// <summary>
		/// Updates the drawable. If the drawable changed in any
		/// visible manner, then this returns true.
		/// </summary>
		public bool Update(DrawableState state)
		{
			return false;
		}
	}
}