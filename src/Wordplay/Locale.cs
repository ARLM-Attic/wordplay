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

using System.Resources;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// Encapsulates the basic functionality of the translation and
	/// globalization elements of the them.
	/// </summary>
	public class Locale
	{
		private static ResourceManager manager;

		/// <summary>
		/// Contains the resource manager for this locale.
		/// </summary>
		public static ResourceManager Resources
		{
			get
			{
				if (manager == null)
				{
					manager = new ResourceManager("locale", typeof(Locale).Assembly);
				}

				return manager;
			}
		}

		/// <summary>
		/// Attempts to translate the given text and return the
		/// results. If it cannot be found, it marks a small error
		/// then returns the key.
		/// </summary>
		public static string Translate(string key)
		{
			return key;
			/*
			string value = Resources.GetString(key);

			if (value == null)
			{
				return key;
			}
			else
			{
				return value;
			}
			*/
		}
	}
}