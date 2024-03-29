﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Orbital
{
	class Laser : GameObject
	{
		/// <summary>
		/// Constructor for our laser
		/// </summary>
		/// <param name="position">The position of our player</param>
		/// <param name="shootingPoint">The position we want to shoot from</param>
		/// <param name="rotation">The rotation of our player</param>
		public Laser(Vector2 position, Vector2 shootingPoint, float rotation, float speed)
		{
			// Cos(rotation) is used to determine what velocity.x is between -1 and 1 based on rotations angle
			// Sin(rotation) is used to determine what velocity.y is between -1 and 1 based on rotations angle
			velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
			this.speed = speed;
			this.position = position;
			this.origin = shootingPoint;
			this.layerDepth = 0.5f;
			this.scale = 3;
			this.animationFps = 10;
			this.rotation = rotation;
			this.color = Color.White;
		}

		#region Methods

		/// <summary>
		/// Calculated the screenbounds and destroys the laser if it exceeds
		/// </summary>
		private void ScreenBounds()
		{
			if (position.X > GameWorld.ScreenSize.X || position.X < GameWorld.ScreenSize.X - GameWorld.ScreenSize.X ||
			    position.Y > GameWorld.ScreenSize.Y || position.Y < GameWorld.ScreenSize.Y - GameWorld.ScreenSize.Y)
			{
				Destroy(this);
			}
		}

		public override void Attack(GameTime gameTime)
		{
		}

		public override void LoadContent(ContentManager content)
		{
			for (int i = 0; i < exhaustSprites.Length; i++)
			{
				exhaustSprites[i] = content.Load<Texture2D>(i + 1 + "pShoot");
			}

			animationSprite = exhaustSprites[0];
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(animationSprite, position, null, color, this.rotation, origin, scale, SpriteEffects.None, layerDepth);
		}

		public override void OnCollision(GameObject obj)
		{

           

		}
		public override void Update(GameTime gameTime)
		{
			Animate(gameTime);
			HandleMovement(gameTime);
			ScreenBounds();

		}

		#endregion
	}
}
