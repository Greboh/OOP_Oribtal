using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital
{
    class SmallAsteroid : GameObject
    {
        private SoundEffect asteroidDestruction;
        
        /// <summary>
        /// construcer for SmallAsteroid that takes 3 parameters. 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="sprite"></param>
        public SmallAsteroid(Vector2 position, float scale, Texture2D sprite)
        {
            this.position = position; //sets position of the SmallAsteroid to the position given in the parameter
            this.scale = scale; // sets scale of the SmallAsteroid to the scale given in the parameter
            this.sprite = sprite; //sets sprite to the sprite given in the parameter
            this.color = Color.White;
            this.speed = 60; //sets speed for asteroid
            this.velocity = new Vector2(myRandom.Next(-2, +2), myRandom.Next(-2, +2)); //gives the Smallasteroid a random moving direction

            //if myRandom gives a 0 the velocity is set to X = 1, to avoid the SmallAsteroid not moving
            if (velocity.X == 0 || velocity.Y == 0)
            {
                velocity = new Vector2(1, 0);
            }
        }

        public override void Attack(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None, 0); //draws the SmallAstreoid
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("smallMeteor_0" + myRandom.Next(5, 7)); //loads a random SmallAsteroid sprite out of 2 possible asteroid sprites
            asteroidDestruction = content.Load<SoundEffect>("Asterioid_destruction_sound");
        }

        public override void OnCollision(GameObject obj)
        {
            if (obj is Laser)
            {
                ScoreManager.UpdateScore(50); // adds 50 to score when SmallAsteroid is hit by laser
                Destroy(this); // destroys SmallAsteroid
                Destroy(obj); //destroys Laser
                asteroidDestruction.Play(); // plays sound when SmallAsteroid is hit by laser
                
            }

        }
        /// <summary>
        /// Destroys Asteroid when the asteroid position exceeds any screensize
        /// </summary>
        private void ScreenBound()
        {
            //If asteroid position exceeds the screensize it is destroyed. No matter which side(x,y) it exceeds
            if (position.X > GameWorld.ScreenSize.X || position.X <0)
            {
                Destroy(this);
            }
            else if (position.Y > GameWorld.ScreenSize.Y || position.Y < 0)
            {
                Destroy(this);
            }
        }

        public override void Update(GameTime gametime)
        {
            HandleMovement(gametime); //updates movement of SmallAsteroid
            ScreenBound(); //implements screendeath for SmallAsteroid
        }
    }
}
