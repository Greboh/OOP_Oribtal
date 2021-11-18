using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Orbital.PowerUps;

namespace Orbital
{
	class Spawner : GameObject
	{
		#region Fields

		private float totalTimeElapsed; // Tracks the total amount of game time 
		private float timeSinceLastAsteroid; // Tracks the time between the last asteroid and now
        private float timeSinceLastPower; // Tracks the time between the last power and now
        private float asteroidTimer = 1.5f; // How fast asteroids spawn
		private float powerTimer = 1; // How fast powers spawn

		private int changeAsteroidDifficultyTimer = 2; // The interval in which to increase the difficult for the asteroids
		private int changePowerDifficultyTimer = 16; // The interval in which to increase the difficult for the powers
		private int scoreThreshold = 1000; // start score amount needed to spawn enemy
		private int scoreThresholdMultiplier = 1000; // amount added onto scoreThreshold whenever a enemy dies
		private int waveNumber = 1; // keeps track of how many waves there have been

		#endregion

		#region Methods

		public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public override void Update(GameTime gameTime)
		{
			SpawnAsteroid(gameTime);
			SpawnPower(gameTime);
			SpawnEnemyShip(gameTime);
		}
        
		public override void OnCollision(GameObject obj)
        {
        }
		public override void Attack(GameTime gameTime)
		{
		}

		/// <summary>
		/// Spawns the enemy ship
		/// </summary>
		/// <param name="gameTime"></param>
		public void SpawnEnemyShip(GameTime gameTime)
        {
	        // if the score amount reached is above the threshold and the amount of enemies onscreen is lower than the total amount of enemies.
			if (GameWorld.Score > scoreThreshold && amountOfEnemies < waveNumber)
			{
				Instantiate(new Enemy()); // spawns enemy
				amountOfEnemies++; // keeps track of how many enemies is on screen
				waveNumber++; // adds to total amount of enemies spawned
				scoreThreshold += scoreThresholdMultiplier; // adds the multiplier onto the next score threshold
			}
		}

		/// <summary>
		/// Spawns asteroids 
		/// </summary>
		/// <param name="gameTime"></param>
		public void SpawnAsteroid(GameTime gameTime)
        {
	        int randomAsteroidPos = myRandom.Next(1,4); //used to determine which asteroid to spawn

	        totalTimeElapsed = (float) gameTime.TotalGameTime.TotalSeconds;

	        timeSinceLastAsteroid += (float) gameTime.ElapsedGameTime.TotalSeconds;

			// If the total time is bigger than the AsteroidDifficultyTimer
			if (totalTimeElapsed > changeAsteroidDifficultyTimer)
            {
	            changeAsteroidDifficultyTimer += 10; // Add 10 seconds to the AsteroidDifficultyTimer

				// Makes sure the asteroids can maximum spawn every half second
				if (asteroidTimer != 0.5f)
				{
					asteroidTimer -= 0.1f;
				}
            }

			// If our timer that checks the time since last asteroid was spawned
			// is smaller than how fast each asteroid can spawn
            if (timeSinceLastAsteroid >= asteroidTimer)
            {
				List<Asteroid> asteroids = new List<Asteroid>();//list used to instantiate asteroids
				Asteroid yAsteroid = new Asteroid(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y))); //asteroid spawns from the left side of the screen
				Asteroid xyAsteroid = new Asteroid(new Vector2((int)GameWorld.ScreenSize.X, myRandom.Next(0, (int)GameWorld.ScreenSize.Y))); //asteroid spawns from the right side of the screen
				Asteroid xAsteroid = new Asteroid(new Vector2(myRandom.Next(0, (int)GameWorld.ScreenSize.X), 0)); //asteroid spawns from the top of the screen

				//add the three asteroids to the list
				asteroids.Add(xAsteroid);
				asteroids.Add(yAsteroid);
				asteroids.Add(xyAsteroid);

