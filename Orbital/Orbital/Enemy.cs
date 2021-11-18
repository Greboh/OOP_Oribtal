using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Orbital
{
    class Enemy : GameObject
    {
        //enemy movement and attacks
        private bool movingRight = true;        // bool for checking if enemy is moving right or left
        private float timeSinceLastShot = 0f;
        private Vector2 shootingPoint;          // Vector2 for storing our shooting point
        private Texture2D healthbar;            // red healthbar sprite
        private Texture2D healthbarBorder;      // black border for healthbar
        private Rectangle healthRectangle;      // used as a sprite "mask" when subtracting width
        private Rectangle borderRectangle;
        private Vector2 healthbarOrigin;        // helps position the health bar below the enemy
        private SoundEffect enemyDestruction;   
        private SoundEffect enemyLaserSound;





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

            enemyDestruction = content.Load<SoundEffect>("Asterioid_destruction_sound");
            enemyLaserSound = content.Load<SoundEffect>("enemyLaser");


        }

        public override void Update(GameTime gameTime)
        {
            Animate(gameTime);
            HandleMovement(gameTime);
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
                    ScoreManager.UpdateScore(250); // updates score with set amount 
                    amountOfEnemies--;                   // removes 1 from total of enemies spawned
                    enemyDestruction.Play();
                    Destroy(this);
                }
                Console.WriteLine("Enemy health: " + this.health);  // outputs current enemy health to console

            }
        }

      

        private void ShipMovement()
        {

            if (movingRight)
            {
                //Sprite starts moving right adding to position with speed
                position.X += speed;

            }
            else
            {
                //When the sprite reaches end of width, the sprite is flipped and it starts subtracting speed from 
                position.X -= speed;
            }
            if (position.X > GameWorld.ScreenSize.X - this.sprite.Width || position.X < 0)
            {
                //When the sprite reaches end of width is changes movingRight bool to false
                movingRight = !movingRight;
            }



        }

        

        public override void Attack(GameTime gameTime)
        {
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds; // Gets the game time in seconds (Framerate independent)

            //checks if enough time has passed
            if (timeSinceLastShot > 0.7) 
            {
                // checks if player i moving right and changes laser accordingly
                // - not used, but can be used to change firerate, direction or amount depending on whitch way the ship is traveling.
                if (movingRight)
                {
                    Instantiate(new EnemyAttack(position, shootingPoint, this.rotation + 1.5f));
                    enemyLaserSound.Play();


                   
                    timeSinceLastShot = 0;
                }
                else
                {
                    Instantiate(new EnemyAttack(position, shootingPoint, this.rotation + 1.5f));
                    enemyLaserSound.Play();

                   
                    timeSinceLastShot = 0;

                }


            }




        }

       


    }
}


