using System;
using Floor_Zero.Classes.Entities;
using Floor_Zero.Classes.File_System;
using Floor_Zero.Classes.Managers.Lighting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solar.Graphics.Cameras;
using Solar.Graphics.Lighting.Top_Down;
using Solar.Graphics.Sprites;
using Solar.Input;

namespace Floor_Zero.Classes.Managers
{
    /// <summary>
    ///     Manages the Tile System.
    /// </summary>
    internal class ManagerTile
    {
        private readonly GraphicsDevice graphicsDevice;
        private readonly Lighting_Manager lighting_Manager;
        private readonly TileMapParser tileMapParser = new TileMapParser();
        private Vector2 CurrentMousePosition;
        private Rectangle _holder;
        private KeyboardState currentKeyboardState;
        private int gridSize = 240;
        private LightArea mouseLight;
        private bool paintMode;
        private KeyboardState previousKeyboardState;
        private Random rand;
        private Vector2 selectedTile;
        private int selectedTileIndex = 3;
        private Tile[,] tileGrid;
        private bool tileMapChanged;
        private SpriteSheet tileSheet;
        private int tileSize = 64;
        private Rectangle tilesInView;
        private Rectangle worldArea;

        public ManagerTile(GraphicsDevice graphicsDevice, Lighting_Manager lighting_Manager)
        {
            this.graphicsDevice = graphicsDevice;
            this.lighting_Manager = lighting_Manager;
        }

        public void Initialize(bool paintMode)
        {
            tileGrid = new Tile[gridSize, gridSize];
            worldArea = new Rectangle(0, 0, 1280, 720);
            // Init the holder
            _holder = new Rectangle(0, 0, tileSize, tileSize);
            rand = new Random();
            this.paintMode = paintMode;

            // Load Light and add to manager.
            mouseLight = new LightArea(graphicsDevice, ShadowmapSize.Size2048);
            lighting_Manager.AddMoveableLight(mouseLight);
        }

        public void LoadContent(ContentManager Content)
        {
            // Load Tile SpriteSheet
            var TileTexture = Content.Load<Texture2D>(@"Tile_Sheet");
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

        public void Draw(SpriteBatch spriteBatch, BasicCamera2D camera)
        {
            if (paintMode)
            {
                PaintDraw(spriteBatch, camera);
            }
            else
            {
                PlayDraw(spriteBatch, camera);
            }
        }

        private void PaintUpdate(BasicCamera2D camera)
        {
            CurrentMousePosition = Vector2.Transform(new Vector2(Game1.mouseState.X, Game1.mouseState.Y),
                Matrix.Invert(camera._transform));

            tilesInView = CullTiles(camera);

            mouseLight.LightPosition = CurrentMousePosition;

            currentKeyboardState = Keyboard.GetState();

            CheckInput();

            ChangeSelectedTileIndex();

            previousKeyboardState = currentKeyboardState;
        }

        private void PlayUpdate(BasicCamera2D camera)
        {
            CurrentMousePosition = Vector2.Transform(new Vector2(Game1.mouseState.X, Game1.mouseState.Y),
                Matrix.Invert(camera._transform));

            tilesInView = CullTiles(camera);
        }

        private void PaintDraw(SpriteBatch spriteBatch, BasicCamera2D camera)
        {
            if (tileMapChanged)
            {
                lighting_Manager.DrawStaticShadows(DrawSolidTiles, spriteBatch, camera);
                tileMapChanged = false;
            }

            // move this code to Manager_Render to allow for shadow and normal rendering via one method.
            lighting_Manager.DrawDynamicShadows(DrawSolidTilesCulled, spriteBatch, camera);

            graphicsDevice.SetRenderTarget(lighting_Manager.screenShadows);
            graphicsDevice.Clear(new Color(16, 16, 16));
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, camera.get_transformation(graphicsDevice));
            lighting_Manager.Draw(spriteBatch);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);

            graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, camera.get_transformation(graphicsDevice));
            PlayDraw(spriteBatch, camera);
            spriteBatch.End();

            var blendState = new BlendState();
            blendState.ColorSourceBlend = Blend.DestinationColor;
            blendState.ColorDestinationBlend = Blend.SourceColor;

