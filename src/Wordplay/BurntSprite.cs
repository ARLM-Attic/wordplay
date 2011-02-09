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

using MfGames.Sprite;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// The burnt sprite handles the animation for a tile being
	/// burnt. It stops animating according to the controls, but it
	/// identifies itself as removable at frame eight.
	/// </summary>
	public class BurntSprite : DrawableSprite
	{
		/// <summary>
		/// Constructs a burnt sprite.
		/// </summary>
		public BurntSprite(ISprite sprite)
			: base(Game.Theme.DrawableFactory.Create("burnt"))
		{
			// Set our frame to 4
			state.Frame = 4;

			// Copy our coordinates
			X = sprite.X;
			Y = sprite.Y;
			Height = sprite.Height;
			Width = sprite.Width;
		}

		/// <summary>
		/// This is true if the sprite can be removed.
		/// </summary>
		public bool CanRemove
		{
			get { return state.Frame == 8; }
		}
	}
}