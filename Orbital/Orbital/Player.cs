using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Orbital.PowerUps;
using SharpDX.Direct2D1;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace Orbital
{
	class Player : GameObject
	{
		// Field for shooting
		private Vector2 exhaustPosition; //Vector2 for storing our ship exhausting point
		private Vector2 shootingPoint; // Vector2 for storing our shooting point
		private float timeSinceLastShot = 0f; //Timer for how fast you can shoot a new laser
		private float rateOfFire = 1f; // How fast the player initially ís able to fire!
		private float lowestRateOfFire = 0.2f; // How fast the player is able to fire at lowest
		private float subtractRateOfFire = 0.2f; // How much the rate of fire decreases after each pickup

		
		// Fields for health and taking damage
		private bool isInvincible = false;
		private float timeSinceLastHit = 0f; // Timer for invincibility
        private Texture2D [] healthBars = new Texture2D[6];
        private Texture2D currentHealthBar;
		private float timeSinceDeath = 0;


		//Fields for speed
		private Texture2D[] speedBars = new Texture2D[16];
		private Texture2D currentSpeedBar;
		private float speedBar = 0;


		public Player()
		{
			this.color = Color.White;
			this.scale = 1;
			this.layerDepth = 1;
			this.animationFps = 10;
			this.health = 100;
			this.speed = 200;
		}

		public override void LoadContent(ContentManager content)
		{
			for (int i = 0; i < exhaustSprites.Length; i++)
            {
                exhaustSprites[i] = content.Load<Texture2D>(i + 1 + "exhaust");
            }
            for(int i = 0; i < healthBars.Length; i++)
            {
                healthBars[i] = content.Load<Texture2D>(i + 1 + "health");
            }
			for(int i = 0; i < speedBars.Length; i++)
            {
				speedBars[i] = content.Load<Texture2D>(i + 1 + "speedbar");
            }

			for (int i = 0; i < deathSprites.Length; i++)
			{
				deathSprites[i] = content.Load<Texture2D>(i + 1 + "PlayerExplosion");
			}

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
			ImpactDisable(gameTime);
            UpdateHealth(gameTime);
			UpdateSpeed(gameTime);
			OnDeath(this.health);
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
			int screenOffset = 20; // Offset for the screen so the player cannot fly outside or half outside.
			velocity = Vector2.Zero; // Variable for the start velocity



			if (this.health >= 1) 
			{
				//Get Keyboard input for moving the player up and down
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

				// Get Keyboard input for moving the player left and right
				if (Keyboard.GetState().IsKeyDown(Keys.D))
				{
					velocity += new Vector2(1, 0);
				}
				else if (Keyboard.GetState().IsKeyDown(Keys.A))
				{
					velocity += new Vector2(-1, 0);
				}

				//Normalize movement vector for smoothness
				if (velocity != Vector2.Zero)
				{
					velocity.Normalize();
				}

				if (speedBar > 0 && Keyboard.GetState().IsKeyDown((Keys.LeftShift)))
				{
					this.speed = 400;
					Console.WriteLine($"Using turbo: {this.speed}");
					speedBar -= 0.5f;

				} 
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

		public void ImpactDisable(GameTime gameTime)
		{
			Color playerTransparencyColor= new Color(255, 255, 255, 0); // Transparent color

			if (isInvincible)
			{
				timeSinceLastHit += (float) gameTime.ElapsedGameTime.TotalSeconds;
				this.color = playerTransparencyColor;

				if (timeSinceLastHit > 2)
				{
					isInvincible = false;
					timeSinceLastHit = 0;
					this.color = Color.White;
				}


			}
		}

		public override void OnCollision(GameObject obj)
		{
			if (obj is Asteroid && !isInvincible || obj is SmallAsteroid && !isInvincible)
			{
				this.health -= 20;
				isInvincible = true;
			}

			if (obj is HealthPower)
			{
				if (this.health < 100) // Check if the player is at full health
				{
					this.health += 20;
				}

				Destroy(obj);
			}
			else if (obj is SpeedPower)
			{
				speedBar = 100;

				Destroy(obj);
			}
			else if (obj is RateOfFirePower)
			{
				if (rateOfFire <= lowestRateOfFire) // Checks if its
				{
					rateOfFire = lowestRateOfFire;
				}
				else if (rateOfFire < 0.24f) // Catches if rateOfFire is not a whole number
				{
					rateOfFire = lowestRateOfFire;
				}
				else rateOfFire -= subtractRateOfFire;
				
				Console.WriteLine(rateOfFire);

				Destroy(obj);
			}


		}

        public override void Draw(SpriteBatch spriteBatch)
        {
	        spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None, layerDepth);

			spriteBatch.Draw(animationSprite, position, null, color, rotation, exhaustPosition, 2, SpriteEffects.None, layerDepth);

			spriteBatch.Draw(currentHealthBar, new Vector2(0, 850), null, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, layerDepth);
			spriteBatch.Draw(currentSpeedBar, new Vector2(0, 600), null, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, layerDepth);
		}

		/// <summary>
		/// Updates healthbar according to damage/current health.
		/// </summary>
		/// <param name="gameTime"></param>
        public void UpdateHealth(GameTime gameTime)
        {
	        switch (health)
            {
				case 100:
                    {
						currentHealthBar = healthBars[0];
                    }break;
                case 80:
                    {
						currentHealthBar = healthBars[1];
						
                    }break;
                case 60:
                    {
						currentHealthBar = healthBars[2];
						
                    }break;
                case 40:
                    {
						currentHealthBar = healthBars[3];
						
                    }
                    break;
                case 20:
                    {
						currentHealthBar = healthBars[4];
						
                    }
                    break;
                case 0:
                    {
						currentHealthBar = healthBars[5];
						sprite = deathSprite;
                    }
                    break;
            }




        }

		/// <summary>
		/// updates speedBar according to current amount of speed
		/// </summary>
		/// <param name="gameTime">Reference to GameTime</param>
		public void UpdateSpeed(GameTime gameTime)
        {
			switch(speedBar)
            {
				case 100:
                    {
						currentSpeedBar = speedBars[0];
                    }break;
				case 90:
                    {
						currentSpeedBar = speedBars[1];
                    }break;
				case 80:
					{
						currentSpeedBar = speedBars[3];
					}
					break;
				case 70:
					{
						currentSpeedBar = speedBars[5];
					}
					break;
				case 60:
					{
						currentSpeedBar = speedBars[7];
					}
					break;
				case 50:
					{
						currentSpeedBar = speedBars[9];
					}
					break;
				case 40:
					{
						currentSpeedBar = speedBars[11];
					}
					break;
				case 30:
					{
						currentSpeedBar = speedBars[12];
					}
					break;
				case 20:
					{
						currentSpeedBar = speedBars[13];
					}
					break;
				case 10:
					{
						currentSpeedBar = speedBars[14];
					}
					break;
				case 0:
                    {
						currentSpeedBar = speedBars[15];
                    }break;
            }

        }

		/// <summary>
		/// Method that makes the player shoot a laser
		/// </summary>
		/// <param name="gameTime">Reference to GameTime</param>
        public override void Attack(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState(); // This records mouse clicks and mouse posisiton

			timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds; // Gets the game time in seconds (Framerate independent)

			if (mouseState.LeftButton == ButtonState.Pressed) // If mouse button is pressed
			{
				if (timeSinceLastShot > rateOfFire)
				{
					Instantiate(new Laser(position, shootingPoint, this.rotation, 1000));

					timeSinceLastShot = 0;
				}

			}
		}

		private void OnDeath(int playerHealth)
		{
			if (health <= 1)
			{
				if (deathSprite == deathSprites[9])
				{
					Destroy(this);
				}
			}
		}

	}
}
