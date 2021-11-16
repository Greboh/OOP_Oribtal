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
		private float timeElapsedEnemy;

		private float asteroidTimer = 1.5f;
		private float powerTimer = 30;

		private int changeAsteroidDifficultyTimer = 2;
		private int changePowerDifficultyTimer = 31;

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
		}



		public void SpawnAsteroid(GameTime gameTime)
		{
	        int randomAsteroidPos = myRandom.Next(1,3);

	        totalTimeElapsed = (float) gameTime.TotalGameTime.TotalSeconds;

	        timeSinceLastAsteroid += (float) gameTime.ElapsedGameTime.TotalSeconds;

			timeElapsedEnemy += (float)gameTime.ElapsedGameTime.TotalSeconds;

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
			// Looks like shit, needs fixing for sure!
			timeSinceLastPower += (float)gameTime.ElapsedGameTime.TotalSeconds;

			totalTimeElapsed = (float)gameTime.TotalGameTime.TotalSeconds;

			List<HealthPower> healthPowers = new List<HealthPower>();
			List<SpeedPower> speedPowers = new List<SpeedPower>();
			//TODO Add all lists


			if (totalTimeElapsed > changePowerDifficultyTimer)
			{
				Console.WriteLine("Timer for power is: " + powerTimer);
				changePowerDifficultyTimer += 10;

				if (powerTimer != 25f)
				{
					powerTimer += 2.5f;
				}
			}


			int randomPowerPicker = myRandom.Next(1, 3); //myRandom.Next(1, 4); // Used to pick the which power to spawn
			int randomPowerPos = myRandom.Next(1, 3); // Used to choose position of the picked power

			if (timeSinceLastPower >= powerTimer)
			{
				if (randomPowerPicker == 1)
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
				else if (randomPowerPicker == 2)
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
				else if (randomPowerPicker == 3)
				{
					//TODO Add Other Powers
				}
				else if (randomPowerPicker == 4)
				{
					//TODO Add Other Powers
				}
				timeSinceLastPower = 0;
			}

		}


		public override void OnCollision(GameObject obj)
        {
            
        }
		public override void Attack(GameTime gameTime)
		{
			throw new NotImplementedException();
		}
	}
}
