using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solar.Graphics.Lighting.Top_Down;
using Ziggyware;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Floor_Zero.Classes.Managers.Lighting
{
    public class Lighting_Manager
    {
        QuadRenderComponent quadRender;
        public ShadowmapResolver shadowmapResolver;
        public LightArea lightArea1;
        RenderTarget2D screenShadows;

        public Lighting_Manager(Game1 game1)
        {
            this.quadRender = new QuadRenderComponent(game1);
            game1.Components.Add(quadRender);
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            shadowmapResolver = new ShadowmapResolver(graphicsDevice, quadRender, ShadowmapSize.Size2048, ShadowmapSize.Size2048);
            shadowmapResolver.LoadContent(Content);
            lightArea1 = new LightArea(graphicsDevice, ShadowmapSize.Size2048);
            screenShadows = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }

        public void UnloadContent()
        {

        }

        public void Update()
        {

        }

        public void Draw()
        {
            lightArea1.LightPosition = new Vector2(Game1.mouseState.X, Game1.mouseState.Y);
            
        }
    }
}
