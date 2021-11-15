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
        private bool invincible = false;
        private float timeElapsed = 0;
        private Texture2D [] healthBars = new Texture2D[6];
        
        


        public Player()
        {
            this.color = Color.White;
            this.scale = 1;
            this.layerDepth = 1;
            this.animationFPS = 10;
            this.health = 100;
        }



        public override void LoadContent(ContentManager content)
        {

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = content.Load<Texture2D>(i + 1 + "exhaust");
            }
            for(int i = 0; i < healthBars.Length; i++)
            {
                healthBars[i] = content.Load<Texture2D>(i + 1 + "health");
            }
            


            animationSprite = sprites[0];
            sprite = content.Load<Texture2D>("Ship");

            this.position = new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2);
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
            CheckInvisiblity(gameTime);
            UpdateHealth(gameTime);
            
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
            int screenOffset = 20;
            velocity = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (position.Y <= GameWorld.ScreenSize.X - GameWorld.ScreenSize.X) // ScreenHeight 
                {
                    velocity = Vector2.Zero;
                }
                else velocity += new Vector2(0, -1);

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (position.Y >= GameWorld.ScreenSize.Y - screenOffset)
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
            this.speed = 200;

            if (velocity != Vector2.Zero) //Normalize movement vector for smoothness

            {
                velocity.Normalize();
            }

        }


        private void ScreenWarp()
        {
            if (position.X > GameWorld.ScreenSize.X)
            {
                position.X = 0;
            }
            else if (position.X < 0)
            {
                position.X = GameWorld.ScreenSize.X;
            }

            if (position.Y > GameWorld.ScreenSize.Y)
            {
                velocity = Vector2.Zero;
            }
        }

        public void CheckInvisiblity(GameTime gameTime)
		{
			if (invincible)
			{
				timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (timeElapsed > 2)
				{
					invincible = false;
					timeElapsed = 0;
				}
			}
		}

        public override void OnCollision( GameObject obj)
        {
			if (obj is Asteroid && !invincible)
			{
				Console.WriteLine($"{GetType().Name} collided with Asteroid");
				this.health -= 20;
				Console.WriteLine($"Current health is: {this.health}");
                invincible = true;
			}
            if(obj is SmallAsteroid && !invincible)
            {
                Console.WriteLine(GetType().Name + " collided with smallAsteroid");
                this.health -= 20;
                Console.WriteLine("Current health is: " + this.health);
                invincible = true;
            }

		}

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None, layerDepth);
            spriteBatch.Draw(animationSprite, position, null, color, rotation, exhaustPosition, 2, SpriteEffects.None, layerDepth);
            spriteBatch.Draw(healthBars[0], new Vector2(0, 850), null, color, 0, Vector2.Zero, 0.5f, SpriteEffects.None, layerDepth);
        }

        float timeSinceLastBugHit = 0;
        public void UpdateHealth(GameTime gameTime)
        {
#if DEBUG
            timeSinceLastBugHit += (float)gameTime.ElapsedGameTime.TotalSeconds; // Gets the game time in seconds (Framerate independent)
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                if (timeSinceLastBugHit >= 0.5f)
                {

                    health -= 20;
                    timeSinceLastBugHit = 0;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.CapsLock))
            {
                if (timeSinceLastBugHit >= 0.5f)
                {

                    health += 20;
                    timeSinceLastBugHit = 0;
                }
            }
#endif
            switch (health)
            {
                case 80:
                    {
                        healthBars[0] = healthBars[1];
                    }break;
                case 60:
                    {
                        healthBars[0] = healthBars[2];
                    }break;
                case 40:
                    {
                        healthBars[0] = healthBars[3];
                    }
                    break;
                case 20:
                    {
                        healthBars[0] = healthBars[4];
                    }
                    break;
                case 0:
                    {
                        healthBars[0] = healthBars[5];
                        Destroy(this);
                    }
                    break;
            }
            
        }

        public override void Attack(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState(); // This records mouse clicks and mouse posisiton



            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds; // Gets the game time in seconds (Framerate independent)

            if (mouseState.LeftButton == ButtonState.Pressed) // If mouse button is pressed
            {

                if (timeSinceLastShot > 0.5f)
                {
                    Instantiate(new Laser(position, shootingPoint, this.rotation));
                    timeSinceLastShot = 0;
                }

            }
        }

        



    }
}
