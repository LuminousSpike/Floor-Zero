using Floor_Zero.Classes.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solar.GUI;
using Solar.GUI.Controls;

namespace Floor_Zero.Classes.Screens
{
    class StartMenuScreen
    {
        GuiSystem guiSystem = new GuiSystem();
        string buttonMainTexturePath = @"Button", buttonSelectedTexturePath = @"Button Selected", fontPath = @"font";

        Button playButton;
        Button paintButton;

        public void Initialize()
        {
            guiSystem.Initialize();

            // Create and Add buttons
            playButton = new Button(new Vector2(175, 50), "Play", buttonMainTexturePath, buttonSelectedTexturePath, fontPath);
            paintButton = new Button(new Vector2(175, 100), "Paint", buttonMainTexturePath, buttonSelectedTexturePath, fontPath);

            guiSystem.Add(playButton);
            guiSystem.Add(paintButton);
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            guiSystem.LoadContent(Content, graphicsDevice);
        }

        public void UnloadContent()
        {
            guiSystem.UnloadContent();
        }

        public void Update()
        {
            guiSystem.Update(Game1.mouseState);

            if (playButton.IsSelected && Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Game1.currentGameState = GameState.GameScreen;
            }
            else if (paintButton.IsSelected && Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Game1.currentGameState = GameState.PaintScreen;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            guiSystem.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
