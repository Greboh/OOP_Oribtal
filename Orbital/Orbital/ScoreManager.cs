using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Orbital
{
    static class ScoreManager
    {
	    private static List<int> listOfScores = new List<int>();

	    public static List<int> ListOfScores
	    {
		    get => listOfScores;
		    set => listOfScores = value;
	    }


	    public static void UpdateScore(int point)
        {
            GameWorld.Score += point;
        }

        public static void SaveScore(int point)
        {
            ListOfScores.Add(GameWorld.Score);
        }

    }
}
