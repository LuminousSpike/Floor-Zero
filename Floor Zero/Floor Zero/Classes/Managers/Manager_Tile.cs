using System;
using Floor_Zero.Classes.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solar.Graphics.Cameras;
using Solar.Graphics.Sprites;
using Solar.Input;
using Floor_Zero.Classes.File_System;
using Floor_Zero.Classes.Managers.Lighting;

namespace Floor_Zero.Classes.Managers
{
    /// <summary>
    /// Manages the Tile System.
    /// </summary>
    class Manager_Tile
    {
        private int gridSize = 240, tileSize = 64, selectedTileIndex = 3;
        private Tile[,] tileGrid;
        private SpriteSheet tileSheet;
        private Vector2 selectedTile, CurrentMousePosition;
        private bool paintMode;
        private KeyboardState currentKeyboardState, previousKeyboardState;
        private Rectangle worldArea, _holder, tilesInView;
        private Random rand;
        private TileMapParser tileMapParser = new TileMapParser();
        private RenderTarget2D screenShadows;

        public void Initialize(bool paintMode, GraphicsDevice graphicsDevice)
        {
            tileGrid = new Tile[gridSize, gridSize];
            worldArea = new Rectangle(0, 0, 1280, 720);
            // Init the holder
            _holder = new Rectangle(0, 0, tileSize, tileSize);
            rand = new Random();
            this.paintMode = paintMode;

            screenShadows = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }

        public void LoadContent(ContentManager Content)
        {
            // Load Tile SpriteSheet
            Texture2D TileTexture = Content.Load<Texture2D>(@"Tile_Sheet");
            tileSheet = new SpriteSheet(TileTexture, new Vector2(tileSize, tileSize));

            CreateTiles(paintMode);
        }

        public void UnloadContent()
        {

        }

        public void Update(BasicCamera2D camera)
        {
            if (paintMode)
            {
                PaintUpdate(camera);
            }
            else
            {
                PlayUpdate(camera);
            }
        }

        public void Draw(SpriteBatch spriteBatch, BasicCamera2D camera, Lighting_Manager lighting_Manager, GraphicsDevice graphicsDevice)
        {
            if (paintMode)
            {
                PaintDraw(spriteBatch, camera, lighting_Manager, graphicsDevice);
            }
            else
            {
                PlayDraw(spriteBatch, camera);
            }
        }

        private void PaintUpdate(BasicCamera2D camera)
        {
            CurrentMousePosition = Vector2.Transform(new Vector2(Game1.mouseState.X, Game1.mouseState.Y), Matrix.Invert(camera._transform));

            tilesInView = CullTiles(camera);

            currentKeyboardState = Keyboard.GetState();

            CheckInput();

            ChangeSelectedTileIndex();

            previousKeyboardState = currentKeyboardState;
        }

        private void PlayUpdate(BasicCamera2D camera)
        {
            CurrentMousePosition = Vector2.Transform(new Vector2(Game1.mouseState.X, Game1.mouseState.Y), Matrix.Invert(camera._transform)); 

            tilesInView = CullTiles(camera);
        }

        private void PaintDraw(SpriteBatch spriteBatch, BasicCamera2D camera, Lighting_Manager lighting_Manager, GraphicsDevice graphicsDevice)
        {
            lighting_Manager.lightArea1.LightPosition = CurrentMousePosition;
            lighting_Manager.lightArea1.BeginDrawingShadowCasters();
            spriteBatch.Begin();
            for (int x = CalculateLowerTileParseBounds(tilesInView.X); x < CalculateUpperTileParseBounds(tilesInView.Width + 1); x++)
            {
                for (int y = CalculateLowerTileParseBounds(tilesInView.Y); y < CalculateUpperTileParseBounds(tilesInView.Height + 1); y++)
                {
                    if (tileGrid[x, y].Type.solid)
                    {

                        DrawTile(spriteBatch, lighting_Manager.lightArea1.ToRelativePosition(GetTileLocation(x, y)), tileGrid[x, y].spriteEffect, tileGrid[x, y].typeID, Color.Black);

                        
                    }
                    MouseHighlight(spriteBatch, x, y, camera);
                }
            }
            spriteBatch.End();
            lighting_Manager.lightArea1.EndDrawingShadowCasters();
            lighting_Manager.shadowmapResolver.ResolveShadows(lighting_Manager.lightArea1.RenderTarget, lighting_Manager.lightArea1.RenderTarget, lighting_Manager.lightArea1.LightPosition);

            graphicsDevice.SetRenderTarget(screenShadows);
            graphicsDevice.Clear(new Color(15, 15, 15));
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, camera.get_transformation(graphicsDevice));
            spriteBatch.Draw(lighting_Manager.lightArea1.RenderTarget, lighting_Manager.lightArea1.LightPosition - lighting_Manager.lightArea1.LightAreaSize * 0.5f, Color.White * 0.5f);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);

            //graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, camera.get_transformation(graphicsDevice));
            PlayDraw(spriteBatch, camera);
            spriteBatch.End();

            BlendState blendState = new BlendState();
            blendState.ColorSourceBlend = Blend.DestinationColor;
            blendState.ColorDestinationBlend = Blend.SourceColor;

