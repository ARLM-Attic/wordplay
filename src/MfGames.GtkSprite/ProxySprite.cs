using Gdk;
using MfGames.Utility;

namespace MfGames.Sprite
{
	/// <summary>
	/// This creates a proxy interface with another sprite. This
	/// allows extending classes to add functionality to a sprite
	/// without changing those classes.
	/// </summary>
	public class ProxySprite
	: Logable, ISprite
	{
		protected ISprite sprite;

		/// <summary>
		/// Constructs the proxy sprite with another sprite.
		/// </summary>
		public ProxySprite(ISprite sprite)
		{
			this.sprite = sprite;
			this.sprite.Invalidate += OnInvalidate;
		}

		/// <summary>
		/// Compares a sprite to another sprite.
		/// </summary>
		public virtual int CompareTo(ISprite sprite)
		{
			if (Z != sprite.Z)
				return Z.CompareTo(sprite.Z);

			if (X != sprite.X)
				return X.CompareTo(sprite.X);
			
			if (Y != sprite.Y)
				return Y.CompareTo(sprite.Y);

			return 0;
		}

		/// <summary>
		/// This method is called when the sprite is told to draw
		/// itself on the window.
		/// </summary>
		public virtual void Render(Drawable dest, Gdk.GC gc)
		{
			sprite.Render(dest, gc);
		}

		/// <summary>
		/// This method is used to update the sprite's capable of
		/// updating themselves.
		/// </summary>
		public virtual void Update()
		{
			sprite.Update();
		}

#region Properties
		/// <summary>
		/// Contains the settable height of the sprite.
		/// </summary>
		public virtual int Height
		{
			get { return sprite.Height; }
			set { sprite.Height = value; }
		}

		/// <summary>
		/// Contains the proxied sprite.
		/// </summary>
		public virtual ISprite ProxiedSprite
		{
			get { return sprite; }
			set
			{
				if (value == null)
					throw new SpriteException("Cannot assign a null sprite");

				sprite = value;
			}
		}

		/// <summary>
		/// Contains the settable width of the sprite.
		/// </summary>
		public virtual int Width
		{
			get { return sprite.Width; }
			set { sprite.Width = value; }
		}

		/// <summary>
		/// Contains the X coordinate of the sprite.
		/// </summary>
		public virtual int X
		{
			get { return sprite.X; }
			set { sprite.X = value; }
		}

		/// <summary>
		/// Contains the Y coordinate of the sprite.
		/// </summary>
		public virtual int Y
		{
			get { return sprite.Y; }
			set { sprite.Y = value; }
		}

		/// <summary>
		/// Contains the Z coordinate of the sprite.
		/// </summary>
		public virtual int Z
		{
			get { return sprite.Z; }
			set { sprite.Z = value; }
		}
#endregion

#region Events
		/// <summary>
		/// This event is used when the sprite invalidated or changes
		/// the appearance of the sprite, or moves within sprite
		/// space.
		/// </summary>
		public event InvalidateHandler Invalidate;

		/// <summary>
		/// Fires an invalidate event to all of the listeners.
		/// </summary>
		public void FireInvalidate()
		{
			// Ignore if we don't have listeners
			if (Invalidate == null)
				return;

			// Create the rectangle
			InvalidateArgs args = new InvalidateArgs();
			args.Rectangle = new Rectangle(X, Y, Width, Height);

			// Fire it
			Invalidate(this, args);
		}

		/// <summary>
		/// Triggered when the internal sprite triggers it.
		/// </summary>
		protected virtual void OnInvalidate(
			object sender, InvalidateArgs args)
		{
			if (Invalidate != null)
				Invalidate(this, args);
		}
#endregion
	}
}
