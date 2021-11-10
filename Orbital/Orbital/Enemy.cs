using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Orbital
{

    class Enemy : GameObject
    {
        bool movingRight = true;
        private Vector2 shootingPoint;
        private float timeSinceLastShot = 0f;


        public Enemy()
        {
            //this.position = new Vector2(myRandom.Next(0, (int)GameWorld.ScreenSize.X), 0);
            this.color = Color.White;
            this.scale = 1;
            this.speed = 3;
        }
        public override void LoadContent(ContentManager content)
        {

            sprite = content.Load<Texture2D>("Ship2");

        }

        public override void Update(GameTime gametime)
        {
            HandleMovement(gametime);
            ScreenBound();
            ShipMovement();
            Attack(gametime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None, 0);
        }

        public override void OnCollision(GameObject obj)
        {
            if (obj is Laser)
            {

            }
        }

        private void ShipMovement()
        {

            if (movingRight)
            {
                position.X += speed;
            }else
            {
                position.X -= speed;
            }if (position.X > GameWorld.ScreenSize.X - this.sprite.Width || position.X < 0)
            {
                movingRight = !movingRight;
            }
                

            //Console.WriteLine(position.X);
            //Console.WriteLine(movingRight);
            //Console.WriteLine(GameWorld.ScreenSize.X);
        }

        public override void Attack(GameTime gameTime)
        {



            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds; // Gets the game time in seconds (Framerate independent)

            if (timeSinceLastShot > 1) // 
            {

               
                    Instantiate(new Laser(position, shootingPoint, this.rotation ));
                    timeSinceLastShot = 0;

                Console.WriteLine();
            }




        }

        private void ScreenBound()
        {
            //if (position.Y > GameWorld.ScreenSize.X)
            //{
            //    Destroy(this);
            //}
            //else if (position.X > GameWorld.ScreenSize.Y)
            //{
            //    Destroy(this);
            //}
        }

        
    }
}