            spriteBatch.Begin(SpriteSortMode.Immediate, blendState);
            spriteBatch.Draw(screenShadows, Vector2.Zero, Color.White);
            spriteBatch.End();


        }

        private void PlayDraw(SpriteBatch spriteBatch, BasicCamera2D camera)
        {
            for (int x = CalculateLowerTileParseBounds(tilesInView.X); x < CalculateUpperTileParseBounds(tilesInView.Width + 1); x++)
            {
                for (int y = CalculateLowerTileParseBounds(tilesInView.Y); y < CalculateUpperTileParseBounds(tilesInView.Height + 1); y++)
                {
                    tileSheet.Draw(spriteBatch, GetTileLocation(x, y), tileGrid[x, y].spriteEffect, tileGrid[x, y].typeID);
                }
            }
        }

        private void AddTile(int tileIndex, Vector2 position, int gridX, int gridY)
        {
            Tile tile = new Tile() { typeID = (Int16)tileIndex };
            tileGrid[gridX, gridY] = tile;
        }

        /// <summary>
        /// Creates the initial Starting Tiles (White & Grey)
        /// </summary>
        private void CreateTiles(bool paintMode)
        {
            int tile = 0;
            for (int x = 0; x < gridSize; x++)
            {
                tile++;
                for (int y = 0; y < gridSize; y++)
                {
                    tile++;
                    if (tile % 2 != 0)
                    {
                        AddTile(0, GetTileLocation(x, y), x, y);
                    }
                    else
                    {
                        AddTile(1, GetTileLocation(x, y), x, y);
                    }
                }
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

        private void MouseHighlight(SpriteBatch spriteBatch, int x, int y, BasicCamera2D camera)
        {
            if (TileArea(x, y, camera))
                tileSheet.Draw(spriteBatch, new Vector2(x * tileSize, y * tileSize), SpriteEffects.None, selectedTileIndex);
        }

        private bool TileArea(int x, int y, BasicCamera2D camera)
        {
            if ((CurrentMousePosition.X >= GetTileLocation(x, y).X - tileSize && CurrentMousePosition.X <= GetTileLocation(x, y).X) &&
                (CurrentMousePosition.Y >= GetTileLocation(x, y).Y - tileSize && CurrentMousePosition.Y <= GetTileLocation(x, y).Y))
            {
                selectedTile = new Vector2(x, y);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ReplaceTile(int index)
        {
            tileGrid[(int)selectedTile.X, (int)selectedTile.Y].typeID = (Int16)index;
            if (tileGrid[(int)selectedTile.X, (int)selectedTile.Y].Type.Flippable)
            {
                tileGrid[(int)selectedTile.X, (int)selectedTile.Y].spriteEffect = ChooseEffect(rand);
            }
        }

        private void RemoveTile()
        {
            if ((selectedTile.X + selectedTile.Y) % 2 != 0)
            {
                ReplaceTile(0);
            }
            else
            {
                ReplaceTile(1);
            }
        }

        private void ChangeSelectedTileIndex()
        {
            if (InputHelper.InputPressed(Keys.X, Buttons.RightThumbstickUp) && selectedTileIndex < 6)
            {
                selectedTileIndex++;
            }

            if (InputHelper.InputPressed(Keys.Z, Buttons.RightThumbstickDown) && selectedTileIndex > 3)
            {
                selectedTileIndex--;
            }
        }

        private bool CheckKeyboardState(Keys key)
        {
            if (currentKeyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Rectangle CullTiles(BasicCamera2D camera)
        {
            worldArea = new Rectangle((int)camera._pos.X - (int)((1280 / 2) / camera.Zoom), (int)camera._pos.Y - (int)((720 / 2) / camera.Zoom), (int)(1280 / camera.Zoom), (int)(720 / camera.Zoom));
            int minX = Math.Max((int)Math.Floor((float)worldArea.Left / tileSize), 0);
            int maxX = Math.Min((int)Math.Ceiling((float)worldArea.Right / tileSize), 1280);
            int minY = Math.Max((int)Math.Floor((float)worldArea.Top / tileSize), 0);
            int maxY = Math.Min((int)Math.Ceiling((float)worldArea.Bottom / tileSize), 720);

            if (minX > 0) minX--;
            if (minY > 0) minY--;

            return new Rectangle(minX, minY, maxX, maxY);
        }

        private Vector2 GetTileLocation(int x, int y)
        {
            return new Vector2(x * tileSize, y * tileSize);
        }

        private int CalculateLowerTileParseBounds(int tileBound)
        {
            if (tileBound > 0)
            {
                tileBound--;
            }
            return tileBound;
        }

        private int CalculateUpperTileParseBounds(int tileBound)
        {
            if (tileBound < gridSize)
            {
                tileBound++;
            }
            else if (tileBound >= gridSize)
            {
                tileBound = gridSize;
            }
            return tileBound;
        }

        private void CheckInput()
        {
            if (Game1.mouseState.LeftButton == ButtonState.Pressed)
            {
                ReplaceTile(selectedTileIndex);
            }

            if (Game1.mouseState.RightButton == ButtonState.Pressed)
            {
                RemoveTile();
            }

            if(InputHelper.InputPressed(Keys.P, Buttons.Start))
            {
                tileMapParser.WriteMap("Map.FZMP", tileGrid, gridSize);
            }

            if (InputHelper.InputPressed(Keys.O, Buttons.Start))
            {
                tileGrid = tileMapParser.ReadMap("Map.FZMP");
            }
        }

        private void DrawTile(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffect, short typeID, Color color)
        {
            tileSheet.Draw(spriteBatch, position, spriteEffect, typeID);
        }
    }
}
