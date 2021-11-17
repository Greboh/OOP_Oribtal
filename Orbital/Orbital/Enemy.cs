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
        //enemy movement and attacks
        bool movingRight = true;
        private float timeSinceLastShot = 0f;
        private Vector2 shootingPoint; // Vector2 for storing our shooting point
        private Texture2D healthbar;
        private Texture2D healthbarBorder;
        private Rectangle healthRectangle;
        private Rectangle borderRectangle;
        private Vector2 healthbarOrigin;


        // Fields for health and taking damage
        private Texture2D[] healthBars = new Texture2D[6];
        private Texture2D currentHealthBar;
        private int remainingHealth;

        public Enemy()
        {
            this.color = Color.White;
            this.scale = 1;
            this.speed = 3;
            this.animationFps = 5;
            this.health = 60;

        }
        public override void LoadContent(ContentManager content)
        {

            sprite = content.Load<Texture2D>("Ship2rotated");
            shootingPoint = new Vector2((sprite.Width / 2) - 30, (sprite.Height / 2) - 15);

            healthbar = content.Load<Texture2D>("healthbar_main_enemy");
            healthbarBorder = content.Load<Texture2D>("healthbar_border_enemy.png");

            healthRectangle = new Rectangle(0, 0,  60, healthbar.Height );
            borderRectangle = new Rectangle(0, 0, healthbarBorder.Width , healthbarBorder.Height);

            healthbarOrigin = new Vector2(healthbar.Width / 2 - 15, healthbar.Height / 2 - 90);

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
            spriteBatch.Draw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.FlipHorizontally, 0);

            spriteBatch.Draw(healthbarBorder, position, borderRectangle, color, rotation, healthbarOrigin, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(healthbar, position, healthRectangle, color, rotation, healthbarOrigin, scale, SpriteEffects.None, 0);


        }



        public override void OnCollision(GameObject obj)
        {

            if (obj is Laser)
            {

                this.health -= 20;           //   damage from hit
                healthRectangle.Width -= 20; // ^ lowering health bar accordingly

                Destroy(obj);
                if (this.health < 1)
                {
                    ScoreManager.UpdateScore(5);
                    amountOfEnemies--;
                    Destroy(this);
                }
                Console.WriteLine(this.health);

            }
        }

      

        private void ShipMovement()
        {

            if (movingRight)
            {
                position.X += speed;
                shipFlip = SpriteEffects.None;

            }
            else
            {
                position.X -= speed;
                shipFlip = SpriteEffects.FlipHorizontally;
            }
            if (position.X > GameWorld.ScreenSize.X - this.sprite.Width || position.X < 0)
            {
                movingRight = !movingRight;
            }



        }

        

        public override void Attack(GameTime gameTime)
        {
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds; // Gets the game time in seconds (Framerate independent)

            if (timeSinceLastShot > 0.7) // 
            {
                if (movingRight)
                {
                    Instantiate(new EnemyAttack(position, shootingPoint, this.rotation + 1.5f));
                   


                    //if (timeSinceLastShot > 0.7)
                    //{
                    //    Instantiate(new EnemyAttack(position, shootingPoint, this.rotation + 1.5f));


                    //    if (timeSinceLastShot > 0.7)
                    //    {
                    //        Instantiate(new EnemyAttack(position, shootingPoint, this.rotation + 2f));



                    //    }
                    //}
                    timeSinceLastShot = 0;
                }
                else
                {
                    Instantiate(new EnemyAttack(position, shootingPoint, this.rotation + 1.5f));

                    //if (timeSinceLastShot > 0.7)
                    //{
                    //    Instantiate(new EnemyAttack(position, shootingPoint, this.rotation + 1.6f));


                    //    if (timeSinceLastShot > 0.7)
                    //    {
                    //        Instantiate(new EnemyAttack(position, shootingPoint, this.rotation + 1.7f));


                    //    }
                    //}
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


