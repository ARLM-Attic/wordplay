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

#endregion

namespace MfGames.Sprite
{
	/// <summary>
	/// A sprite that has a desired location that it moves toward at a
	/// given rate.
	/// </summary>
	public class MovingSprite : ProxySprite
	{
		private long lastUpdateX;
		private long lastUpdateY;

		/// <summary>
		/// Constructs the moving sprite with another sprite.
		/// </summary>
		public MovingSprite(ISprite sprite)
			: base(sprite)
		{
		}

		/// <summary>
		/// This method is used to update the sprite's capable of
		/// updating themselves.
		/// </summary>
		public override void Update()
		{
			// Check to see if we are moving
			long now = DateTime.UtcNow.Ticks;

			if (IsMoving)
			{
				// Invalidate the "before"
				FireInvalidate();

				// Figure out the time that passed
				double diffX = ((double) (now - lastUpdateX)) / 10000000;
				double diffY = ((double) (now - lastUpdateY)) / 10000000;
				int cx = (int) (rx * diffX);
				int cy = (int) (ry * diffY);

				// Move the X rate
				if (dx != X && cx > 0)
				{
					if (dx < X)
					{
						X -= cx;
						if (dx > X)
						{
							X = dx;
						}
					}

					if (dx > X)
					{
						X += cx;
						if (dx < X)
						{
							X = dx;
						}
					}

					// Save our update
					lastUpdateX = now;
				}

				// Move the Y rate
				if (dy != Y && cy > 0)
				{
					if (dy < Y)
					{
						Y -= cy;
						if (dy > Y)
						{
							Y = dy;
						}
					}

					if (dy > Y)
					{
						Y += cy;
						if (dy < Y)
						{
							Y = dy;
						}
					}

					// Save our update
					lastUpdateY = now;
				}

				// Invalidate the after
				FireInvalidate();

				// See if we stopped moving
				if (!IsMoving)
				{
					FireStoppedMoving();
				}
			}

			// Call the parent
			base.Update();
		}

		#region Properties

		private int dx, dy;
		private double rx, ry;

		/// <summary>
		/// Contains the desired X-coordinate for this sprite.
		/// </summary>
		public int DesiredX
		{
			get { return dx; }
			set
			{
				dx = value;
				lastUpdateX = DateTime.UtcNow.Ticks;
			}
		}

		/// <summary>
		/// Contains the desired Y-coordinate for this sprite.
		/// </summary>
		public int DesiredY
		{
			get { return dy; }
			set
			{
				dy = value;
				lastUpdateY = DateTime.UtcNow.Ticks;
			}
		}

		/// <summary>
		/// This is true when the sprite is not at its desired
		/// location.
		/// </summary>
		public bool IsMoving
		{
			get { return dx != X || dy != Y; }
		}

		/// <summary>
		/// Contains the rate of the sprite, as pixels per second, for
		/// the X-axis.
		/// </summary>
		public double RateX
		{
			get { return rx; }
			set { rx = value; }
		}

		/// <summary>
		/// Contains the rate of the sprite, as pixels per second, for
		/// the Y-axis.
		/// </summary>
		public double RateY
		{
			get { return ry; }
			set { ry = value; }
		}

		#endregion

		#region Events

		/// <summary>
		/// Fires a "not moving" event.
		/// </summary>
		private void FireStoppedMoving()
		{
			if (StoppedMoving != null)
			{
				StoppedMoving(this, new EventArgs());
			}
		}

		public event EventHandler StoppedMoving;

		#endregion
	}
}