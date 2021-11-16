using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Orbital
{
	//class Enemy : GameObject
	//{

        //Random myRandom = new Random();


        //public EnemyShip()
        //{
        //    int x = myRandom.Next(25, 1895);
        //    int y = -100;
        //    position = new Vector2(x, y);

        //    velocity.Y = 1;
        //    speed = myRandom.Next(500, 1000);


        //}

        //public override void LoadContent(ContentManager content)
        //{
        //    exhaustSprites = new Texture2D[4];

        //    exhaustSprites[0] = content.Load<Texture2D>("enemyBlack1");
        //    exhaustSprites[1] = content.Load<Texture2D>("enemyBlue1");
        //    exhaustSprites[2] = content.Load<Texture2D>("enemyGreen1");
        //    exhaustSprites[3] = content.Load<Texture2D>("enemyRed1");
                


        //}

        //public void Respawn()
        //{
        //    int index = myRandom.Next(0, 4);
        //    sprite = exhaustSprites[index];
        //    velocity = new Vector2(0, 1);
        //    speed = myRandom.Next(50, 150);
        //    position.X = myRandom.Next(0, (int)GameWorld.Screensize.X - sprite.Width);
        //    position.Y = 0 - sprite.Height;


        //}

        //public override void Update(GameTime gametime)
        //{
        //    Move(gametime);

        //    if (position.Y > GameWorld.Screensize.Y)
        //    {
        //        Respawn();
        //    }

        //}

        //public override void OnCollision(GameObject obj)
        //{

            

        //}

    //}
}
