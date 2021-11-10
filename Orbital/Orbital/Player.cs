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
		private Vector2 exhaustPosition; //Vector2 for storing our ship exhausting point
        private Vector2 shootingPoint; // Vector2 for storing our shooting point

        // Field for shooting cooldown
		private float timeSinceLastShot = 0f; 


        public Player()
        {
            this.color = Color.White;
			this.scale = 1;
            this.layerDepth = 1;
            this.animationFPS = 10;
		} 



        public override void LoadContent(ContentManager content)
        {

			for (int i = 0; i < sprites.Length; i++)
			{
				sprites[i] = content.Load<Texture2D>(i + 1 + "exhaust");
			}



			animationSprite = sprites[0];
            sprite = content.Load<Texture2D>("Ship");

            this.position = new Vector2(GameWorld.Screensize.X / 2, GameWorld.Screensize.Y / 2);
            this.origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            exhaustPosition = new Vector2(sprite.Width / 2, (sprite.Height / 2) - 20);
            shootingPoint = new Vector2((sprite.Width / 2) - 30, (sprite.Height / 2) - 15);

            this.offset.X = (-sprite.Width / 2); // Used to draw collisionBox
            this.offset.Y = -sprite.Height / 2; // Used to draw collisionBox

        }



        public override void Update(GameTime gameTime)
		{

			HandleInput();
			HandleMovement(gameTime);
			Animate(gameTime);
			ScreenWarp();
			LookAtMouse();
			Attack(gameTime);
		}

		private void LookAtMouse()
		{
            MouseState mouseState = Mouse.GetState(); // This records mouse clicks and mouse posisiton
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 targetedAngle = mousePosition - position;
            rotation = (float)Math.Atan2(targetedAngle.Y, targetedAngle.X);
        }

        private void HandleInput()
        {
            ///////////////////////
            /// New movement method
            ///////////////////////

            position = velocity + position;

            if (Keyboard.GetState().IsKeyDown(Keys.Left)) rotation -= 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) rotation += 0.1f;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (position.Y <= GameWorld.Screensize.X - GameWorld.Screensize.X) // ScreenHeight 
                {
                    velocity = Vector2.Zero;
                }
                else velocity += new Vector2(0, -1);
                 
            }
            else if (velocity != Vector2.Zero)
            {
                if (position.Y >= GameWorld.Screensize.Y - screenOffset)
                {
                    velocity = Vector2.Zero;
                }
                else velocity += new Vector2(0, 1);



            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) // Speed boost
            {
                this.speed = 350;
            }   this.speed = 200;



        }


        private void ScreenWarp()
        {
            if (position.X > GameWorld.Screensize.X)
            {
                position.X = 0;
            }
            else if (position.X < 0)
            {
                position.X = GameWorld.Screensize.X;
            }

            if (position.Y > GameWorld.Screensize.Y)
            {
                velocity = Vector2.Zero;
            }
        }

        public override void OnCollision(GameObject obj)
        {
            
        }

		public override void Draw(SpriteBatch spriteBatch)
		{
            spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None, layerDepth);
            spriteBatch.Draw(animationSprite, position, null, color, rotation, exhaustPosition, 2, SpriteEffects.None, layerDepth);
        }

		public override void Attack(GameTime gameTime)
		{
            MouseState mouseState = Mouse.GetState(); // This records mouse clicks and mouse posisiton



            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds; // Gets the game time in seconds (Framerate independent)

            if (mouseState.LeftButton == ButtonState.Pressed) // If mouse button is pressed
			{

				if (timeSinceLastShot > 0.5f)
				{
                    Instantiate(new Laser( position, shootingPoint, this.rotation));
					timeSinceLastShot = 0;
				}

			}




        }




	}
}
