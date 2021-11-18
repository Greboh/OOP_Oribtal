using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Orbital.PowerUps;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace Orbital
{
	class Player : GameObject
	{
		#region  Fields

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
		private Texture2D[] healthBars = new Texture2D[6]; //sprites indicating different amounts of health 
		private Texture2D currentHealthBar; //sprie used to show the current amount of player health

		//Fields for speed
		private Texture2D[] speedBars = new Texture2D[16]; //sprites indicating different amounts of speed
		private Texture2D currentSpeedBar; //sprite used to show the current amount of speed available for player
		private float speedBar = 0; //Player starts out with zero turbo

		//Fields for Sound Effects
		private SoundEffect gameOverSound; //plays when player dies
		private SoundEffect laserSound; //plays when laser is fired
		private SoundEffect playerHit; //plays when player takes any damage
		private SoundEffect turboPickUp; //plays when SpeedPower is picked up by player
		private SoundEffect healthPickUp;//plays when HealhtPower is picked up by player
		private SoundEffect firepowerPickUp; //plays when RateOfFirePower is picked up by player

		#endregion

		/// <summary>
		/// Constructor for the player
		/// </summary>
		public Player()
		{
			this.color = Color.White;
			this.scale = 1;
			this.layerDepth = 1;
			this.animationFps = 10;
			this.health = 100;
			this.speed = 200;
		}

		#region Methods

		/// <summary>
		/// Loads all sprites that is used by the player
		/// </summary>
		/// <param name="content"></param>
		public override void LoadContent(ContentManager content)
		{
			// Loads all exhaustSprites
			for (int i = 0; i < exhaustSprites.Length; i++)
			{
				exhaustSprites[i] = content.Load<Texture2D>(i + 1 + "exhaust");
			}
			// Loads all healthbars
			for (int i = 0; i < healthBars.Length; i++)
			{
				healthBars[i] = content.Load<Texture2D>(i + 1 + "health");
			}
			// Loads all speedbars
			for (int i = 0; i < speedBars.Length; i++)
			{
				speedBars[i] = content.Load<Texture2D>(i + 1 + "speedbar");
			}
			// Loads all deathSprites
			for (int i = 0; i < deathSprites.Length; i++)
			{
				deathSprites[i] = content.Load<Texture2D>(i + 1 + "PlayerExplosion");
			}

			//Loads all soundeffects
			gameOverSound = content.Load<SoundEffect>("gameoverSound");
			laserSound = content.Load<SoundEffect>("pewpew");
			playerHit = content.Load<SoundEffect>("Player_hit_Effect");
			turboPickUp = content.Load<SoundEffect>("turbopowerup_sound");
			healthPickUp = content.Load<SoundEffect>("healthpowerup_sound");
			firepowerPickUp = content.Load<SoundEffect>("firePower_sound");

			sprite = content.Load<Texture2D>("Ship"); // Sets the ship sprite

			this.position = new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2); // Sets the players start position to the middle of the screen
			this.origin = new Vector2(sprite.Width / 2, sprite.Height / 2); // Sets the origin to the middle of the sprite
			exhaustPosition = new Vector2(sprite.Width / 2, (sprite.Height / 2) - 20); // Sets the position for the exhaust to draw
			shootingPoint = new Vector2((sprite.Width / 2) - 30, (sprite.Height / 2) - 15); // Sets the position of the laser to draw

			this.offset.X = (-sprite.Width / 2); // Used to draw collisionBox
			this.offset.Y = -sprite.Height / 2; // Used to draw collisionBox

		}

		/// <summary>
		///  Handles everything that needs constant updating
		/// </summary>
		/// <param name="gameTime"></param>
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

		/// <summary>
		/// Makes the player rotate towards mouse
		/// </summary>
		private void LookAtMouse()
		{
			MouseState mouseState = Mouse.GetState(); // This records mouse clicks and mouse posisiton
			Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y); // Gets the X and Y posistions of the mouse and stores it in a vector2
			Vector2 targetedAngle = mousePosition - position; // Gets the position at which we are aiming
			rotation = (float)Math.Atan2(targetedAngle.Y, targetedAngle.X); // Returns the angle between 2 coordinates in radians
		}

		/// <summary>
		/// Handles all player input making it possible to control the player
		/// </summary>
		private void HandleInput()
		{
			int screenOffset = 20; // Offset for the screen so the player cannot fly outside or half outside.
			velocity = Vector2.Zero; // Variable for the start velocity
			this.speed = 200;

			//Disables input if the health is equal to or lower than 0
			if (this.health >= 0)
			{
				//Get Keyboard input for moving the player up and down
				if (Keyboard.GetState().IsKeyDown(Keys.W))
				{
					// Checks if the player position is less than 0 on the Y-Axis 
					if (position.Y <= GameWorld.ScreenSize.Y - GameWorld.ScreenSize.Y)
					{
						velocity = Vector2.Zero;
					}
					else velocity += new Vector2(0, -1);

				}
				else if (Keyboard.GetState().IsKeyDown(Keys.S))
				{
					// Checks if the player position is more than the screensize - a small offset on the 
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

				//If Player has any SpeedPower available, get Keyboard input for implementing speed power
				if (speedBar > 0 && Keyboard.GetState().IsKeyDown((Keys.LeftShift)))
				{
					this.speed = 400; //adjusts Players speed to 400
					speedBar -= 0.5f; //subtracts from Players current amount of speed

				}
			}

		}

		/// <summary>
		/// Warps the player on the X-Axis so he cannot fly out of the screen
		/// </summary>
		private void ScreenWarp()
		{
			// Checks if the player is outside the screen on the X-axis
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
		/// <summary>
		/// Makes the player not able to be hit twice in a row. But instead gives him a window of invulnerability 
		/// </summary>
		/// <param name="gameTime"></param>
		public void ImpactDisable(GameTime gameTime)
		{
			Color playerTransparencyColor = new Color(255, 255, 255, 0); // Transparent color

			// checks if the player has taken damage 
			if (isInvincible)
			{
				timeSinceLastHit += (float)gameTime.ElapsedGameTime.TotalSeconds;
				this.color = playerTransparencyColor;

				// Resets so the player can take damage again after 2 seconds
				if (timeSinceLastHit > 2)
				{
					isInvincible = false;
					timeSinceLastHit = 0;
					this.color = Color.White;
				}


			}
		}

		/// <summary>
		/// Decides what happends if the player collides with anything
		/// </summary>
		/// <param name="obj">The obj that the player collides with</param>
		public override void OnCollision(GameObject obj)
		{
			if (obj is Asteroid && !isInvincible || obj is SmallAsteroid && !isInvincible)
			{
				this.health -= 20;
				isInvincible = true;
				playerHit.Play();
			}

			if (obj is HealthPower)
			{
				if (this.health < 100) // Check if the player is at full health
				{
					this.health += 20; //adds 20 to health
					Destroy(obj);
					healthPickUp.Play();
				}
				else Destroy(obj);
			}
			else if (obj is SpeedPower)
			{
				speedBar = 100; //gives player full speed
				Destroy(obj);
				turboPickUp.Play();
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

				firepowerPickUp.Play();
				Destroy(obj);
				Console.WriteLine(rateOfFire);
			}

			//When player is hit by enemyship attack
			if (obj is EnemyAttack)
			{
				this.health -= 20;
				isInvincible = true;
				Destroy(obj);
			}


		}

		/// <summary>
		/// Draws all our sprites that the player uses
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None, layerDepth);

			spriteBatch.Draw(animationSprite, position, null, color, rotation, exhaustPosition, 2, SpriteEffects.None, layerDepth);

			spriteBatch.Draw(currentHealthBar, new Vector2(0, 850), null, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, layerDepth);
			spriteBatch.Draw(currentSpeedBar, new Vector2(0, 600), null, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, layerDepth);
		}

		/// <summary>
		/// Updates currentHealthBar according to damage/current health.
		/// </summary>
		/// <param name="gameTime"></param>
		public void UpdateHealth(GameTime gameTime)
		{
			switch (this.health)
			{
				case 100:
					{
						currentHealthBar = healthBars[0]; //sets current healthbar to 100% capacity
					}
					break;
				case 80:
					{
						currentHealthBar = healthBars[1]; //sets currentHealthbar to 80% capacity

					}
					break;
				case 60:
					{
						currentHealthBar = healthBars[2];

					}
					break;
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
						currentHealthBar = healthBars[5]; //sets currentHealthBar to 0% capacity
						sprite = deathSprite;
					}
					break;
			}




		}

		/// <summary>
		/// updates currentspeedBar sprite according to current amount of speed
		/// </summary>
		/// <param name="gameTime">Reference to GameTime</param>
		public void UpdateSpeed(GameTime gameTime)
		{
			switch (speedBar)
			{
				case 100:
					{
						currentSpeedBar = speedBars[0]; //sets currentspeedbar to 100% capacity
					}
					break;
				case 90:
					{
						currentSpeedBar = speedBars[1]; //sets currentspeedbar to ca. 90% capacity
					}
					break;
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
						currentSpeedBar = speedBars[15]; //sets currentSpeedBar to 0% capacity
					}
					break;
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
					laserSound.Play();

					timeSinceLastShot = 0;
				}

			}
		}

		/// <summary>
		/// Logic that happends when the player dies
		/// </summary>
		/// <param name="playerHealth"></param>
		private void OnDeath(int playerHealth)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.G))
			{
				this.health = 0;
			}

			// If health drops to 0 or less
			if (health <= 0)
			{
				// if deathSprite is equal to deathSprites with index 9, we know the animation is done
				if (deathSprite == deathSprites[9])
				{
					gameOverSound.Play();
					Destroy(this);
					myGameWorld.currentGameState = Gamestate.DeathScreen; // Sets the GameState to DeathScreen
				}
			}
		}
		#endregion
	}
}
