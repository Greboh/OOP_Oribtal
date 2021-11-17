﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Orbital
{
     class ScoreManager
    {
	    private static List<int> listOfScores = new List<int>();
	    private static string filePath = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName; // return the application.exe current folder
	    private static string fileName = Path.Combine(filePath, "OrbitalHighscores.txt");

	    // Properties
		public static List<int> ListOfScores
	    {
		    get => listOfScores;
		    set => listOfScores = value;
	    }

        //Methods
        public static void UpdateScore(int point)
        {
            GameWorld.Score += point;

        }

        public static void SaveScore(int point)
        {
            ListOfScores.Add(point);
        }

        public static void SaveToTxt()
        {
	        if (listOfScores.Count != 0)
	        {
				string createHighscoreTxt = listOfScores.Max().ToString();

				if (!File.Exists(fileName))
				{
					File.WriteAllText(fileName, createHighscoreTxt);
				}
				else if (File.Exists(fileName))
				{
					File.Delete(fileName);
					File.WriteAllText(fileName, createHighscoreTxt);
				}
			}

        }

        public static int ReadTxt()
        {
	        string createPlaceholderHighScoreTxt = "0";
	        int highScore = GameWorld.Score;

	        if (!File.Exists(fileName))
	        {
		        File.WriteAllText(fileName, createPlaceholderHighScoreTxt);
				highScore = Convert.ToInt32(createPlaceholderHighScoreTxt);

			}
	        else
	        {
				string readHighscoreTxt = File.ReadAllText(fileName);
				highScore = Convert.ToInt32(readHighscoreTxt);

	        }
			Console.WriteLine(highScore);
			return highScore;
        }

    }
}
