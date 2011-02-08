using C5;
using MfGames.Utility;
using NetSpell.SpellChecker.Dictionary;
using System;
using System.IO;
using System.Xml;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Encapsulates the entire functionality of a language.
	/// </summary>
	public class Language
	: Logable
	{
		private WeightedSelector frequencies = new WeightedSelector();
		private HashDictionary <char, int> scores =
			new HashDictionary <char, int> ();
		private WordDictionary dictionary;

		/// <summary>
		/// Contains the directory of the dictionaries.
		/// </summary>
		public static string DictionaryDirectory
		{
			get { return Path.Combine(Game.BaseDirectory, "dicts"); }
		}

		/// <summary>
		/// Loads a language into memory.
		/// </summary>
		public Language(string name)
		{
			LoadLanguage(name);
		}

		/// <summary>
		/// Creates a new value for the token.
		/// </summary>
		public char CreateValue()
		{
			char c = (char) frequencies.RandomObject;

			if (frequencies[c] > 1)
				frequencies[c]--;

			return c;
		}

		/// <summary>
		/// Returns the score value of a given character.
		/// </summary>
		public int GetScore(char value)
		{
			try { return scores[value]; } catch { return 0; }
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
					new FileInfo(Path.Combine(DictionaryDirectory,
							name + ".xml"));
				
				if (!file.Exists)
					throw new WordplayException(
						"Cannot find freq file: " + file);
				
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
		private void Register(char value, int freq, int score)
		{
			frequencies[value] = freq;
			scores[value] = score;
		}
	}
}
