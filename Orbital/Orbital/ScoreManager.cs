using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Orbital
{
     class ScoreManager
    {
	    #region  Fields

	    private static List<int> listOfScores = new List<int>(); // Creates a list to store the highscore 
	    public static string filePath = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName; // Finds the folder where the assembly is located ie gamefolder
	    private static string fileName = Path.Combine(filePath, "OrbitalHighscores.txt"); // Combines filepath and the string "OribitalHighscore.txt"

		#endregion

		#region Properties

		/// <summary>
		/// Property for listOfScores
		/// </summary>
		public static List<int> ListOfScores
	    {
		    get => listOfScores;
		    set => listOfScores = value;
	    }

		#endregion      

		#region Methods

		/// <summary>
		/// Updates the score
		/// </summary>
		/// <param name="point">The value in which to add to the score</param>
		public static void UpdateScore(int point)
		{
			GameWorld.Score += point;
		}

		/// <summary>
		/// Saves the score
		/// </summary>
		/// <param name="score">The value in which save</param>
		public static void SaveScore(int score)
		{
			ListOfScores.Add(score);
		}

		/// <summary>
		/// Saves the highest score to a txt file
		/// </summary>
		public static void SaveToTxt()
		{
			// If the list is not 0
			if (listOfScores.Count != 0)
			{
				string createHighscoreTxt = listOfScores.Max().ToString(); // creates a string which is equal to the highest value in our list and then converts the number to a string

				// if the txt storing the highscore exists
				if (!File.Exists(fileName))
				{
					File.WriteAllText(fileName, createHighscoreTxt); // Write all text to the file in the path
				}
				// If the txt does exist
				else if (File.Exists(fileName))
				{
					File.Delete(fileName); // Delete it to ensure we only have 1 txt file with 1 line in it
					File.WriteAllText(fileName, createHighscoreTxt); // Write all text to the file in the path
				}
			}


		}

		/// <summary>
		/// Reads the text file we created in SaveToTxt()
		/// </summary>
		/// <returns>Returns an int which is our highscore</returns>
		public static int ReadTxt()
		{
			int highScore; // used to store the value we want to return

			if (!File.Exists(fileName))
			{
				highScore = GameWorld.Score; // If the file doesn't exist we know the highscore is 0 therefor we set highscore to our current score which is 0
			}
			else
			{
				string readHighscoreTxt = File.ReadAllText(fileName); // Reads the txt file
				highScore = Convert.ToInt32(readHighscoreTxt); // Converts the line in the txt file from string to int

			}
			return highScore; // Returns highscore
		}

		#endregion

	}
}
