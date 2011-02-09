using System;

using Gdk;

/// <summary>
/// This encapsulates the basic and common functionality of a sprite
/// pane, which is used to show the various aspects of the sprite
/// library.
/// </summary>
public abstract class DemoSpritePane
{
	private static Random random = new Random();

	protected static Random Entropy { get { return random; } }
	
	/// <summary>
	/// Contains the name of the pane (for the drop-down list).
	/// </summary>
	public abstract string Name { get; }

	/// <summary>
	/// Called when the size of the widget is changed.
	/// </summary>
	public virtual void Configure(int width, int height)
	{
	}

	/// <summary>
	/// Called when the pane needs to be rendered.
	/// </summary>
	public virtual void Render(Drawable drawable, Rectangle region)
	{
	}

	/// <summary>
	/// Called when the pane needs to be updated in some manner.
	/// </summary>
	public virtual void Update(Gtk.Widget widget)
	{
	}
}