            spriteBatch.Begin(SpriteSortMode.Immediate, blendState);
            spriteBatch.Draw(lighting_Manager.screenShadows, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private void PlayDraw(SpriteBatch spriteBatch, BasicCamera2D camera)
        {
            for (int x = CalculateLowerTileParseBounds(tilesInView.X);
                x < CalculateUpperTileParseBounds(tilesInView.Width + 1);
                x++)
            {
                for (int y = CalculateLowerTileParseBounds(tilesInView.Y);
                    y < CalculateUpperTileParseBounds(tilesInView.Height + 1);
                    y++)
                {
                    tileSheet.Draw(spriteBatch, GetTileLocation(x, y), tileGrid[x, y].spriteEffect,
                        tileGrid[x, y].typeID);
                    MouseHighlight(spriteBatch, x, y, camera);
                }
            }
        }

        private void AddTile(int tileIndex, Vector2 position, int gridX, int gridY)
        {
            var tile = new Tile {typeID = (Int16) tileIndex};
            tileGrid[gridX, gridY] = tile;
        }

        /// <summary>
        ///     Creates the initial Starting Tiles (White & Grey)
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
                    if (tile%2 != 0)
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

        private void MouseHighlight(SpriteBatch spriteBatch, int x, int y, BasicCamera2D camera)
        {
            if (TileArea(x, y, camera))
                tileSheet.Draw(spriteBatch, new Vector2(x*tileSize, y*tileSize), SpriteEffects.None, selectedTileIndex);
        }

        private bool TileArea(int x, int y, BasicCamera2D camera)
        {
            if ((CurrentMousePosition.X >= GetTileLocation(x, y).X - tileSize &&
                 CurrentMousePosition.X <= GetTileLocation(x, y).X) &&
                (CurrentMousePosition.Y >= GetTileLocation(x, y).Y - tileSize &&
                 CurrentMousePosition.Y <= GetTileLocation(x, y).Y))
            {
                selectedTile = new Vector2(x, y);
                return true;
            }
            return false;
        }

        private void ReplaceTile(int index)
        {
            tileGrid[(int) selectedTile.X, (int) selectedTile.Y].typeID = (Int16) index;
            if (tileGrid[(int) selectedTile.X, (int) selectedTile.Y].Type.Flippable)
            {
                tileGrid[(int) selectedTile.X, (int) selectedTile.Y].spriteEffect = ChooseEffect(rand);
            }
        }

        private void RemoveTile()
        {
            if ((selectedTile.X + selectedTile.Y)%2 != 0)
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
            return false;
        }

        private Rectangle CullTiles(BasicCamera2D camera)
        {
            worldArea = new Rectangle((int) camera._pos.X - (int) ((1280/2)/camera.Zoom),
                (int) camera._pos.Y - (int) ((720/2)/camera.Zoom), (int) (1280/camera.Zoom), (int) (720/camera.Zoom));
            int minX = Math.Max((int) Math.Floor((float) worldArea.Left/tileSize), 0);
            int maxX = Math.Min((int) Math.Ceiling((float) worldArea.Right/tileSize), 1280);
            int minY = Math.Max((int) Math.Floor((float) worldArea.Top/tileSize), 0);
            int maxY = Math.Min((int) Math.Ceiling((float) worldArea.Bottom/tileSize), 720);

            if (minX > 0) minX--;
            if (minY > 0) minY--;

            return new Rectangle(minX, minY, maxX, maxY);
        }

        private Vector2 GetTileLocation(int x, int y)
        {
            return new Vector2(x*tileSize, y*tileSize);
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
            if (InputHelper.InputPressed(Keys.G, Buttons.A))
            {
                lighting_Manager.AddStationaryLight(new LightArea(graphicsDevice, ShadowmapSize.Size2048)
                {
                    LightPosition = CurrentMousePosition
                });
                tileMapChanged = true;
            }

            if (Game1.mouseState.LeftButton == ButtonState.Pressed)
            {
                ReplaceTile(selectedTileIndex);
                tileMapChanged = true;
            }

            if (Game1.mouseState.RightButton == ButtonState.Pressed)
            {
                RemoveTile();
                tileMapChanged = true;
            }

            if (InputHelper.InputPressed(Keys.P, Buttons.Start))
            {
                tileMapParser.WriteMap("Map.FZMP", tileGrid, gridSize);
            }

            if (InputHelper.InputPressed(Keys.O, Buttons.Start))
            {
                tileGrid = tileMapParser.ReadMap("Map.FZMP");
            }
        }

        private void DrawTile(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffect, short typeID)
        {
            tileSheet.Draw(spriteBatch, position, spriteEffect, typeID);
        }

        private void DrawSolidTilesCulled(SpriteBatch spriteBatch, LightArea light)
        {
            spriteBatch.Begin();
            for (int x = CalculateLowerTileParseBounds(tilesInView.X);
                x < CalculateUpperTileParseBounds(tilesInView.Width + 1);
                x++)
            {
                for (int y = CalculateLowerTileParseBounds(tilesInView.Y);
                    y < CalculateUpperTileParseBounds(tilesInView.Height + 1);
                    y++)
                {
                    if (tileGrid[x, y].Type.solid)
                    {
                        DrawTile(spriteBatch, light.ToRelativePosition(GetTileLocation(x, y)),
                            tileGrid[x, y].spriteEffect, tileGrid[x, y].typeID);
                    }
                }
            }
            spriteBatch.End();
        }

        private void DrawSolidTiles(SpriteBatch spriteBatch, LightArea light)
        {
            spriteBatch.Begin();
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (tileGrid[x, y].Type.solid)
                    {
                        DrawTile(spriteBatch, light.ToRelativePosition(GetTileLocation(x, y)),
                            tileGrid[x, y].spriteEffect, tileGrid[x, y].typeID);
                    }
                }
            }
            spriteBatch.End();
        }
    }
}