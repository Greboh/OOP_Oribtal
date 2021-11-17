using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Orbital
{
    class ScoreManager
    {

        public static void UpdateScore(int point)
        {
            GameWorld.Score += point;
        }

    }
}