				//Spawns asteroid based on randomAsteroidPos random number.
				if (randomAsteroidPos == 1) 
				{
                    Instantiate(asteroids[0]);
				}
                else if (randomAsteroidPos == 2)
				{
					Instantiate(asteroids[1]);
				}
                else Instantiate(asteroids[2]);

				timeSinceLastAsteroid = 0; // Resets the time between last asteroid spawn

            }
        }

		/// <summary>
		/// Spawns PowerUps
		/// </summary>
		/// <param name="gameTime"></param>
		private void SpawnPower(GameTime gameTime)
		{
			int randomPowerPicker = myRandom.Next(1, 4); // Used to pick the which power to spawn
			int randomPowerPos = myRandom.Next(1, 3); // Used to choose position of the picked power

			timeSinceLastPower += (float)gameTime.ElapsedGameTime.TotalSeconds;

			totalTimeElapsed = (float)gameTime.TotalGameTime.TotalSeconds;

			List<HealthPower> healthPowers = new List<HealthPower>(); // List of all HealthPowers
			List<SpeedPower> speedPowers = new List<SpeedPower>(); // List of all SpeedPowers
			List<RateOfFirePower> rateOfFirePowers = new List<RateOfFirePower>(); // List of all RateOfFirePowers

			// If the total time is bigger than the PowerDifficultyTime
			if (totalTimeElapsed > changePowerDifficultyTimer)
			{
				changePowerDifficultyTimer += 10; // Add 10 seconds to the PowerDifficultyTime

				// Makes sure the powers can maximum spawn every 30 second
				if (powerTimer != 30f)
				{
					powerTimer += 2.5f;
				}
			}

			// If the total time is bigger than the AsteroidDifficultyTimer
			if (timeSinceLastPower >= powerTimer)
			{
				if (randomPowerPicker == 1)
				{
					HealthPower xPower = new HealthPower(new Vector2(myRandom.Next(0, (int)GameWorld.ScreenSize.X), 0)); // Powers spawns from the left side of the screen
					HealthPower yPower = new HealthPower(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y))); // Powers spawns from the right side of the screen
					HealthPower xyPower = new HealthPower(new Vector2((int)GameWorld.ScreenSize.X, myRandom.Next(0, (int)GameWorld.ScreenSize.Y))); //Powers spawns from the top of the screen
					
					// Adds the three powers to the list
					healthPowers.Add(xPower);
					healthPowers.Add(yPower);
					healthPowers.Add(xyPower);

					//Spawns Power based on randomPowerPicker random number.
					if (randomPowerPos == 1)
					{
						Instantiate(healthPowers[0]);
					}
					else if (randomPowerPos == 2)
					{
						Instantiate(healthPowers[1]);
					}
					else Instantiate(healthPowers[2]);

				}
				else if (randomPowerPicker == 2) // Speed Power
				{
					SpeedPower xPower = new SpeedPower(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));
					SpeedPower yPower = new SpeedPower(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));
					SpeedPower xyPower = new SpeedPower(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));

					speedPowers.Add(xPower);
					speedPowers.Add(yPower);
					speedPowers.Add(xyPower);

					if (randomPowerPos == 1)
					{
						Instantiate(speedPowers[0]);
					}
					else if (randomPowerPos == 2)
					{
						Instantiate(speedPowers[1]);
					}
					else Instantiate(speedPowers[2]);

				}
				else if (randomPowerPicker == 3) // Rate of fire Power
				{
					RateOfFirePower xPower = new RateOfFirePower(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));
					RateOfFirePower yPower = new RateOfFirePower(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));
					RateOfFirePower xyPower = new RateOfFirePower(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));

					rateOfFirePowers.Add(xPower);
					rateOfFirePowers.Add(yPower);
					rateOfFirePowers.Add(xyPower);

					if (randomPowerPos == 1)
					{
						Instantiate(rateOfFirePowers[0]);
					}
					else if (randomPowerPos == 2)
					{
						Instantiate(rateOfFirePowers[1]);
					}
					else Instantiate(rateOfFirePowers[2]);
				}
				timeSinceLastPower = 0; // Resets the time between last asteroid
			}

		}

		#endregion
	}
}
