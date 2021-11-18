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
        /// Constructor for Asteroid. Takes Position as parameter
        /// </summary>
        /// <param name="position"></param>
        public Asteroid(Vector2 position)
        {
            this.position = position; //spawn position for Asteroid
            this.color = Color.White;
            this.scale = 1; //size of Asteroid
            this.speed = myRandom.Next(200, 300); //random speed for Asteroid

            //Sets direction of Asteroid movement in relation to its spawn position
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
            
            sprite = content.Load<Texture2D>("Meteor_0"+ myRandom.Next(5, 7)); //loads a random asteroid sprite out of 2 possible asteroid sprites
            asteroidDestruction = content.Load<SoundEffect>("Asterioid_destruction_sound"); //loads destruction sound for asteroid
           
        }
        
        public override void Update(GameTime gameTime)
        {
            HandleMovement(gameTime); //updates movement of Asteroid
            ScreenBound(); // implements screendeath for Asterod
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None, 0);//draw asteroid
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
                ScoreManager.UpdateScore(100); //adds 100 to score when asteroid is hit by laser
                Destroy(this); //destroys asteroid
                Destroy(obj);//destroys object
                asteroidDestruction.Play(); // plays sound when asteroid is hit by laser

                //Spawns 4 smallAsteroids when asteroid is destroyed
                for (int i = 0; i < 4; i++)
                {
                    Instantiate(new SmallAsteroid(this.position, this.scale, this.sprite)); //spawns smallasteroid at position
                }
            }
        }

		public override void Attack(GameTime gameTime)
		{

		}

        /// <summary>
        /// Destroys Asteroid when the asteroid position exceeds any screensize
        /// 
        /// </summary>
        private void ScreenBound()

        {
            //If asteroid position exceeds the screensize it is destroyed. No matter which side(x,y) it exceeds
            if (position.Y > GameWorld.ScreenSize.Y)
            {
                Destroy(this);
            }
            if (position.X > GameWorld.ScreenSize.X)
            {
                Destroy(this);
            }
            else if (position.X < 0)
            {
                Destroy(this);
            }
        }
	}
}
