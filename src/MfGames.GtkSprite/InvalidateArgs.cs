using Gdk;
using System;

namespace MfGames.Sprite
{
	/// <summary>
	/// The InvalidateArgs are used to contain the changes made to a
	/// sprite. All of the arguments are given in sprite space, not
	/// widget space.
	/// </summary>
	public class InvalidateArgs
	: EventArgs
	{
		public Rectangle Rectangle;
	}

	/// <summary>
	/// Defines the basic delegate for handling the invalidate
	/// arguments.
	/// </summary>
	public delegate void InvalidateHandler(object sender, InvalidateArgs args);
}
