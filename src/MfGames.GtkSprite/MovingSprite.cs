using System;

namespace MfGames.Sprite
{
	/// <summary>
	/// A sprite that has a desired location that it moves toward at a
	/// given rate.
	/// </summary>
	public class MovingSprite
	: ProxySprite
	{
		private long lastUpdateX = 0;
		private long lastUpdateY = 0;

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
						if (dx > X) X = dx;
					}

					if (dx > X)
					{
						X += cx;
						if (dx < X) X = dx;
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
						if (dy > Y) Y = dy;
					}

					if (dy > Y)
					{
						Y += cy;
						if (dy < Y) Y = dy;
					}

					// Save our update
					lastUpdateY = now;
				}

				// Invalidate the after
				FireInvalidate();

				// See if we stopped moving
				if (!IsMoving)
					FireStoppedMoving();
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
			set { dx = value; lastUpdateX = DateTime.UtcNow.Ticks; }
		}

		/// <summary>
		/// Contains the desired Y-coordinate for this sprite.
		/// </summary>
		public int DesiredY
		{
			get { return dy; }
			set { dy = value; lastUpdateY = DateTime.UtcNow.Ticks; }
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
		public event EventHandler StoppedMoving;

		/// <summary>
		/// Fires a "not moving" event.
		/// </summary>
		private void FireStoppedMoving()
		{
			if (StoppedMoving != null)
				StoppedMoving(this, new EventArgs());
		}
#endregion
	}
}
