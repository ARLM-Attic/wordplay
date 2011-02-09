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

using System.IO;

using C5;

#endregion

namespace MfGames.Sprite
{
	/// <summary>
	/// This class creates and loads various pixbufs into memory. This
	/// is based on a key, which is basically a site-relative key,
	/// excluding the file extension and using / instead of the
	/// system-specific paths.
	///
	/// To find images, search paths are registered using the
	/// SearchPaths property (which is a list of DirectoryInfo
	/// objects). The paths are searched, in order, until a file that
	/// contains the key, plus a valid extension. Once found, it
	/// returns a drawable for that image.
	///
	/// This factory only creates static drawables.
	/// </summary>
	public class PixbufDrawableFactory
	{
		#region Creation

		/// <summary>
		/// Constructs a drawable object using the given key.
		/// </summary>
		public IDrawable Create(string key)
		{
			// Adjust the internal fields. This creates a "/key" path
			// appropriate for the current operating system.
			string p = Path.DirectorySeparatorChar +
			           key.Replace('/', Path.DirectorySeparatorChar);

			// Collect a list files that have an appropriate name
			foreach (DirectoryInfo di in searchPaths)
			{
				// Create a full partial path (excluding the stuff
				// after the key).
				string fp = di.FullName + p;

				// We get the directory, which may or not be the
				// original di one (since a/b/c would be the a/b
				// directory inside it. If this directory doesn't
				// exist, then just move to the next search path.
				string dn = Path.GetDirectoryName(fp);
				DirectoryInfo d = new DirectoryInfo(dn);

				if (!d.Exists)
				{
					continue;
				}

				// Collect files that start with the key ("key.*") as
				// possible images.
				FileInfo[] fis = d.GetFiles(key + ".*");

				if (fis.Length == 0)
				{
					continue;
				}

				// Try to create a drawable
				IDrawable drawable = CreateDrawable(key, fis);

				if (drawable != null)
				{
					return drawable;
				}
			}

			// Cannot create the pixbuf
			throw new SpriteException("Cannot create pixbuf: " + key);
		}

		/// <summary>
		/// Internal function that attempts to load a drawable of some
		/// manner from the given filename.
		/// </summary>
		private IDrawable CreateDrawable(
			string key,
			FileInfo[] fis)
		{
			// We basically have a single possible pattern, which is
			// "key.*" where "*" is a valid extension ("jpg", "png",
			// "gif", "svg").
			foreach (FileInfo fi in fis)
			{
				// Check for ending
				string suffix = fi.Name.Substring(key.Length).ToLower();

				if (suffix == ".svg")
				{
					// Create a SVG drawable (this actually isn't a
					// pixbuf)
					return new RsvgDrawable(fi);
				}
				else if (suffix == ".png" || suffix == ".jpg" || suffix == ".gif")
				{
					// Create a simple pixbuf drawable
					return new PixbufDrawable(fi);
				}
			}

			// We didn't find anything
			return null;
		}

		#endregion

		#region Properties

		private readonly LinkedList<DirectoryInfo> searchPaths =
			new LinkedList<DirectoryInfo>();

		/// <summary>
		/// Contains a list of directories to search for paths.
		/// </summary>
		public LinkedList<DirectoryInfo> SearchPaths
		{
			get { return searchPaths; }
		}

		#endregion
	}
}