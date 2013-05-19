using Floor_Zero.Classes.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solar.Graphics.Cameras;
using Floor_Zero.Classes.Managers.HUD.Game_Editor;

namespace Floor_Zero.Classes.Screens
{
    class PaintScreen
    {
        // Managers
        Manager_Tile manager_Tile;
        BasicCamera2D camera = new BasicCamera2D();
        Hud_Manager_GameEditor manager_Hud = new Hud_Manager_GameEditor();

        public PaintScreen()
        {
            // Managers
            manager_Tile = new Manager_Tile();
        }

        public void Initialize(Game game)
        {
            // Managers
            manager_Tile.Initialize();
            manager_Hud.Initialize();
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            // Managers
            manager_Tile.LoadContent(Content, true);
            manager_Hud.LoadContent(Content, graphicsDevice);
        }

        public void UnloadContent()
        {
            // Managers
            manager_Tile.UnloadContent();
            manager_Hud.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            // Managers
            manager_Tile.Update(camera);
            manager_Hud.Update();
            UpdateCamera();

        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            // Managers
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, camera.get_transformation(graphicsDevice));
            manager_Tile.Draw(spriteBatch, camera);
            spriteBatch.End();

            spriteBatch.Begin();
            manager_Hud.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void UpdateCamera()
        {
            int speed = 30 + (int)(-camera.Zoom * 10);
            if (speed < 1) speed = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                camera.Move(new Vector2(0, -speed));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                camera.Move(new Vector2(0, speed));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                camera.Move(new Vector2(-speed, 0));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                camera.Move(new Vector2(speed, 0));
            }

            camera.Update(Game1.mouseState);
            
        }
    }
}
