using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Solar.GUI;
using Solar.GUI.Controls;

namespace Floor_Zero.Classes.Managers.HUD.Game_Editor
{
    class Hud_Manager_GameEditor
    {
        GuiSystem hudSystem = new GuiSystem();
        Box box1;

        public void Initialize()
        {
            hudSystem.Initialize();
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            hudSystem.LoadContent(Content, graphicsDevice);
            box1 = new Box(new Vector2(10, 10), 200, 600, 1, Color.Gray, Color.Black, graphicsDevice);

            hudSystem.Add(box1);
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
