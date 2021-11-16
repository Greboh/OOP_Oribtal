using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Orbital
{

    class Enemy : GameObject
    {
        private Vector2 explosionPosition;
        private Vector2 shootingPoint;

        bool movingRight = true;

        private float timeSinceLastShot = 0f;

        int enemyHealth = 2;


        public Enemy()
        {
            this.color = Color.White;
            this.scale = 1;
            this.speed = 3;
            this.animationFPS = 5;
            //this.rotation = 1.6f;
            

        }
        public override void LoadContent(ContentManager content)
        {

            sprite = content.Load<Texture2D>("Ship2fixed");
            explosionPosition = new Vector2(sprite.Width / 2 - 5, (sprite.Height / 2 + 25));

            for (int i = 0; i < sprites2.Length; i++)
            {
                sprites2[i] = content.Load<Texture2D>("Ship2_Explosion_00" + i);
                Console.WriteLine(sprites2[i]);

            }

            animationSprite2 = sprites2[0];

        }

        public override void Update(GameTime gameTime)
        {
            Animate(gameTime);
            HandleMovement(gameTime);
            ScreenBound();
            ShipMovement();
            Attack(gameTime);

            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, shipFlip, 0);
            spriteBatch.Draw(animationSprite2, position, null, color, rotation, explosionPosition, scale, shipFlip, 0);

        }

        public override void OnCollision(GameObject obj)
        {

            if (obj is Laser)
            {
                enemyHealth--;
                if (enemyHealth == 0)
                {

                    



                    //Destroy(this);
                    //Destroy(obj);
                    Console.WriteLine("Enemy ship destroyed");
                }
                    
                
                Console.WriteLine(enemyHealth);

            }
        }

        public void Explode(GameTime gameTime)
        {
            
           
        }

        private void ShipMovement()
        {

            if (movingRight)
            {
                position.X += speed;
                shipFlip = SpriteEffects.None;
                shootingPoint = new Vector2(-10, 40);

            }
            else
            {
                position.X -= speed;
                shipFlip = SpriteEffects.FlipHorizontally;
                shootingPoint = new Vector2(-10, 15);

            }
            if (position.X > GameWorld.ScreenSize.X - this.sprite.Width || position.X < 0)
            {
                movingRight = !movingRight;
            }
                

            //Console.WriteLine(position.X);
            //Console.WriteLine(movingRight);
            //Console.WriteLine(GameWorld.ScreenSize.X);
        }

        public override void Attack(GameTime gameTime)
        {
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds; // Gets the game time in seconds (Framerate independent)

            if (timeSinceLastShot > 0.7) // 
            {
                if (movingRight)
                {
                    Instantiate(new EnemyAttack(position, shootingPoint, this.rotation + 1.6f));
                    timeSinceLastShot = 0;
                }
                else
                {
                    Instantiate(new EnemyAttack(position, shootingPoint, this.rotation + 1.6f));
                    timeSinceLastShot = 0;
                }
                    

            }




        }

        private void ScreenBound()
        {
            //if (position.Y > GameWorld.ScreenSize.X)
            //{
            //    Destroy(this);
            //}
            //else if (position.X > GameWorld.ScreenSize.Y)
            //{
            //    Destroy(this);
            //}
        }

        
    }
}


