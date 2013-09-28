using System;
using System.IO;
using Floor_Zero.Classes.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Floor_Zero.Classes.File_System
{
    internal class TileMapParser
    {
        private BinaryReader binaryReader;
        private BinaryWriter binaryWriter;
        private string fileName;
        private Tile[,] tileMap;

        public void WriteMap(string fileName, Tile[,] tileMap, int mapSize)
        {
            using (binaryWriter = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                binaryWriter.Write(mapSize);
                for (int x = 0; x < mapSize; x++)
                {
                    for (int y = 0; y < mapSize; y++)
                    {
                        binaryWriter.Write(tileMap[x, y].typeID);
                        //binaryWriter.Write(EncodeSpriteEffect(tileMap[x, y].spriteEffect));
                    }
                }
            }
        }

        public Tile[,] ReadMap(string fileName)
        {
            Tile[,] tileMap;
            using (binaryReader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                int mapSize = binaryReader.ReadInt32();
                tileMap = new Tile[mapSize, mapSize];
                var rand = new Random();

                for (int x = 0; x < mapSize; x++)
                {
                    for (int y = 0; y < mapSize; y++)
                    {
                        tileMap[x, y].typeID = binaryReader.ReadInt16();
                        tileMap[x, y].spriteEffect = ChooseEffect(rand);
                        //tileMap[x, y].spriteEffect = DecodeSpriteEffect(binaryReader.ReadInt16());
                    }
                }
            }
            return tileMap;
        }

        private short EncodeSpriteEffect(SpriteEffects spriteEffect)
        {
            if (spriteEffect == SpriteEffects.FlipVertically)
            {
                return 1;
            }
            if (spriteEffect == SpriteEffects.FlipHorizontally)
            {
                return 2;
            }
            return 0;
        }

        private SpriteEffects DecodeSpriteEffect(short spriteEffect)
        {
            if (spriteEffect == 1)
            {
                return SpriteEffects.FlipVertically;
            }
            if (spriteEffect == 2)
            {
                return SpriteEffects.FlipHorizontally;
            }
            return SpriteEffects.None;
        }

        private SpriteEffects ChooseEffect(Random rand)
        {
            var effect = SpriteEffects.None;
            switch (rand.Next(0, 3))
            {
                case 0:
                    effect = SpriteEffects.FlipHorizontally;
                    break;
                case 1:
                    effect = SpriteEffects.FlipVertically;
                    break;
                case 2:
                    break;
            }
            return effect;
        }
    }
}