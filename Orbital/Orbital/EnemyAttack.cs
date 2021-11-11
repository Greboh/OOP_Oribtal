﻿using Microsoft.Xna.Framework;
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
            this.scale = 1;
            this.animationFPS = 10;
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
            

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Attack(GameTime gameTime)
        {
        }

        public override void OnCollision(GameObject obj)
        {

        }

    }
}