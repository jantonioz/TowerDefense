using MapEditorWinform.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorWinform
{
    public class DrawTest : MonoGame.Forms.Controls.UpdateWindow
    {
        Camera camera;

        public Texture2D nonType, type1, type2, cursor, start, end;
        public bool isMouseOver = false;
        SpriteFont sf;
        Vector2 mouseCenter;
        MouseState lastMS;

        public MapManager map;

        protected override void Initialize()
        {
            base.Initialize();
            camera = new Camera(GraphicsDevice.Viewport);
            loadTextures();

        }

        public void selectedType(int selectedType)
        {
            map.SelectedType = selectedType;
        }

        public void loadTextures()
        {
            cursor = Editor.Content.Load<Texture2D>("cursor");
            nonType = Editor.Content.Load<Texture2D>("Tile");
            type1 = Editor.Content.Load<Texture2D>("DarkGrass");
            type2 = Editor.Content.Load<Texture2D>("Terrain");

            start = Editor.Content.Load<Texture2D>("MapEditorStart");
            end = Editor.Content.Load<Texture2D>("MapEditorEnd");

            sf = Editor.Content.Load<SpriteFont>("Font");
            Dictionary<string, Texture2D> dict = new Dictionary<string, Texture2D>();

            dict.Add("nonType", nonType);
            dict.Add("type1", type1);
            dict.Add("type2", type2);
            dict.Add("start", start);
            dict.Add("end", end);

            map = new MapManager(10, 10, dict);
        }

        public Vector2 getMousePos()
        {
            return Vector2.Transform(Mouse.GetState().Position.ToVector2(), Matrix.Invert(camera.Transform));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            camera.UpdateCamera(GraphicsDevice.Viewport);


            mouseCenter = getMousePos();

            map.CurrentCol = (int)mouseCenter.X / 100;
            map.CurrentRow = (int)mouseCenter.Y / 100;


            mouseCenter.X -= 50;
            mouseCenter.Y -= 50;

            MouseState currentMS = Mouse.GetState();


            if (lastMS.LeftButton == ButtonState.Released && currentMS.LeftButton == ButtonState.Pressed && isMouseOver)
            {
                map.addTo();
            }
            
        }

        protected override void Draw()
        {
            base.Draw();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Editor.spriteBatch.Begin(transformMatrix: camera.Transform);


            map.Draw(Editor.spriteBatch, new Vector2(map.CurrentCol * 100, map.CurrentRow * 100));


            //Editor.spriteBatch.Draw(cursor, getMousePos(), Color.White);

            //Editor.spriteBatch.DrawString(sf, $"C: {currentCol}, R:{currentRow}", mouseCenter, Color.Red);

            Editor.spriteBatch.End();
            
        }
    }
}
