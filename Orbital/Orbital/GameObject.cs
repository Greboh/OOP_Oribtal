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
		protected Texture2D[] sprites = new Texture2D[4];

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
		protected float fps;

		private Random myRandom = new Random();

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

		protected void Move(GameTime gameTime)
		{
			float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			position += ((velocity * speed) * deltaTime);
		}

		public abstract void Update(GameTime gametime);

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(sprite, position, null, color, 0, origin, scale, SpriteEffects.None, 0);


		}
		protected void Animate(GameTime gametime)
		{
			timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

			currentIndex = (int)(timeElapsed * fps);
			sprite = sprites[currentIndex];

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



	}
}
