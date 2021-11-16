using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital
{
    class SmallAsteroid : GameObject
    {
        private SoundEffect asteroidDestruction;

        public SmallAsteroid(Vector2 position, float scale, Texture2D sprite)
        {
            this.position = position;
            this.scale = scale;
            this.sprite = sprite;
            this.color = Color.White;
            this.speed = 60;
            this.velocity = new Vector2(myRandom.Next(-2, +2), myRandom.Next(-2, +2));
            if (velocity.X == 0 || velocity.Y == 0)
            {
                velocity = new Vector2(1, 0);
            }
        }

        public override void Attack(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None, 0);
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("smallMeteor_0" + myRandom.Next(5, 7));
            asteroidDestruction = content.Load<SoundEffect>("Asterioid_destruction_sound");
        }

        public override void OnCollision(GameObject obj)
        {
            if (obj is Laser)
            {
                Destroy(this);
                Destroy(obj);
                asteroidDestruction.Play();

            }

        }
        private void ScreenBound()
        {
            if (position.X > GameWorld.ScreenSize.X || position.X <0)
            {
                Destroy(this);
            }
            else if (position.Y > GameWorld.ScreenSize.Y || position.Y < 0)
            {
                Destroy(this);
            }
        }

        public override void Update(GameTime gametime)
        {
            HandleMovement(gametime);
            ScreenBound();
        }
    }
}
