using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Flood_Control_Reimplement
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Contents

        SpriteFont pericles36;

        Texture2D background;
        Texture2D pipeTileSheet;
        Texture2D titleScreen;

        #endregion

        #region Rectangles

        Rectangle wholeWindow;

        #endregion

        #region GameStates

        GameBoard gameBoard;

        int gameScore;

        enum GameState { TitleScreen, Playing, GameOver }
        GameState gameState = GameState.TitleScreen;

        #endregion

        #region Timers

        const float gameOverScreenSeconds = 5f;

        float gameOverScreenTimer = gameOverScreenSeconds;


        #endregion



        #region Helper Functions

        private void ResetGame()
        {
            gameScore = 0;
            if (gameBoard == null)
                gameBoard = new GameBoard();
            else
                gameBoard.ResetBoard();
        }

        #endregion


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            wholeWindow = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            pericles36 = Content.Load<SpriteFont>(@"Fonts/Pericles36");
            background = Content.Load<Texture2D>(@"Textures/Background");
            pipeTileSheet = Content.Load<Texture2D>(@"Textures/Tile_Sheet");
            titleScreen = Content.Load<Texture2D>(@"Textures/TitleScreen");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            switch(gameState)
            {
                case GameState.TitleScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        this.ResetGame();
                        gameState = GameState.Playing;
                    }
                    break;
                case GameState.Playing:
                    break;
                case GameState.Pause:
                    break;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch(gameState)
            {
                case GameState.TitleScreen:
                    spriteBatch.Draw(titleScreen, wholeWindow, Color.White);
                    break;
                case GameState.Playing:
                    spriteBatch.Draw(background, wholeWindow, Color.White);
                    

                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
