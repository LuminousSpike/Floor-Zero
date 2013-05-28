using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Solar.Graphics.Lighting.Top_Down;
using Ziggyware;
using System.Collections.Generic;

namespace Floor_Zero.Classes.Managers.Lighting
{
    public class Lighting_Manager
    {
        QuadRenderComponent quadRender;
        public ShadowmapResolver shadowmapResolver;
        RenderTarget2D screenShadows;

        List<LightArea> listLights = new List<LightArea>();

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
            shadowmapResolver = new ShadowmapResolver(graphicsDevice, quadRender, ShadowmapSize.Size2048, ShadowmapSize.Size1024);
            shadowmapResolver.LoadContent(Content);

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
            
        }

        public void AddLight(LightArea lightArea)
        {
            listLights.Add(lightArea);
        }

        public void ShadowDrawBegin()
        {
            foreach (LightArea light in listLights)
            {
                light.BeginDrawingShadowCasters();
            }
        }

        public void ShadowDrawEnd()
        {
            foreach (LightArea light in listLights)
            {
                light.EndDrawingShadowCasters();
            }
        }

        private void 
    }
}
