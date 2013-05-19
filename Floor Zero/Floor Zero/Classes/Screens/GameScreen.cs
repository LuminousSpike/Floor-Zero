using Floor_Zero.Classes.Managers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Solar.Graphics.Cameras;

namespace Floor_Zero.Classes.Screens
{
    class GameScreen
    {
        // Managers
        Manager_Tile manager_Tile;

        public GameScreen()
        {
            // Managers
            manager_Tile = new Manager_Tile();
        }

        public void Initialize()
        {
            // Managers
            manager_Tile.Initialize();
        }

        public void LoadContent(ContentManager Content)
        {
            // Managers
            manager_Tile.LoadContent(Content, false);
        }

        public void UnloadContent()
        {
            // Managers
            manager_Tile.UnloadContent();
        }

        public void Update()
        {
            // Managers
            //manager_Tile.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Managers
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, Game1.SpriteScale);
            //manager_Tile.Draw(spriteBatch, );
            spriteBatch.End();
        }
    }
}
