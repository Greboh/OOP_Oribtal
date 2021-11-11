using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Orbital
{
	class Asteroid : GameObject
	{
        
        public Asteroid()
        {
            this.position = new Vector2(myRandom.Next(0, (int)GameWorld.ScreenSize.X), 0);
            this.color = Color.White;
            this.scale = 1;
            this.speed = myRandom.Next(20, 50);
            this.velocity.Y = 1;

        }
        public override void LoadContent(ContentManager content)
        {
            
            sprite = content.Load<Texture2D>("Meteor_0"+ myRandom.Next(5, 7));
           
        }

        public override void Update(GameTime gametime)
        {
            HandleMovement(gametime);
            ScreenBound();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None, 0);
        }

        public override void OnCollision(GameObject obj)
        {
            if (obj is Laser)
            {
                Destroy(this);
                Destroy(obj);
                for (int i = 0; i < 4; i++)
                {
                    Instantiate(new SmallAsteroid(this.position, this.scale,this.sprite));
                }
            }
        }

		public override void Attack(GameTime gameTime)
		{
			throw new NotImplementedException();
		}

        private void ScreenBound()
        {
			if (position.Y > GameWorld.ScreenSize.X)
			{
				Destroy(this);
			}
			else if (position.X > GameWorld.ScreenSize.Y)
			{
				Destroy(this);
			}
		}
	}
}
