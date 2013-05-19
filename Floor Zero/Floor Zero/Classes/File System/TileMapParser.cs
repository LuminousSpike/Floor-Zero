using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Floor_Zero.Classes.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Floor_Zero.Classes.File_System
{
    class TileMapParser
    {
        BinaryWriter binaryWriter;
        BinaryReader binaryReader;
        string fileName;
        Tile[,] tileMap;

        public TileMapParser()
        {
            
        }

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
                Random rand = new Random();

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
                return (Int16)1;
            }
            else if (spriteEffect == SpriteEffects.FlipHorizontally)
            {
                return (Int16)2;
            }
            else
            {
                return (Int16)0;
            }
        }

        private SpriteEffects DecodeSpriteEffect(short spriteEffect)
        {
            if (spriteEffect == (Int16)1)
            {
                return SpriteEffects.FlipVertically;
            }
            else if (spriteEffect == (Int16)2)
            {
                return SpriteEffects.FlipHorizontally;
            }
            else
            {
                return SpriteEffects.None;
            }
        }

        private SpriteEffects ChooseEffect(Random rand)
        {
            SpriteEffects effect = SpriteEffects.None;
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
