﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital
{
    class SmallAsteroid : GameObject
    {

        public SmallAsteroid(Vector2 position, float scale, Texture2D sprite)
        {
            this.position = position;
            this.scale = scale;
            this.sprite = sprite;
            this.color = Color.White;
            this.speed = 60;
            this.velocity = new Vector2(myRandom.Next(-2, +2), myRandom.Next(-2, +2));
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
        }

        public override void OnCollision(GameObject obj)
        {
            if (obj is Laser)
            {
                Destroy(this);
                Destroy(obj);
            }

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

        public override void Update(GameTime gametime)
        {
            HandleMovement(gametime);
            ScreenBound();
        }
    }
}