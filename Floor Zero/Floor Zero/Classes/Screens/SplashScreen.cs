using Floor_Zero.Classes.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Solar;

namespace Floor_Zero.Classes.Screens
{
    internal class SplashScreen
    {
        private Texture2D bituserLogo;
        private int imageIndex;
        private Texture2D myLogo;
        private float pastTime;
        private Timer splashScreenTimer;

        public void Initialize()
        {
            splashScreenTimer = new Timer(5);
            splashScreenTimer.Start();
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            bituserLogo = Content.Load<Texture2D>(@"Splash\Bituser Logo");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (splashScreenTimer.Update((float) gameTime.TotalGameTime.TotalSeconds - pastTime))
            {
                Game1.currentGameState = GameState.StartMenu;
            }

            if (splashScreenTimer.Elapsed >= 5)
            {
                imageIndex = 1;
            }

            pastTime = (float) gameTime.TotalGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (imageIndex == 0)
            {
                spriteBatch.Draw(bituserLogo, new Vector2(0, 0), Color.White);
            }
            spriteBatch.End();
        }
    }
}