﻿using Floor_Zero.Classes.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Floor_Zero.Classes.Managers
{
    public enum GameState
    {
        SplashScreen,
        StartMenu,
        GameScreen,
        PaintScreen
    }

    class Manager_GameState
    {
        // Set current and loaded GameState
        GameState currentGameState { get { return Game1.currentGameState; } set { Game1.currentGameState = value; } }
        GameState loadedGameState { get { return Game1.loadedGameState; } set { Game1.loadedGameState = value; } }

        // Game Screens (States)
        SplashScreen splashScreen = new SplashScreen();
        StartMenuScreen startMenuScreen = new StartMenuScreen();
        GameScreen gameScreen = new GameScreen();
        PaintScreen paintScreen;

        /// <summary>
        /// Initailize the Start Menu.
        /// </summary>
        public void Initialize(Game1 game1)
        {
            paintScreen = new PaintScreen(game1);
            splashScreen.Initialize();
        }

        /// <summary>
        /// Load Content for Start Menu.
        /// </summary>
        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            splashScreen.LoadContent(Content, graphicsDevice);
        }

        /// <summary>
        /// Unload Content for every state.
        /// </summary>
        public void UnloadContent()
        {
            splashScreen.UnloadContent();
        }

        /// <summary>
        /// Go through update logic of whichever state is active.
        /// </summary>
        public void Update(ContentManager Content, GraphicsDevice graphicsDevice, GameTime gameTime, Game game)
        {
            if (currentGameState == loadedGameState)
            {
                if (currentGameState == GameState.SplashScreen)
                {
                    splashScreen.Update(gameTime);
                }
                else if (currentGameState == GameState.StartMenu)
                {
                    startMenuScreen.Update();
                }
                else if (currentGameState == GameState.GameScreen)
                {
                    gameScreen.Update();
                }
                else if (currentGameState == GameState.PaintScreen)
                {
                    paintScreen.Update(gameTime);
                }
            }
            else
            {
                LoadGameState(Content, graphicsDevice, game);
            }
        }

        /// <summary>
        /// Go through Draw logic of whichever state is active.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (currentGameState == loadedGameState)
            {
                if (currentGameState == GameState.SplashScreen)
                {
                    splashScreen.Draw(spriteBatch);
                }
                else if (currentGameState == GameState.StartMenu)
                {
                    startMenuScreen.Draw(spriteBatch);
                }
                else if (currentGameState == GameState.GameScreen)
                {
                    gameScreen.Draw(spriteBatch, graphicsDevice);
                }
                else if (currentGameState == GameState.PaintScreen)
                {
                    paintScreen.Draw(spriteBatch, graphicsDevice);
                }
            }
        }

        /// <summary>
        /// Loads the current GameState.
        /// </summary>
        private void LoadGameState(ContentManager Content, GraphicsDevice graphicsDevice, Game game)
        {
            Content.Unload();
            if (currentGameState == GameState.SplashScreen)
            {
                splashScreen.Initialize();
                splashScreen.LoadContent(Content, graphicsDevice);
                loadedGameState = GameState.SplashScreen;
            }
            else if (currentGameState == GameState.StartMenu)
            {
                startMenuScreen.Initialize();
                startMenuScreen.LoadContent(Content, graphicsDevice);
                loadedGameState = GameState.StartMenu;
            }
            else if (currentGameState == GameState.GameScreen)
            {
                gameScreen.Initialize();
                gameScreen.LoadContent(Content);
                loadedGameState = GameState.GameScreen;
            }
            else if (currentGameState == GameState.PaintScreen)
            {
                paintScreen.Initialize();
                paintScreen.LoadContent(Content, graphicsDevice);
                loadedGameState = GameState.PaintScreen;
            }
        }
    }
}
