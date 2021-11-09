using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Orbital
{
	public abstract class GameObject
	{

		protected Texture2D sprite;
		protected Texture2D exhaustSprite;
		protected Texture2D[] exhaustSprites = new Texture2D[4];

		protected Vector2 position;
		protected Vector2 velocity;
		protected Vector2 offset;
		protected Vector2 origin;

		protected Color color;

		protected int health;
		protected int damage;
		private float timeElapsed;
		private int currentIndex;

		protected float scale;
		protected float speed;
		protected float animationFPS = 10;
		protected float rotation;
		protected const float tangentialVelocity = 2f;
		protected float friction = 0.1f;

		protected Rectangle spriteRectangle;

		private GameWorld myGameWorld;

		protected Random myRandom = new Random();

		public Rectangle Collision
		{
			get
			{
				return new Rectangle(
					   (int)(position.X + offset.Y),
					   (int)(position.Y + offset.X),
					   sprite.Width,
					   sprite.Height
				   );
			}
		}

		public abstract void LoadContent(ContentManager content);

		protected void HandleMovement(GameTime gameTime)
		{
			float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			position += ((velocity * speed) * deltaTime);
		}

		public abstract void Update(GameTime gametime);

		public abstract void Draw(SpriteBatch spriteBatch);

		protected void Animate(GameTime gametime)
		{
			timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

			currentIndex = (int)(timeElapsed * animationFPS);

			exhaustSprite = exhaustSprites[currentIndex];

			if (currentIndex >= exhaustSprites.Length - 1)
			{
				timeElapsed = 0;
				currentIndex = 0;
			}
		}

		public abstract void OnCollision(GameObject obj);

		public void CheckCollision(GameObject obj)
		{
			if (Collision.Intersects(obj.Collision))
			{
				OnCollision(obj);
			}
		}

		public void SetGameWorld(GameWorld gameWorld)
		{
			myGameWorld = gameWorld;
		}

		public void Instantiate(GameObject gameObject)
		{
			myGameWorld.Instantiate(gameObject);
		}

		public void Destroy(GameObject gameObject)
		{
			myGameWorld.DestroyGameObject(gameObject);
		}


	}
}
