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
        float timeElapsedAstroid;
        float timeElapsedEnemy;


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
            timeElapsedAstroid += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeElapsedEnemy += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsedAstroid >= 2f)
            {
               
                Asteroid spawnedAstroid = new Asteroid();
                Instantiate(spawnedAstroid);
                timeElapsedAstroid = 0;



            }
            if (timeElapsedEnemy >= 5f)
            {
                Enemy spawnedEnemy = new Enemy();
                Instantiate(spawnedEnemy);
                timeElapsedEnemy = 0;

                
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
