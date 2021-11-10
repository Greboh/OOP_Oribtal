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

            public Enemy()
            {
                //this.position = new Vector2(myRandom.Next(0, (int)GameWorld.ScreenSize.X), 0);
                this.color = Color.White;
                this.scale = 1;
                this.speed = myRandom.Next(100, 100);
                //this.velocity.Y = 1;

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
            bool goingRight = true;
            



            if (position.X < GameWorld.ScreenSize.X - sprite.Width && goingRight == true)
            {
                this.velocity.X = 1;

                if (position.X > GameWorld.ScreenSize.X - sprite.Width)
                {
                    goingRight = false;
                }

            }
            else if (position.X > GameWorld.ScreenSize.X - sprite.Width && goingRight == false)
            {
                this.velocity.X = -1;

            }


            Console.WriteLine(goingRight);
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


