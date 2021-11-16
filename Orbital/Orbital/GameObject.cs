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
		protected Texture2D animationSprite;
		protected Texture2D[] sprites = new Texture2D[4];


		protected Vector2 position;
		protected Vector2 velocity;
		protected Vector2 offset;
		protected Vector2 origin;

		protected Color color;
		protected SpriteEffects shipFlip;

		protected Vector2 bulletDirection;
		protected float rotation;

		protected int health;
		protected int damage;
		protected float timeElapsed;
		private int currentIndex;

		protected float scale;
		protected float speed;
		protected float animationFps;
		protected float layerDepth;

		private GameWorld myGameWorld;

		protected Random myRandom = new Random();

		public Rectangle Collision
		{
			get
			{
				return new Rectangle(
					(int)(position.X + offset.Y),
					(int)(position.Y + offset.X),
					sprite != null ? sprite.Width : 0,
					sprite != null ? sprite.Height : 0
				    );
			}
		}


		public abstract void LoadContent(ContentManager content);

		protected void HandleMovement(GameTime gameTime)
		{
			float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			position += ((velocity * speed) * deltaTime);
		}

		public abstract void Update(GameTime gameTime);

		public abstract void Draw(SpriteBatch spriteBatch);

		protected void Animate(GameTime gameTime)
		{
			timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

			currentIndex = (int)(timeElapsed * animationFps);

			animationSprite = sprites[currentIndex];

			if (currentIndex >= sprites.Length - 1)
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
			//Console.WriteLine(gameObject + " Has been added!");

			myGameWorld.Instantiate(gameObject);
		}

		public void Destroy(GameObject gameObject)
		{
			//Console.WriteLine(gameObject + " Has been Destroyed!");

			myGameWorld.DestroyGameObject(gameObject);
		}

		public abstract void Attack(GameTime gameTime);

	}
}
