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
        float timeElapsed;

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public override void LoadContent(ContentManager content)
        {
           
        }

        public override void Update(GameTime gameTime)
        {
            SpawnAsteroid(gameTime);
        }

        public void SpawnAsteroid(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed >= 3.5f)
            {
                
                
                List<Asteroid> asteroids = new List<Asteroid>();
                Asteroid yAsteroid = new Asteroid(new Vector2(0, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));
                Asteroid xyAsteroid = new Asteroid(new Vector2((int)GameWorld.ScreenSize.X, myRandom.Next(0, (int)GameWorld.ScreenSize.Y)));
                Asteroid xAsteroid = new Asteroid(new Vector2(myRandom.Next(0, (int)GameWorld.ScreenSize.X), 0));

                asteroids.Add(xAsteroid);
                asteroids.Add(yAsteroid);
                asteroids.Add(xyAsteroid);

                foreach (Asteroid obj in asteroids)
                {
                    Instantiate(obj);
                }
                timeElapsed = 0;
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
