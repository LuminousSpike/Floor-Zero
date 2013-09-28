using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Solar.Graphics.Cameras;
using Solar.Graphics.Lighting.Top_Down;
using Ziggyware;

namespace Floor_Zero.Classes.Managers.Lighting
{
    public class Lighting_Manager
    {
        private readonly List<LightArea> dynamicLights = new List<LightArea>();
        private readonly GraphicsDevice graphicsDevice;
        private readonly QuadRenderComponent quadRender;
        private readonly List<LightArea> staticLights = new List<LightArea>();
        public ShadowmapResolver dynamicShadowmapResolver;
        public RenderTarget2D screenShadows;
        public ShadowmapResolver staticShadowmapResolver;

        public Lighting_Manager(Game1 game1)
        {
            quadRender = new QuadRenderComponent(game1);
            graphicsDevice = game1.GraphicsDevice;
            game1.Components.Add(quadRender);
        }

        public void Initialize()
        {
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            dynamicShadowmapResolver = new ShadowmapResolver(graphicsDevice, quadRender, ShadowmapSize.Size512,
                ShadowmapSize.Size512);
            dynamicShadowmapResolver.LoadContent(Content);

            staticShadowmapResolver = new ShadowmapResolver(graphicsDevice, quadRender, ShadowmapSize.Size1024,
                ShadowmapSize.Size1024);
            staticShadowmapResolver.LoadContent(Content);

            screenShadows = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width,
                graphicsDevice.Viewport.Height);
        }

        public void UnloadContent()
        {
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (LightArea light in staticLights)
            {
                spriteBatch.Draw(light.RenderTarget, light.LightPosition - light.LightAreaSize*0.5f, light.color*0.8f);
            }

            foreach (LightArea light in dynamicLights)
            {
                spriteBatch.Draw(light.RenderTarget, light.LightPosition - light.LightAreaSize*0.5f, Color.White*0.5f);
            }
        }

        public void AddStationaryLight(LightArea lightArea)
        {
            var rand = new Random();
            lightArea.color = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
            staticLights.Add(lightArea);
            //dynamicLights.Add(lightArea);
        }

        public void AddMoveableLight(LightArea lightArea)
        {
            dynamicLights.Add(lightArea);
        }

        private void ShadowDrawBegin(LightArea light)
        {
            light.BeginDrawingShadowCasters();
        }

        private void ShadowDrawEnd(LightArea light)
        {
            light.EndDrawingShadowCasters();
        }

        private void ResolveShadows(LightArea light, ShadowmapResolver shadowmapResolver)
        {
            shadowmapResolver.ResolveShadows(light.RenderTarget, light.RenderTarget, light.LightPosition);
        }

        public void DrawStaticShadows(Action<SpriteBatch, LightArea> drawSprites, SpriteBatch spriteBatch,
            BasicCamera2D camera)
        {
            foreach (LightArea light in staticLights)
            {
                ShadowDrawBegin(light);
                drawSprites(spriteBatch, light);
                ShadowDrawEnd(light);
                ResolveShadows(light, staticShadowmapResolver);
            }
        }

        public void DrawDynamicShadows(Action<SpriteBatch, LightArea> drawSprites, SpriteBatch spriteBatch,
            BasicCamera2D camera)
        {
            foreach (LightArea light in dynamicLights)
            {
                if (light.LightPosition.X > (camera.Pos.X - (graphicsDevice.Viewport.Width/2)) &&
                    light.LightPosition.X < (camera.Pos.X + (graphicsDevice.Viewport.Width/2))
                    && light.LightPosition.Y > (camera.Pos.Y - (graphicsDevice.Viewport.Height/2)) &&
                    light.LightPosition.Y < (camera.Pos.Y + graphicsDevice.Viewport.Height/2))
                {
                    ShadowDrawBegin(light);
                    drawSprites(spriteBatch, light);
                    ShadowDrawEnd(light);
                    ResolveShadows(light, dynamicShadowmapResolver);
                }
            }
        }
    }
}