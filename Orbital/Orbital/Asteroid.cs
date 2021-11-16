using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;


namespace Orbital
{
	class Asteroid : GameObject
	{
        private SoundEffect asteroidDestruction;
        
        /// <summary>
        /// Constructor uses position paramater to spawn asteroid on different axis and set movement accordingly
        /// </summary>
        /// <param name="position"></param>
        public Asteroid(Vector2 position)
        {
            this.position = position;
            this.color = Color.White;
            this.scale = 1;
            this.speed = myRandom.Next(20, 50);

            if (this.position.X == GameWorld.ScreenSize.X)
            {
                this.velocity.X = -1;
            }
            else if(this.position.Y == 0)
            {
                this.velocity.Y =1;
            }
            else this.velocity.X = 1;
        }

        /// <summary>
        /// Loads Random Asteroid Sprite texture. 
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            
            sprite = content.Load<Texture2D>("Meteor_0"+ myRandom.Next(5, 7));
            asteroidDestruction = content.Load<SoundEffect>("Asterioid_destruction_sound");
           
        }

        public override void Update(GameTime gameTime)
        {
            HandleMovement(gameTime);
            ScreenBound();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Method for destroying Asteroids when hit by laser. 
        /// Method Spawns smallAsteroids when Asteroid is destroyed.
        /// </summary>
        /// <param name="obj"></param>
        public override void OnCollision(GameObject obj)
        {
            if (obj is Laser)
            {
                Destroy(this);
                Destroy(obj);
                asteroidDestruction.Play();
                for (int i = 0; i < 4; i++)
                {
                    Instantiate(new SmallAsteroid(this.position, this.scale, this.sprite));
                }
            }
        }

		public override void Attack(GameTime gameTime)
		{
		}

        /// <summary>
        /// Destroys Asteroid when the asteroid position exceed X-axis maximum.
        /// Move asteroid to oppesite side of the Y-axis max/min they've reached.
        /// </summary>
        private void ScreenBound()

        {
            if (position.Y > GameWorld.ScreenSize.Y)
            {
                Destroy(this);
            }
            if (position.X > GameWorld.ScreenSize.X)
            {
                position.X = 0;
            }
            else if (position.X < 0)
            {
                position.X = GameWorld.ScreenSize.X;
            }
        }
	}
}
