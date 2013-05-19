using Floor_Zero.Classes.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solar.Graphics.Cameras;

namespace Floor_Zero.Classes.Screens
{
    class PaintScreen
    {
        // Managers
        Manager_Tile manager_Tile;
        BasicCamera2D camera = new BasicCamera2D();
        

        public PaintScreen()
        {
            // Managers
            manager_Tile = new Manager_Tile();
        }

        public void Initialize(Game game)
        {
            // Managers
            manager_Tile.Initialize();
            
        }

        public void LoadContent(ContentManager Content)
        {
            // Managers
            manager_Tile.LoadContent(Content, true);
        }

        public void UnloadContent()
        {
            // Managers
            manager_Tile.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            // Managers
            manager_Tile.Update(camera);
            UpdateCamera();

        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            // Managers
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, camera.get_transformation(graphicsDevice));
            manager_Tile.Draw(spriteBatch, camera);
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
