using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Solar.GUI;
using Solar.GUI.Controls;

namespace Floor_Zero.Classes.Managers.HUD.Game_Editor
{
    class Hud_Manager_GameEditor
    {
        GuiSystem hudSystem = new GuiSystem();

        public void Initialize()
        {
            hudSystem.Initialize();
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            hudSystem.LoadContent(Content, graphicsDevice);
        }

        public void UnloadContent()
        {
            hudSystem.UnloadContent();
        }

        public void Update()
        {
            hudSystem.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            hudSystem.Draw(spriteBatch);
        }
    }
}
