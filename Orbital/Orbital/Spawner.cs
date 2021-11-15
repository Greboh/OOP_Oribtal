using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Orbital.PowerUps;


namespace Orbital
{
	class Spawner : GameObject
	{
        private float timeElapsed;
        private float timeSinceLastPower;

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

	        timeElapsed += (float) gameTime.ElapsedGameTime.TotalSeconds;

            // [IDEA]
            // Replace with if statement that decides in which of the four sides the asteroid should spawn using random.Next

            // Maybe solved now on line 56 - 64
            // Now it only spawns 1 asteroid at least

            if (timeElapsed >= 3.5f)
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

				timeElapsed = 0;

            }

            

		}

		private void SpawnPower(GameTime gameTime)
		{
			// Looks like shit, needs fixing for sure!
			timeSinceLastPower += (float)gameTime.ElapsedGameTime.TotalSeconds;

			List<HealthPower> healthPowers = new List<HealthPower>();
			List<SpeedPower> speedPowers = new List<SpeedPower>();
			//TODO Add all lists



			int randomPowerPicker = 2; //myRandom.Next(1, 4); // Used to pick the which power to spawn
			int randomPowerPos = myRandom.Next(1, 3); // Used to choose position of the picked power

			if (timeSinceLastPower >= 5)
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
