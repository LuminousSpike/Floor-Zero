using Floor_Zero.Classes.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solar.Graphics.Cameras;
using Solar.Input;

namespace Floor_Zero.Classes.Screens
{
    internal class GameScreen
    {
        // Managers
        private readonly BasicCamera2D camera = new BasicCamera2D();
        private ManagerTile manager_Tile;

        public void Initialize()
        {
            // Managers
            //manager_Tile.Initialize(false);
        }

        public void LoadContent(ContentManager Content)
        {
            // Managers
            manager_Tile.LoadContent(Content);
        }

        public void UnloadContent()
        {
            // Managers
            manager_Tile.UnloadContent();
        }

        public void Update()
        {
            // Managers
            UpdateCamera();
            manager_Tile.Update(camera);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            // Managers
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, camera.get_transformation(graphicsDevice));
            //manager_Tile.Draw(spriteBatch, camera);
            spriteBatch.End();
        }

        private void UpdateCamera()
        {
            int speed = 30 + (int) (-camera.Zoom*10);
            if (speed < 1) speed = 1;
            if (InputHelper.InputDown(Keys.W, Buttons.DPadUp))
            {
                camera.Move(new Vector2(0, -speed));
            }
            if (InputHelper.InputDown(Keys.S, Buttons.DPadDown))
            {
                camera.Move(new Vector2(0, speed));
            }
            if (InputHelper.InputDown(Keys.A, Buttons.DPadLeft))
            {
                camera.Move(new Vector2(-speed, 0));
            }
            if (InputHelper.InputDown(Keys.D, Buttons.DPadRight))
            {
                camera.Move(new Vector2(speed, 0));
            }

            camera.Update(Game1.mouseState);
        }
    }
}