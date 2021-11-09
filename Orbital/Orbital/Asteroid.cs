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
            this.position = new Vector2(myRandom.Next(0, 1200), myRandom.Next(0, 0));
            this.color = Color.White;
            this.scale = 0.3f;
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None, 0);
        }

        public override void OnCollision(GameObject obj)
        {
            
        }
    }
}
