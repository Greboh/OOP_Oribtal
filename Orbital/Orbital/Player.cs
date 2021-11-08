using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Orbital
{
	class Player : GameObject
	{
        public Player()
        {
            speed = 150;
            velocity = Vector2.Zero;
            color = Color.White;
        }


        private void HandleInput()
        {
            velocity = Vector2.Zero;
            KeyboardState currentState = Keyboard.GetState();

            if (currentState.IsKeyDown(Keys.W) || currentState.IsKeyDown(Keys.Up))
            {
                velocity += new Vector2(0, -1);
            }
            if (currentState.IsKeyDown(Keys.S) || currentState.IsKeyDown(Keys.Down))
            {
                velocity += new Vector2(0, 1);
            }
            if (currentState.IsKeyDown(Keys.A) || currentState.IsKeyDown(Keys.Left))
            {
                velocity += new Vector2(-1, 0);
            }
            if (currentState.IsKeyDown(Keys.D) || currentState.IsKeyDown(Keys.Right))
            {
                velocity += new Vector2(1, 0);
            }


        }

        public override void LoadContent(ContentManager content)
        {
            
        }

        

        public override void Update(GameTime gameTime)
        {
            HandleInput();
            Move(gameTime);
            Animate(gameTime);

        }

        private void ScreenWarp()
        {
           
        }

        public override void OnCollision(GameObject obj)
        {
            
        }
    }
}
