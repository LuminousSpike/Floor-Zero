using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Floor_Zero.Classes.Entities
{
    internal struct Tile
    {
        public SpriteEffects spriteEffect;
        public short typeID;

        public TileType Type
        {
            get { return TileType.Get(typeID); }
            set { typeID = value.ID; }
        }
    }

    internal class TileType
    {
        private static readonly List<TileType> types;
        public bool Flippable;
        public short ID;
        public string Name;
        public bool solid;

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

            // Stone wall
            var StoneWall = new TileType();
            StoneWall.Name = "Stone Wall";
            StoneWall.solid = true;
            StoneWall.Flippable = true;
            types.Add(StoneWall);

            // Dirt floor
            var DirtFloor = new TileType();
            DirtFloor.Name = "Dirt Floor";
            DirtFloor.solid = false;
            DirtFloor.Flippable = true;
            types.Add(DirtFloor);

            // Grass floor
            var GrassFloor = new TileType();
            GrassFloor.Name = "Grass Floor";
            GrassFloor.solid = false;
            GrassFloor.Flippable = true;
            types.Add(GrassFloor);

            // Stone floor
            var StoneFloor = new TileType();
            StoneFloor.Name = "Stone Floor";
            StoneFloor.solid = false;
            StoneFloor.Flippable = true;
            types.Add(StoneFloor);
        }

        public static TileType Get(short id)
        {
            return types[id];
        }
    }
}