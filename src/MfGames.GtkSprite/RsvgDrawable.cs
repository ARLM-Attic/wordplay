using Gdk;
using MfGames.Utility;
using Rsvg;
using System;
using System.IO;

namespace MfGames.Sprite
{
	/// <summary>
	/// This class represents a single SVG image in memory. It has no
	/// animation or processing elements, just the basic display and
	/// rendering code.
	/// </summary>
	public class RsvgDrawable
	: PixbufDrawable
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
		protected override Gdk.Pixbuf CreatePixbuf(
			DrawableState state, string cacheKey)
		{
			// Render the image
			Gdk.Pixbuf p = Rsvg.Pixbuf.FromFileAtSize(file.FullName,
				state.Width, state.Height);
			PixbufCache[cacheKey] = p;
			return p;
		}
	}
}
