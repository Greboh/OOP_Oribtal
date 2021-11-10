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

            if (timeElapsed >= 2f)
            {
                Asteroid spawnedEnemy = new Asteroid();
                Instantiate(spawnedEnemy);
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
