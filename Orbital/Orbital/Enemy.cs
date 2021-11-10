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

        public override void Attack(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}


