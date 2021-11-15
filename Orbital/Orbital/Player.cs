using System;
using System.Collections.Generic;
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
		private Vector2 exhaustPosition; //Vector2 for storing our ship exhausting point
		private Vector2 shootingPoint; // Vector2 for storing our shooting point

		// Field for shooting cooldown
		private float timeSinceLastShot = 0f; //Timer for how fast you can shoot a new laser

		private int totalShotsFired = 0; //Variable for closing while loop when shooting more than 1 laser
		private float newSpeed;
		private float speedMultiplier = 1.5f;
		
		// Fields for taking damage
		private bool isInvincible = false;
		private float timeSinceLastHit = 0f; // Timer for invinsibility

		public Player()
		{
			this.color = Color.White;
			this.scale = 1;
			this.layerDepth = 1;
			this.animationFps = 10;
			this.health = 100;
			this.speed = 200;
			newSpeed = this.speed;
		}

		public override void LoadContent(ContentManager content)
		{

			for (int i = 0; i < sprites.Length; i++)
			{
				sprites[i] = content.Load<Texture2D>(i + 1 + "exhaust");
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
			ImpactDisable(gameTime);
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

			if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) // Speed boost
			{
				this.speed = newSpeed * speedMultiplier;
				Console.WriteLine($"Using turbo: {this.speed}");
			}
			else this.speed = newSpeed;

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

		public void ImpactDisable(GameTime gameTime)
		{
			if (isInvincible)
			{
				timeSinceLastHit += (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (timeSinceLastHit > 2)
				{
					isInvincible = false;
					timeSinceLastHit = 0;
				}
			}
		}

		public override void OnCollision(GameObject obj)
		{
			if (obj is Asteroid && !isInvincible)
			{
				Console.WriteLine($"{GetType().Name} collided with Asteroid");
				this.health -= 20;
				Console.WriteLine($"Current health is: {this.health}");
				isInvincible = true;
			}

			if (obj is HealthPower)
			{
				if (this.health < 100)
				{
					this.health += 20;
				}
				Console.WriteLine($"Current health is: {this.health}");
			}
			else if (obj is SpeedPower)
			{
				newSpeed += 200;
				this.speed += newSpeed;
				Console.WriteLine(this.speed);
			}

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
				if (timeSinceLastShot > .5)
				{
					Instantiate(new Laser(position, shootingPoint, this.rotation, 1000));
					totalShotsFired++;

					Console.WriteLine(Mouse.GetState());
					timeSinceLastShot = 0;
				}
			}
		}
	}
}
