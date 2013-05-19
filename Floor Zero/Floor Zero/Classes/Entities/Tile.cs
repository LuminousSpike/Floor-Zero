using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Floor_Zero.Classes.Entities
{
    struct Tile
    {
        public short typeID;
        public SpriteEffects spriteEffect;
        public TileType Type
        {
            get { return TileType.Get(typeID); }
            set { typeID = value.ID; }
        }
    }

    class TileType
    {
        public short ID;
        public string Name;
        public bool solid;
        public bool Flippable;

        static List<TileType> types;
        static TileType()
        {
            types = new List<TileType>();

            // Empty 1
            var Empty1 = new TileType();
            Empty1.Name = "Empty";
            Empty1.solid = false;
            Empty1.Flippable = false;
            types.Add(Empty1);

            // Empty 2
            var Empty2 = new TileType();
            Empty2.Name = "Empty";
            Empty2.solid = false;
            Empty2.Flippable = false;
            types.Add(Empty2);

            // Empty 3
            var Empty3 = new TileType();
            Empty3.Name = "Empty";
            Empty3.solid = false;
            Empty3.Flippable = false;
            types.Add(Empty3);

            // Stone
            var Stone = new TileType();
            Stone.Name = "Stone";
            Stone.solid = false;
            Stone.Flippable = true;
            types.Add(Stone);
        }

        public static TileType Get(short id)
        {
            return types[id];
        }
    }
}
