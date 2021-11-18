using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Orbital
{
	public enum Gamestate
	{
		Menu,
		Ingame,
		DeathScreen,
	}

	public class GameWorld : Game
	{
		private static GraphicsDeviceManager myGraphics;
		private SpriteBatch mySpriteBatch;

		private readonly int screenHeight = 900;
		private readonly int screenWidth =1200;


		private readonly List<GameObject> listOfCurrentObjects = new List<GameObject>();
		private readonly List<GameObject> listOfObjectsToAdd = new List<GameObject>();
		private readonly List<GameObject> listOfObjectsToDestroy = new List<GameObject>();

		private Texture2D collisionTexture;
		private Texture2D background;
		private Texture2D menu;
		private Texture2D deathScreen;
		private Song backgroundMusic;
		

		private static int score;
		private int highScore = ScoreManager.ReadTxt();
		private SpriteFont text;
		private SpriteFont highScoreFont;
		public Gamestate currentGameState;

		private Color scoreColor = new Color(169, 169, 169, 100);

		// Properties

		public static Vector2 ScreenSize { get; set; }
        public static int Score { get => score; set => score = value; }




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
			Console.WriteLine(ScoreManager.filePath);


		}

		protected override void Initialize()
		{
			Instantiate(new Player());
			Instantiate(new Spawner());

			base.Initialize();
			currentGameState = Gamestate.Menu;
			ScoreManager.ReadTxt();

		}

		protected override void LoadContent()
		{
			mySpriteBatch = new SpriteBatch(GraphicsDevice);

			collisionTexture = Content.Load<Texture2D>("CollisionTexture");

			background = Content.Load<Texture2D>("Background");
			menu = Content.Load<Texture2D>("menu");
			deathScreen = Content.Load<Texture2D>("DeathScreen");

			text = Content.Load<SpriteFont>("File");
			highScoreFont = Content.Load<SpriteFont>("HighScoreFont");

			backgroundMusic = Content.Load<Song>("Orbital.Soundtrack");
			MediaPlayer.Play(backgroundMusic);



			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			MenuControls();
			UpdateGameObjects(gameTime);

			CallInstantiate();
			CallDestroy();
		}


		protected override void Draw(GameTime gameTime)
		{
			mySpriteBatch.Begin(SpriteSortMode.FrontToBack);

			if (currentGameState == Gamestate.Menu)
			{
				if (ScoreManager.ListOfScores.Count != 0 && ScoreManager.ListOfScores.Max() != highScore)
				{
					if (ScoreManager.ListOfScores.Max() > highScore)
					{
						highScore = ScoreManager.ListOfScores.Max();
					}

				}

				mySpriteBatch.Draw(menu, new Rectangle(0, -10, (0 + (int) ScreenSize.X), (35 + (int)ScreenSize.Y)), Color.White);
				mySpriteBatch.DrawString(highScoreFont, highScore.ToString(), new Vector2(450, 650), scoreColor);


			}
			else if (currentGameState == Gamestate.Ingame)
			{
				mySpriteBatch.Draw(background, new Rectangle(0, 0, 2000, 2000), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
				mySpriteBatch.DrawString(text, "SCORE: " + score, new Vector2(0, 0), scoreColor);

				foreach (GameObject obj in listOfCurrentObjects)
				{
					obj.Draw(mySpriteBatch);

#if DEBUG
					DrawCollisionBox(obj);
#endif

				}
			}
			else if (currentGameState == Gamestate.DeathScreen)
			{
				ScoreManager.SaveScore(score);


				mySpriteBatch.Draw(deathScreen, new Rectangle(0, 0, 1200, 900), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
				mySpriteBatch.DrawString(text, score.ToString(), new Vector2(285, 645), scoreColor, 0, Vector2.Zero, 2.5f, SpriteEffects.None, 1);
				mySpriteBatch.DrawString(text, highScore.ToString(), new Vector2(990, 645), scoreColor, 0, Vector2.Zero, 2.5f, SpriteEffects.None, 1);


				if (Keyboard.GetState().IsKeyDown(Keys.Enter))
				{
					score = 0;
					listOfCurrentObjects.Clear();
					Instantiate(new Player());
					Instantiate(new Spawner());
					currentGameState = Gamestate.Menu;

				}
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
		}

		/// <summary>
		/// If we are ingame then update all GameObjects in listOfCurrentObjects
		/// </summary>
		/// <param name="gameTime"></param>
		private void UpdateGameObjects(GameTime gameTime)
		{
			if (currentGameState == Gamestate.Ingame)
			{
				foreach (GameObject obj in listOfCurrentObjects)
				{
					obj.Update(gameTime);

					foreach (GameObject other in listOfCurrentObjects)
					{
						if (obj != other)
						{
							obj.CheckCollision(other);
						}
					}
				}
			}
		}

		/// <summary>
		/// Checks for user input to navigate the menu
		/// </summary>
		private void MenuControls()
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				ScoreManager.SaveToTxt();
				Exit();
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.Space) && currentGameState == Gamestate.Menu)
			{
				currentGameState = Gamestate.Ingame;
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
