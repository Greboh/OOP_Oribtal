using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital
{
    public class EnemyAttack : GameObject
    {

        public EnemyAttack(Vector2 position, Vector2 shootingPoint, float rotation)
        {

            velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            this.speed = 1000; // Too low and it will cause a bug!
            this.position = position;
            this.origin = shootingPoint;
            this.layerDepth = 0.5f;
            this.scale = 3;
            this.animationFps = 10;
            this.rotation = rotation;
            this.color = Color.White;
        }

        public override void Update(GameTime gametime)
        {
            Animate(gametime);

            this.position += velocity;
            HandleMovement(gametime);

            if (position.X > GameWorld.ScreenSize.X || position.X < GameWorld.ScreenSize.X - GameWorld.ScreenSize.X ||
                position.Y > GameWorld.ScreenSize.Y || position.Y < GameWorld.ScreenSize.Y - GameWorld.ScreenSize.Y)
            {
                Destroy(this);
            }

        }

        public override void LoadContent(ContentManager content)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = content.Load<Texture2D>(i + 1 + "pShoot");
            }

            animationSprite = sprites[0];

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animationSprite, position, null, color, this.rotation, origin, scale, SpriteEffects.None, layerDepth);

        }

        public override void Attack(GameTime gameTime)
        {
        }

        public override void OnCollision(GameObject obj)
        {

            if (obj is Player)
            {
                // Lower player health here

                Destroy(this);
                Destroy(obj);

            }

        }

    }
}

