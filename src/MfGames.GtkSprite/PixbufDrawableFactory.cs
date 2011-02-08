using C5;
using MfGames.Utility;
using System.IO;

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
	: Logable
	{
#region Creation
		/// <summary>
		/// Constructs a drawable object using the given key.
		/// </summary>
		public IDrawable Create(string key)
		{
			// Adjust the internal fields. This creates a "/key" path
			// appropriate for the current operating system.
			string p = Path.DirectorySeparatorChar
				+ key.Replace('/', Path.DirectorySeparatorChar);

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
					continue;

				// Collect files that start with the key ("key.*") as
				// possible images.
				FileInfo [] fis = d.GetFiles(key + ".*");

				if (fis.Length == 0)
					continue;

				// Try to create a drawable
				IDrawable drawable = CreateDrawable(key, fis);

				if (drawable != null)
					return drawable;
			}

			// Cannot create the pixbuf
			throw new SpriteException("Cannot create pixbuf: " + key);
		}

		/// <summary>
		/// Internal function that attempts to load a drawable of some
		/// manner from the given filename.
		/// </summary>
		private IDrawable CreateDrawable(string key, FileInfo [] fis)
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
				else if (suffix == ".png" ||
					suffix == ".jpg" ||
					suffix == ".gif")
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
		private LinkedList <DirectoryInfo> searchPaths =
			new LinkedList <DirectoryInfo> ();

		/// <summary>
		/// Contains a list of directories to search for paths.
		/// </summary>
		public LinkedList <DirectoryInfo> SearchPaths
		{
			get { return searchPaths; }
		}
#endregion
	}
}
