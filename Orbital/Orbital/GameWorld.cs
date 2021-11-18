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
	/// <summary>
	/// Enum used to define GameStates
	/// </summary>
	public enum Gamestate
	{
		Menu,
		Ingame,
		DeathScreen,
	}

	public class GameWorld : Game
	{
		#region Fields

		private static GraphicsDeviceManager myGraphics;
		private SpriteBatch mySpriteBatch;

		private readonly List<GameObject> listOfCurrentObjects = new List<GameObject>(); // List of objects in the game
		private readonly List<GameObject> listOfObjectsToAdd = new List<GameObject>(); // List of objects to add to the game
		private readonly List<GameObject> listOfObjectsToDestroy = new List<GameObject>(); // List of objects to remove from the game

		public Gamestate currentGameState; // Stores what GameState we are currently in

		private Texture2D collisionTexture;
		private Texture2D background; // Background for when you are ingame
		private Texture2D menu; // Background for when you are in the menu
		private Texture2D deathScreen; //B ackground for when you die
		
		private SpriteFont text; // Spritefont used for score text
		private SpriteFont highScoreFont; // Spritefont used for the highscore text

		private Song backgroundMusic; // Soundtrack for game

		private Color scoreColor = new Color(169, 169, 169, 100); //Color used for all Text

		private static int score; // Keeps track of the score
		private int highScore = ScoreManager.ReadTxt(); // Keeps track of the highscore

		private readonly int screenHeight = 900;
		private readonly int screenWidth =1200;

		#endregion

		#region Properties
		public static Vector2 ScreenSize { get; set; } //Property for setting the ScreenSize for public use
        public static int Score { get => score; set => score = value; } //Property for setting the score for public use

		#endregion
		public GameWorld()
		{
			myGraphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			myGraphics.PreferredBackBufferHeight = screenHeight;
			myGraphics.PreferredBackBufferWidth = screenWidth;
			myGraphics.IsFullScreen = false;
			myGraphics.ApplyChanges();

			ScreenSize = new Vector2(myGraphics.PreferredBackBufferWidth, myGraphics.PreferredBackBufferHeight); // Saves the Screensize
		}

		#region Methods
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

			backgroundMusic = Content.Load<Song>("Orbital.Soundtrack"); //Loads in the banger soundtrack
			MediaPlayer.Play(backgroundMusic); //soundtrack is played during



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
			mySpriteBatch.Begin(SpriteSortMode.FrontToBack); //Makes sure it uses the layerDepth

			if (currentGameState == Gamestate.Menu)
			{
				// Makes sure that the the array of scores has something in it
				// Makes sure that the max value in the array is not lower than the highscore
				if (ScoreManager.ListOfScores.Count != 0 && ScoreManager.ListOfScores.Max() > highScore)
				{
					highScore = ScoreManager.ListOfScores.Max(); // Sets the highscore to the max value in the array
				}

				mySpriteBatch.Draw(menu, new Rectangle(0, -10, (0 + (int) ScreenSize.X), (35 + (int)ScreenSize.Y)), Color.White);
				mySpriteBatch.DrawString(highScoreFont, highScore.ToString(), new Vector2(450, 650), scoreColor);


			}
			else if (currentGameState == Gamestate.Ingame)
			{
				mySpriteBatch.Draw(background, new Rectangle(0, 0, 2000, 2000), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
				mySpriteBatch.DrawString(text, "SCORE: " + score, new Vector2(0, 0), scoreColor);

				// Runs through our entire list of currentObjects 
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
				ScoreManager.SaveScore(score); // Adds our current score to the array of scores

				mySpriteBatch.Draw(deathScreen, new Rectangle(0, 0, 1200, 900), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
				mySpriteBatch.DrawString(text, score.ToString(), new Vector2(285, 645), scoreColor, 0, Vector2.Zero, 2.5f, SpriteEffects.None, 1);
				mySpriteBatch.DrawString(text, highScore.ToString(), new Vector2(990, 645), scoreColor, 0, Vector2.Zero, 2.5f, SpriteEffects.None, 1);
			}

			mySpriteBatch.End();
			
			base.Draw(gameTime);
		}

		/// <summary>
		/// Moves our GameObject to our list of objects to add
		/// </summary>
		/// <param name="gameObject">The object we want to instantiate</param>
		public void Instantiate(GameObject gameObject)
		{
			gameObject.SetGameWorld(this); // Sets our GameWorld to this GameWorld
			listOfObjectsToAdd.Add(gameObject); // Adds the GameObject to the list
		}

		/// <summary>
		/// Moves our Gameobjects to our list of objects to destroy
		/// </summary>
		/// <param name="gameObject">The object we want to destroy</param>
		public void DestroyGameObject(GameObject gameObject)
		{
			listOfObjectsToDestroy.Add(gameObject); // Adds the GameObject to the list
		}

		/// <summary>
		/// If we are ingame then update all GameObjects in listOfCurrentObjects
		/// </summary>
		/// <param name="gameTime"></param>
		private void UpdateGameObjects(GameTime gameTime)
		{
			if (currentGameState == Gamestate.Ingame)
			{
				// Updates all all of the GameObjects in the list of current objects
				foreach (GameObject obj in listOfCurrentObjects)
				{
					obj.Update(gameTime);
					
					// Checks for collision in all the objects in the list of current objects
					foreach (GameObject other in listOfCurrentObjects)
					{
						// if the our GameObject is not equal to the collision object 
						if (obj != other)
						{
							obj.CheckCollision(other);
						}
					}
				}
			}
		}

		/// <summary>
		/// Enables the user to navigate the menu
		/// </summary>
		private void MenuControls()
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				ScoreManager.SaveToTxt(); // Save the highscore to a txt file
				Exit(); // Exit the game
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.Space) && currentGameState == Gamestate.Menu)
			{
				currentGameState = Gamestate.Ingame;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Enter) && currentGameState == Gamestate.DeathScreen)
			{
				score = 0; // Resets the score
				listOfCurrentObjects.Clear(); // Clears our list of current objects 
				Instantiate(new Player()); // Spawn a new player
				Instantiate(new Spawner()); // Spawn a new spawner
				currentGameState = Gamestate.Menu;
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
					addObj.LoadContent(Content); // Loads the content we wish to spawn in the game
					listOfCurrentObjects.Add(addObj); // Adds them to the list of current objects in the game
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
					listOfCurrentObjects.Remove(destroyObj); // Remove the objects we wish to remove from the game
				}
			}
		}

		/// <summary>
		/// Draws a collision Box
		/// </summary>
		/// <param name="gameObject">The Gameobject we wish to draw a collision box around</param>
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

		#endregion
	}
}
