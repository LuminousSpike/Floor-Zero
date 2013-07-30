using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Floor_Zero.Classes.Entities
{
    public class LightSource : Entity2D
    {
        private float lightIntensity, lightArea;
        private Vector2 previousPosition;
        private bool dynamic;

        public float LightIntensity { get { return lightIntensity; } set { lightIntensity = value; } }
        public float LightArea { get { return lightArea; } set { lightArea = value; } }
        public Vector2 GetPosition { get { return position; } }
        public Vector2 GetPreviousPosition { get { return previousPosition; } }
        public bool IsDynamic { get { return dynamic; } }

        public LightSource(Vector2 position, float lightIntensity, float LightArea, bool dynamic)
        {
            this.position = position;
            this.previousPosition = position;
            this.lightIntensity = lightIntensity;
            this.LightArea = LightArea;
            this.dynamic = dynamic;
        }

        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
        }

        public override void LoadContent(Texture2D texture)
        {
            base.LoadContent(texture);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public void SetPosition(Vector2 position)
        {
            this.previousPosition = this.position;
            this.position = position;
        }
    }
}
