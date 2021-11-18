using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Orbital
{
	public abstract class GameObject
	{
        #region Fields
		// Texture 2D
        protected Texture2D sprite; // Default sprite value
		protected Texture2D animationSprite; // Sprite that uses animations
		protected Texture2D deathSprite; // Sprite that uses death animations
		protected Texture2D[] exhaustSprites = new Texture2D[4]; // Array to hold exhaust sprites
		protected Texture2D[] deathSprites = new Texture2D[10]; // Array to hold death sprites

		// Vector2
		protected Vector2 position; // Stores position of the GameObject
		protected Vector2 velocity; // Stores the velocity of the GameObject
		protected Vector2 offset; // Stores the offset used to spawn the GameObject
		protected Vector2 origin; // Stores the origin position of the GameObject

		//Color
		protected Color color; //Stores the color value of the GameObject

		//Float
		protected float rotation; // Stores the rotation of the GameObject
		protected float scale; // Stores the scale of the Gameobject 
		protected float speed; // Stores the speed of the GameObject
		protected float animationFps; // Stores the amount of FPS the animations have
		protected float layerDepth; // Stores the layerdepth that the GameObject's content should draw at
		private float exhaustTimeElapsed; // Stores the time in order to load sprites from the exhaustSprites[]
		private float deathTimeElapsed; // Stores the time in order to load sprites from the deathSprites[]

		//Int
		protected int health; // Stores the health of the GameObject
		protected int amountOfEnemies; // Stores the number of enemies currently present
		private int exhaustCurrentIndex; // Stores the value of animationFps * exhaustTimeElapsed used to draw a new sprite at a specific time
		private int deathCurrentIndex; // Stores the value of animationFps * deahtTimeElapsed used to draw a new sprite at a specific time

	

		protected GameWorld myGameWorld; // Stores the GameWorld in a variable for easier access

		protected Random myRandom = new Random(); // Creates a variable which is used for getting a random number between x and y
        #endregion

		// Creates a rectange that gets the sprite and checks its width and height and stores it in a new rectangle
        public Rectangle Collision
		{
			get
			{
				return new Rectangle
					(
					(int)(position.X + offset.Y), // GameObjects position + GameObjects offset on the Y-Axis
					(int)(position.Y + offset.X), // GameObjects position + Gameobjects offset on the X-Axis
					sprite != null ? sprite.Width : 0, // If the sprite is not null then get the sprite witdh otherwise the width is set to 0
					sprite != null ? sprite.Height : 0 // If the sprite is not null then get the sprite height otherwise the height is set to 0
					);
			}
		}

		// Used to load content in all subclasses. Abstract since we only need to use it in the subclasses
		public abstract void LoadContent(ContentManager content);

		// Used to update in all subclasses. Abstract since we only need to use it in the subclasses
		public abstract void Update(GameTime gameTime);

		// Used to Attack in all subclasses. Abstract since we only need to use it in the subclasses
		public abstract void Attack(GameTime gameTime);

		// Used to Draw in all subclasses. Abstract since we only need to use it in the subclasses
		public abstract void Draw(SpriteBatch spriteBatch);

		// Used to tell what happends when the GameObjects collides. Abstract since we only need to use it in the subclasses
		public abstract void OnCollision(GameObject obj);

		// Used to move the GameObjects. Uses the GameObject's velocity and speed
		protected void HandleMovement(GameTime gameTime)
		{
			float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; // Gets the GameTime in whole seconds
			position += ((velocity * speed) * deltaTime); // Makes the GameObject's move by manipulating their position
		}

		// Used to Animate our sprites
		protected void Animate(GameTime gameTime)
		{
			exhaustTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds; // Gets the time in total seconds used to animate exhaust
			deathTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds; 

			exhaustCurrentIndex = (int)(exhaustTimeElapsed * animationFps); // Uses our animationfps variable in order to control how fast the sprites change
			deathCurrentIndex = (int)(deathTimeElapsed * animationFps);

			animationSprite = exhaustSprites[exhaustCurrentIndex]; // Sets our animationSprite to ExhaustSprites at an index of the animationrate
			deathSprite = deathSprites[deathCurrentIndex];

			// Makes sure that our index cannot exceed the arrays length
			if (deathCurrentIndex >= deathSprites.Length - 1)
			{
				deathTimeElapsed = 0; // Resets TimeElapsed 
				deathCurrentIndex = 0; // Resets the current index
			}
			if (exhaustCurrentIndex >= exhaustSprites.Length - 1)
			{
				exhaustTimeElapsed = 0;
				exhaustCurrentIndex = 0;
			}


		}

		// Used to check if a GameObject collides with anything
		public void CheckCollision(GameObject obj)
		{
			if (Collision.Intersects(obj.Collision))
			{
				OnCollision(obj); // Calls the GameObjects's OnCollision Method
			}
		}

		// Used to set our gameworld in the GameWorld class
		public void SetGameWorld(GameWorld gameWorld)
		{
			myGameWorld = gameWorld; // Takes our variable myGameWorld and applies it to the method parameter
		}

		// Used to instantiate an object into our Gameworld
		// In reality it adds them to our list of objects to add in the GameWorld Class
		public void Instantiate(GameObject gameObject)
		{
			myGameWorld.Instantiate(gameObject);
		}

		// Used to destroy an object in our GameWorld
		// In reality it adds them to our list of objects to destroy in the GameWorld Class
		public void Destroy(GameObject gameObject)
		{
			myGameWorld.DestroyGameObject(gameObject);
		}


		

	}
}
