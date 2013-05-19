using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Floor_Zero.Classes.Entities
{
    public abstract class Entity2D
    {
        protected Vector2 position = new Vector2(0, 0);
        protected Texture2D texture;

        public virtual void Initialize(Vector2 position)
        {
            this.position = position;
        }

        public virtual void LoadContent(Texture2D texture)
        {
            this.texture = texture;
        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
