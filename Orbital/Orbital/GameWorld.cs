using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Orbital
{
	public class GameWorld : Game
	{
		private static GraphicsDeviceManager myGraphics;
		private SpriteBatch mySpriteBatch;

		private readonly int screenHeight = 900;
		private readonly int screenWidth =1200;
		
		private int score;
		private SpriteFont File;


		private readonly List<GameObject> listOfCurrentObjects = new List<GameObject>();
		private readonly List<GameObject> listOfObjectsToAdd = new List<GameObject>();
		private readonly List<GameObject> listOfObjectsToDestroy = new List<GameObject>();

		private Texture2D collisionTexture;
		private Texture2D background;


		public static Vector2 ScreenSize { get; set; }
        public int Score { get => score; set => score = value; }

        public GameWorld()
		{
			myGraphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			myGraphics.PreferredBackBufferHeight = screenHeight;
			myGraphics.PreferredBackBufferWidth = screenWidth;
			myGraphics.IsFullScreen = false;
			myGraphics.ApplyChanges();

			ScreenSize = new Vector2(myGraphics.PreferredBackBufferWidth, myGraphics.PreferredBackBufferHeight);
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			Instantiate(new Player());
			Instantiate(new Spawner());
			
			base.Initialize();
		}

		protected override void LoadContent()
		{
			mySpriteBatch = new SpriteBatch(GraphicsDevice);

			collisionTexture = Content.Load<Texture2D>("CollisionTexture");

			background = Content.Load<Texture2D>("Background");
			File = Content.Load<SpriteFont>("File");

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			// TODO: Add your update logic here

			//player.Update(gameTime);

			foreach (GameObject obj in listOfCurrentObjects)
			{
				obj.Update(gameTime);

				foreach (GameObject other in listOfCurrentObjects)
				{
					if(obj != other)
					{
						obj.CheckCollision(other);
					}
				}

			}


			CallInstantiate();
			CallDestroy();
		}

		protected override void Draw(GameTime gameTime)
		{
			// TODO: Add your drawing code here

			mySpriteBatch.Begin(SpriteSortMode.FrontToBack);
			
			mySpriteBatch.Draw(background, new Rectangle(0,0,2000,2000), null, Color.White, 0, new Vector2(0,0), SpriteEffects.None, 0);
			mySpriteBatch.DrawString(File, "SCORE: " + score, new Vector2(0, 0), Color.Red);

			foreach (GameObject obj in listOfCurrentObjects)
			{
				obj.Draw(mySpriteBatch);

#if DEBUG
				DrawCollisionBox(obj);
#endif

			}

			mySpriteBatch.End();


			base.Draw(gameTime);
		}

		/// <summary>
		/// Moves our GameObject to our list of objects to add
		/// </summary>
		/// <param name="gameObject"></param>
		public void Instantiate(GameObject gameObject)
		{
			gameObject.SetGameWorld(this);
			listOfObjectsToAdd.Add(gameObject);
		}

		/// <summary>
		/// Moves our Gameobjects to our list of objects to destroy
		/// </summary>
		/// <param name="gameObject"></param>
		/// 
		public void DestroyGameObject(GameObject gameObject)
		{
			listOfObjectsToDestroy.Add(gameObject);
			if (gameObject is SmallAsteroid)
			{
				score += 5;
			}
			else if (gameObject is Asteroid)
			{
				score += 10;
			}
		}


		/// <summary>
		/// Checks if there are any objects to add from our add list
		/// If there is it loads their content and adds them to our current objects list
		/// </summary>
		private void CallInstantiate()
		{
			if (listOfObjectsToAdd.Count > 0)
			{
				foreach (GameObject addObj in listOfObjectsToAdd)
				{
					addObj.LoadContent(Content);
					listOfCurrentObjects.Add(addObj);
				}

				listOfObjectsToAdd.Clear();
			}
		}

		/// <summary>
		/// Checks if there are any objects to destroy from our destroy list
		/// if there is it removes them from our current objects list
		/// </summary>
		private void CallDestroy()
		{
			
			if(listOfObjectsToDestroy.Count > 0)
			{
				foreach (GameObject destroyObj in listOfObjectsToDestroy)
				{
					listOfCurrentObjects.Remove(destroyObj);
				}
			}
		}

		private void DrawCollisionBox(GameObject gameObject)
		{

			Rectangle topLine = new Rectangle(gameObject.Collision.X, gameObject.Collision.Y, gameObject.Collision.Width, 1);
			Rectangle bottomLine = new Rectangle(gameObject.Collision.X, gameObject.Collision.Y + gameObject.Collision.Height, gameObject.Collision.Width, 1);
			Rectangle rightLine = new Rectangle(gameObject.Collision.X + gameObject.Collision.Width, gameObject.Collision.Y, 1, gameObject.Collision.Height);
			Rectangle leftLine = new Rectangle(gameObject.Collision.X, gameObject.Collision.Y, 1, gameObject.Collision.Height);

			mySpriteBatch.Draw(collisionTexture, topLine, Color.Red);
			mySpriteBatch.Draw(collisionTexture, bottomLine, Color.Red);
			mySpriteBatch.Draw(collisionTexture, rightLine, Color.Red);
			mySpriteBatch.Draw(collisionTexture, leftLine, Color.Red);
		}



	}
}
