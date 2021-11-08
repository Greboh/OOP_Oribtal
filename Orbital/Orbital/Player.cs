using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Orbital
{
	class Player : GameObject
	{
        public Player()
        {
            this.position = new Vector2(500, 500);
            this.color = Color.White;
			this.scale = 1;
		} 



        public override void LoadContent(ContentManager content)
        {
            //exhaustSprites = new Texture2D[4];

			for (int i = 0; i < exhaustSprites.Length; i++)
			{
                exhaustSprites[i] = content.Load<Texture2D>(i + 1 + "exhaust");
			}

            sprite = content.Load<Texture2D>("Ship");
            exhaustSprite = exhaustSprites[0];
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);

        }



        public override void Update(GameTime gameTime)
        {
            HandleInput();
            HandleMovement(gameTime);
            Animate(gameTime);
            ScreenWarp();

		}

        private void HandleInput()
        {
            int screenOffset = 20;
            velocity = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (position.Y <= GameWorld.ScreenWidth - GameWorld.ScreenWidth) // ScreenHeight 
                {
                    velocity = Vector2.Zero;
                }
                else velocity += new Vector2(0, -1);

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (position.Y >= GameWorld.ScreenHeight - screenOffset)
                {
                    velocity = Vector2.Zero;
                }
                else velocity += new Vector2(0, 1);

            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                velocity += new Vector2(-1, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) // Speed boost
            {
                this.speed = 350;
            }
            else this.speed = 200;

            if (velocity != Vector2.Zero) //Normalize movement vector for smoothness

            {
                velocity.Normalize();
            }

            Console.WriteLine(velocity);
        }


        private void ScreenWarp()
        {
            if (position.X > GameWorld.ScreenWidth)
            {
                position.X = 0;
            }
            else if (position.X < 0)
            {
                position.X = GameWorld.ScreenWidth;
            }

            if (position.Y > GameWorld.ScreenHeight)
            {
                velocity = Vector2.Zero;
            }
        }

        public override void OnCollision(GameObject obj)
        {
            
        }

		public override void Draw(SpriteBatch spriteBatch)
		{
            spriteBatch.Draw(sprite, position, null, color, 0, origin, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(exhaustSprite, position, null, color, 0, new Vector2(38,13), 2, SpriteEffects.None, 0);
        }
    }
}
