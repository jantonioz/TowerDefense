using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ToweDefense
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Vector2 screenSize = Vector2.Zero;

        Texture2D tile, selectionTile, cursor;
        Vector2 position = Vector2.Zero, mousePos;

        float scale_x = 0;
        float scale_y = 0;
        float resolutionScale = 0;

        Camera camera;

        Mapa m;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            screenSize = new Vector2(
                    graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight
                );
            //this.IsMouseVisible = true;

            scale_x = 1 * (  screenSize.X / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width );
            scale_y = 1 * (  screenSize.Y / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height );
            resolutionScale = scale_x;
            // redimenciona los pixeles 
            graphics.IsFullScreen = true;

            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            tile = Content.Load<Texture2D>("Tile");
            selectionTile = Content.Load<Texture2D>("selectionTile");
            cursor = Content.Load<Texture2D>("cursor");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            camera = null;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            camera.UpdateCamera(graphics.GraphicsDevice.Viewport);
            MouseState ms = Mouse.GetState();
            mousePos = new Vector2(ms.X * scale_x, ms.Y * scale_y);

            int x = (int)(mousePos.X / 100);
            x *= 100;
            int y = (int)(mousePos.Y / 100);
            y *= 100;

            position = new Vector2(x, y);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here


            spriteBatch.Begin(transformMatrix: camera.Transform);

            spriteBatch.Draw(selectionTile, position, Color.White);
            for (int i = 0; i < screenSize.X; i += 100)
            {
                for (int j = 0; j < screenSize.Y; j += 100)
                {
                    spriteBatch.Draw(tile, new Vector2(i, j), Color.White);
                }
            }


            spriteBatch.Draw(cursor, mousePos, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
