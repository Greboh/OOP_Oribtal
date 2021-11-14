using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


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
				Console.WriteLine("test");
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

			List<HealthPower> powers = new List<HealthPower>();
			//TODO Add all lists



			int randomPowerPicker = 1; //myRandom.Next(1, 4); // Used to pick the which power to spawn
			int randomHealthPowerPos = myRandom.Next(1, 3); // Used to choose position of the picked power

			if (timeSinceLastPower >= 20)
			{
				if (randomPowerPicker == 1)
				{

					HealthPower yPower = new HealthPower(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));
					HealthPower xyPower = new HealthPower(new Vector2((int)GameWorld.ScreenSize.X, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));
					HealthPower xPower = new HealthPower(new Vector2(myRandom.Next(0, (int)GameWorld.ScreenSize.X), 0));

					powers.Add(xPower);
					powers.Add(yPower);
					powers.Add(xyPower);



					if (randomHealthPowerPos == 1)
					{
						Instantiate(powers[0]);
					}
					else if (randomHealthPowerPos == 2)
					{
						Instantiate(powers[1]);
					}
					else Instantiate(powers[2]);

					Console.WriteLine("Health spawned!");
				}
				else if (randomPowerPicker == 2)
				{
					//TODO Add Other Powers
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
