using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Orbital.PowerUps;
using SharpDX.MediaFoundation;


namespace Orbital
{
	class Spawner : GameObject
	{
		private float totalTimeElapsed;
		private float timeSinceLastAsteroid;
        private float timeSinceLastPower;

		private float asteroidTimer = 1.5f; // How fast asteroids spawn
		private float powerTimer = 5; // How fast powers spawn

		private int changeAsteroidDifficultyTimer = 2;
		private int changePowerDifficultyTimer = 16;

	
		private int scoreThreshold = 500;				// start score amount needed to spawn enemy
		private int scoreThresholdMultiplier = 500;		// amount added onto scoreThreshold whenever a enemy dies
		private int waveNumber = 1;						// keeps track of how many waves there have been


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
            
           


            
				

			
		

		public void SpawnEnemyShip(GameTime gameTime)
        {
			
			Enemy spawnedEnemy = new Enemy();
			
			// if the score amount reached is above the threshold and the amount of enemies onscreen is lower than the total amount of enemies.
			if (GameWorld.Score > scoreThreshold && amountOfEnemies < waveNumber)
			{
				Instantiate(spawnedEnemy);					// spawns enemy
				amountOfEnemies++;							// keeps track of how many enemies is on screen
				waveNumber++;								// adds to total amount of enemies spawned
				scoreThreshold += scoreThresholdMultiplier; // adds the multiplier onto the next score threshold

			}
			
		}



		public void SpawnAsteroid(GameTime gameTime)
        {
	        int randomAsteroidPos = myRandom.Next(1,4);

	        totalTimeElapsed = (float) gameTime.TotalGameTime.TotalSeconds;

	        timeSinceLastAsteroid += (float) gameTime.ElapsedGameTime.TotalSeconds;

            // [IDEA]
            // Replace with if statement that decides in which of the four sides the asteroid should spawn using random.Next

            // Maybe solved now on line 56 - 64
            // Now it only spawns 1 asteroid at least

            if (totalTimeElapsed > changeAsteroidDifficultyTimer)
            {
	            changeAsteroidDifficultyTimer += 10;
				Console.WriteLine("Timer for asteroid is: " + asteroidTimer);

				if (asteroidTimer != 0.5f)
				{
					asteroidTimer -= 0.1f;
				}
            }


            if (timeSinceLastAsteroid >= asteroidTimer)
            {
				
				List<Asteroid> asteroids = new List<Asteroid>();
				Asteroid yAsteroid = new Asteroid(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));
				Asteroid xyAsteroid = new Asteroid(new Vector2((int)GameWorld.ScreenSize.X, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));
				Asteroid xAsteroid = new Asteroid(new Vector2(myRandom.Next(0, (int)GameWorld.ScreenSize.X), 0));

				asteroids.Add(xAsteroid);
				asteroids.Add(yAsteroid);
				asteroids.Add(xyAsteroid);

				if (randomAsteroidPos == 1) 
				{
                    Instantiate(asteroids[0]);
				}
                else if (randomAsteroidPos == 2)
				{
					Instantiate(asteroids[1]);
				}
                else Instantiate(asteroids[2]);


				//foreach (Asteroid obj in asteroids)
				//{
				//	Instantiate(obj);
				//}

				timeSinceLastAsteroid = 0;

            }

            

		}

		private void SpawnPower(GameTime gameTime)
		{
			//TODO Looks like shit, needs fixing for sure!
			timeSinceLastPower += (float)gameTime.ElapsedGameTime.TotalSeconds;

			totalTimeElapsed = (float)gameTime.TotalGameTime.TotalSeconds;

			List<HealthPower> healthPowers = new List<HealthPower>();
			List<SpeedPower> speedPowers = new List<SpeedPower>();
			List<RateOfFirePower> rateOfFirePowers = new List<RateOfFirePower>();


			if (totalTimeElapsed > changePowerDifficultyTimer)
			{
				Console.WriteLine("Timer for power is: " + powerTimer);
				changePowerDifficultyTimer += 10;

				if (powerTimer != 30f)
				{
					powerTimer += 2.5f;
				}
			}


			int randomPowerPicker = myRandom.Next(1, 4); // Used to pick the which power to spawn
			int randomPowerPos = myRandom.Next(1, 3); // Used to choose position of the picked power

			if (timeSinceLastPower >= powerTimer)
			{
				if (randomPowerPicker == 1) // HealthPower
				{

					HealthPower xPower = new HealthPower(new Vector2(myRandom.Next(0, (int)GameWorld.ScreenSize.X), 0));
					HealthPower yPower = new HealthPower(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));
					HealthPower xyPower = new HealthPower(new Vector2((int)GameWorld.ScreenSize.X, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));

					healthPowers.Add(xPower);
					healthPowers.Add(yPower);
					healthPowers.Add(xyPower);



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
				timeSinceLastPower = 0;
			}

		}


		public override void OnCollision(GameObject obj)
        {
            
        }
		public override void Attack(GameTime gameTime)
		{
		}

		private void OnDeath()
		{
			if (myGameWorld.currentGameState == Gamestate.DeathScreen)
			{
				Destroy(this);
			}
		}
	}
}
