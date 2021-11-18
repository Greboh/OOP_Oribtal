using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Orbital.PowerUps
{
	class RateOfFirePower : GameObject
	{
		#region Methods
		public RateOfFirePower(Vector2 position)
		{
			this.position = position;
			this.color = Color.White;
			this.scale = 1;
			this.speed = myRandom.Next(100, 200);

			if (this.position.X == GameWorld.ScreenSize.X)
			{
				this.velocity.X = -1;
			}
			else if (this.position.Y == 0)
			{
				this.velocity.Y = 1;
			}
			else this.velocity.X = 1;

		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.sprite, position, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
		}

		public override void LoadContent(ContentManager content)
		{
			this.sprite = content.Load<Texture2D>("RateOfFirePower");
		}

		public override void Attack(GameTime gameTime)
		{
		}

		public override void OnCollision(GameObject obj)
		{

		}

		public override void Update(GameTime gameTime)
		{
			HandleMovement(gameTime);
		}

		#endregion
	}
}
