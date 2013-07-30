using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Solar.Graphics.Sprites;
using System;
using Solar.Graphics.Cameras;
using Floor_Zero.Classes.Entities;

namespace Floor_Zero.Classes.Managers.Lighting
{
    public class Lighting_Manager
    {
        public RenderTarget2D screenShadows;
        
        private GraphicsDevice graphicsDevice;
        private List<LightSource> Lights = new List<LightSource>();
        private int lightingMapX, lightingMapY;
        private float[,] lightingMap;

        public Lighting_Manager(Game1 game1)
        {
            this.graphicsDevice = game1.GraphicsDevice;
        }

        public void Initialize(int mapSizeX, int mapSizeY)
        {

        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {

        }

        public void UnloadContent()
        {

        }

        public void Update(LightSource light)
        {
            CalculateDynamicLightingMap(light);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void CreateLightingMap(int x, int y)
        {
            lightingMapX = x;
            lightingMapY = y;
            lightingMap = new float[x, y];
        }

        public void AddLight(LightSource light)
        {
            Lights.Add(light);
            if (light.IsDynamic)
            {
                CalculateDynamicLightingMap(light);
            }
            else
            {
                CalculateLightingMap(light);
            }
        }

        private void CalculateLightingMap(LightSource light)
        {
            // Needs a bounding rectangle to only calculate for near lights?
            for (int x = 0; x < lightingMapX; x++)
            {
                for (int y = 0; y < lightingMapY; y++)
                {
                    lightingMap[x, y] += CalculateLightValue(new Vector2(x * 64, y * 64), light);
                }
            }
        }

        private void CalculateDynamicLightingMap(LightSource light, bool forced = false)
        {
            // Needs a bounding rectangle to only calculate for near lights?
            for (int x = InsideBounds((int)light.GetPosition.X / 64 - (int)light.LightArea, lightingMapX); x < InsideBounds((int)light.GetPosition.X / 64 + (int)light.LightArea, lightingMapX); x++)
            {
                for (int y = InsideBounds((int)light.GetPosition.Y / 64 - (int)light.LightArea, lightingMapY); y < InsideBounds((int)light.GetPosition.Y / 64 + (int)light.LightArea, lightingMapY); y++)
                {
                    if (light.GetPosition != light.GetPreviousPosition || forced == true)
                    {
                        lightingMap[x, y] += CalculateLightValue(new Vector2(x * 64, y * 64), light);
                    }
                }
            }
        }

        private int InsideBounds(int value, int bounds)
        {
            if (value > 0)
            {
                if (value < bounds)
                {
                    return value;
                }
                else
                {
                    return bounds;
                }
            }
            else
            {
                return 0;
            }
        }

        public float CalculateLightValue(Vector2 tilePosition, LightSource light)
        {
            Vector2 lightMap = new Vector2();
            float length = 0;
            lightMap = (light.GetPreviousPosition - tilePosition) / light.LightArea;
            length -= light.LightIntensity / lightMap.Length();

            if (length > 0) length = 0;

            lightMap = (light.GetPosition - tilePosition) / light.LightArea;
            length += light.LightIntensity / lightMap.Length();
            return length;
        }

        public float GetLightValue(int x, int y)
        {
            return lightingMap[x, y];
        }
    }
}
