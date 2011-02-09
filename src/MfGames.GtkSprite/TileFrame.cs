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

namespace MfGames.Sprite
{
	/// <summary>
	/// A tile frame is a single frame (image) inside a tile. It has a
	/// small amount of controls, including timing functions and which
	/// frame is next control.
	/// </summary>
	public class TileFrame
	{
		private int delay;
		private int nextFrame;
		private bool random = true;
		private Tile tile;

		/// <summary>
		/// Constructs the tile frame with a tile for the default
		/// values.
		/// </summary>
		public TileFrame(Tile tile)
		{
			this.tile = tile;
		}

		/// <summary>
		/// Contains the delay in milliseconds that the frame is to be
		/// shown. If this is zero or less, it uses
		/// the delay of the tile.
		/// </summary>
		public int Delay
		{
			get
			{
				if (delay <= 0)
				{
					return tile.Delay;
				}
				return delay;
			}
			set { delay = value; }
		}

		/// <summary>
		/// Determines the next frame for this tile. As a default, the
		/// tile assigns this to the next frame inside tile.
		/// </summary>
		public int NextFrame
		{
			get { return nextFrame; }
			set { nextFrame = value; }
		}

		/// <summary>
		/// Is true if this frame can be selected as a random frame.
		/// </summary>
		public bool Random
		{
			get { return random; }
			set { random = value; }
		}
	}
}