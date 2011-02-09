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
using System.IO;
using System.Xml;

using C5;

using NetSpell.SpellChecker.Dictionary;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// Encapsulates the entire functionality of a language.
	/// </summary>
	public class Language
	{
		private readonly WeightedSelector frequencies = new WeightedSelector();

		private readonly HashDictionary<char, int> scores =
			new HashDictionary<char, int>();

		private WordDictionary dictionary;

		/// <summary>
		/// Loads a language into memory.
		/// </summary>
		public Language(string name)
		{
			LoadLanguage(name);
		}

		/// <summary>
		/// Contains the directory of the dictionaries.
		/// </summary>
		public static string DictionaryDirectory
		{
			get { return Path.Combine(Game.BaseDirectory, "dicts"); }
		}

		/// <summary>
		/// Creates a new value for the token.
		/// </summary>
		public char CreateValue()
		{
			char c = (char) frequencies.RandomObject;

			if (frequencies[c] > 1)
			{
				frequencies[c]--;
			}

			return c;
		}

		/// <summary>
		/// Returns the score value of a given character.
		/// </summary>
		public int GetScore(char value)
		{
			try
			{
				return scores[value];
			}
			catch
			{
				return 0;
			}
		}

		/// <summary>
		/// Checks to see if the given word is valid.
		/// </summary>
		public bool IsValidWord(string word)
		{
			return dictionary.Contains(word.ToLower());
		}

		/// <summary>
		/// Loads a language into memory.
		/// </summary>
		private void LoadLanguage(string name)
		{
			try
			{
				// Load the dictionary into memory
				dictionary = new WordDictionary();
				dictionary.DictionaryFolder = DictionaryDirectory;
				dictionary.DictionaryFile = name + ".dic";
				dictionary.Initialize();

				// Load the frequencies and scores into memory
				FileInfo file =
					new FileInfo(Path.Combine(DictionaryDirectory, name + ".xml"));

				if (!file.Exists)
				{
					throw new WordplayException("Cannot find freq file: " + file);
				}

				TextReader tr = file.OpenText();
				XmlTextReader xtr = new XmlTextReader(tr);

				while (xtr.Read())
				{
					if (xtr.LocalName == "letter")
					{
						char value = xtr["value"][0];
						int freq = Int32.Parse(xtr["freq"]);
						int score = Int32.Parse(xtr["score"]);
						Register(value, freq, score);
					}
				}

				xtr.Close();
				tr.Close();
			}
			catch (Exception e)
			{
				// Make an error message
				Error("Cannot load language: {0}", name);

				// Try to load it
				if (name != "en-US")
				{
					LoadLanguage("en-US");
				}
				else
				{
					throw e;
				}
			}
		}

		/// <summary>
		/// Registers a letter in memory.
		/// </summary>
		private void Register(
			char value,
			int freq,
			int score)
		{
			frequencies[value] = freq;
			scores[value] = score;
		}
	}
}