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

		
		private int screenHeight = 900;
		private int screenWidth =1200;


		private List<GameObject> listOfCurrentObjects = new List<GameObject>();
		private List<GameObject> listOfObjectsToAdd = new List<GameObject>();
		private List<GameObject> listOfObjectsToDestroy = new List<GameObject>();

		public static int ScreenHeight
		{
			get { return myGraphics.PreferredBackBufferHeight; }
			set { myGraphics.PreferredBackBufferHeight = value; }
		}		
		public static int ScreenWidth
		{
			get { return myGraphics.PreferredBackBufferWidth; }
			set { myGraphics.PreferredBackBufferWidth = value; }
		}



		public GameWorld()
		{
			myGraphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			myGraphics.PreferredBackBufferHeight = screenHeight;
            myGraphics.PreferredBackBufferWidth = screenWidth;
			myGraphics.IsFullScreen = false;
			myGraphics.ApplyChanges();

			




			base.Initialize();
		}

		protected override void LoadContent()
		{
			mySpriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			// TODO: Add your update logic here


			CallInstantiate();
			CallDestroy();
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here

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
		}


		/// <summary>
		/// Checks if there is any objects to add from our add list
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
		/// Checks if there is any objects to destroy from our destroy list
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
		



	}
}
