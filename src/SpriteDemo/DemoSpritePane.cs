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

using Gdk;

using Gtk;

#endregion

/// <summary>
/// This encapsulates the basic and common functionality of a sprite
/// pane, which is used to show the various aspects of the sprite
/// library.
/// </summary>
public abstract class DemoSpritePane
{
	private static readonly Random random = new Random();

	protected static Random Entropy
	{
		get { return random; }
	}

	/// <summary>
	/// Contains the name of the pane (for the drop-down list).
	/// </summary>
	public abstract string Name { get; }

	/// <summary>
	/// Called when the size of the widget is changed.
	/// </summary>
	public virtual void Configure(
		int width,
		int height)
	{
	}

	/// <summary>
	/// Called when the pane needs to be rendered.
	/// </summary>
	public virtual void Render(
		Drawable drawable,
		Rectangle region)
	{
	}

	/// <summary>
	/// Called when the pane needs to be updated in some manner.
	/// </summary>
	public virtual void Update(Widget widget)
	{
	}
}