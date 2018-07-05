using MapEditorWinform.Enemies;
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

    public delegate void MapManagerCallbackEvent(string notice, bool value);


    // la clase DrawTest extiende de MonoGame.Forms.Controls.UpdateWindow
    // Encapsula el juego en un componente de windows forms
    public class DrawTest : MonoGame.Forms.Controls.UpdateWindow
    {
        Camera camera;

        public Texture2D nonType, type1, type2, cursor, start, end, dot, enemyTexture, enemyLifeBar;

        // Solo debe aceptar los clicks cuando el raton está sobre el componente
        public bool isMouseOver = false;
        
        SpriteFont sf;
        Vector2 mouseCenter;

        
        Enemy enemy;
        public int gameState = 0; 

        // Editor de mapas
        public MapManager map;

        //Disparador de eventos
        MapManagerCallbackEvent callback;

        protected override void Initialize()
        {
            base.Initialize();

            // inicializa la camara con los graficos de la pantalla actual
            camera = new Camera(GraphicsDevice.Viewport);

            // Enlaza el manejador de eventos a una funcion local
            callback = new MapManagerCallbackEvent(hasNotice);

            // carga las texturas necesarias para el editor y demás
            loadTextures();

            // crea un enemigo [TEST]
            enemy = new Enemy(enemyTexture, enemyLifeBar, new Vector2(100, 100));
            

        }


        // cambia el seleccionador del editor de mapa
        // el seleccionador crea una vista previa del tipo seleccionado
        public void selectedType(int selectedType)
        {
            map.SelectedType = selectedType;
        }


        // Carga las texuras para el editor 
        // Las texturas deben estar compiladas y en la carpeta de ejecución de .exe
        // puede ser Debug, o Release según las configuraciones de visual studio
        public void loadTextures()
        {
            cursor = Editor.Content.Load<Texture2D>("cursor");
            nonType = Editor.Content.Load<Texture2D>("Tile");
            type1 = Editor.Content.Load<Texture2D>("DarkGrass");
            type2 = Editor.Content.Load<Texture2D>("Terrain");

            start = Editor.Content.Load<Texture2D>("MapEditorStart");
            end = Editor.Content.Load<Texture2D>("MapEditorEnd");

            enemyTexture = Editor.Content.Load<Texture2D>("EnemiTest");
            enemyLifeBar = Editor.Content.Load<Texture2D>("lifeBar");


            // crea una textura de 3x3 de color blanco
            dot = new Texture2D(GraphicsDevice, 3, 3);
            Color[] data = new Color[3 * 3];
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.White;

            dot.SetData(data);



            sf = Editor.Content.Load<SpriteFont>("Font");

            // Diccionario para guardar las texturas
            // para insertar un item en un diccionario se necesita la llave y el valor
            // para obtener un item de un diccionario se llama según la llave
            // dict[_Llave_]
            Dictionary<string, Texture2D> dict = new Dictionary<string, Texture2D>();

            dict.Add("nonType", nonType);
            dict.Add("type1", type1);
            dict.Add("type2", type2);
            dict.Add("start", start);
            dict.Add("end", end);
            dict.Add("dot", dot);
            
            map = new MapManager(10, 10, dict, callback);
        }

        // obtiene la posicion del ratón, la convierte a posiciones reales de la pantalla
        public Vector2 getMousePos()
        {
            return Vector2.Transform(Mouse.GetState().Position.ToVector2(), Matrix.Invert(camera.Transform));
        }

        public void updateMapMgr()
        {
            mouseCenter = getMousePos();

            map.CurrentCol = (int)mouseCenter.X / 100;
            map.CurrentRow = (int)mouseCenter.Y / 100;


            mouseCenter.X -= 50;
            mouseCenter.Y -= 50;

            MouseState currentMS = Mouse.GetState();


            // Cuando se haga click izquierdo y el ratón esté sobre el componente agrega un elemento al mapa 
            // segun el tipo seleccionado
            if (currentMS.LeftButton == ButtonState.Pressed && isMouseOver)
            {
                map.addTo();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            camera.UpdateCamera(GraphicsDevice.Viewport);
            updateMapMgr();


            
            if(gameState == 1)
                enemy.Update();



        }

        /// <summary>
        /// Método que captura los eventos generados por el editor de mapa
        /// </summary>
        /// <param name="notice">Mensaje del evento</param>
        /// <param name="value">Afirmación o negacion del mensaje</param>

        public void hasNotice(string notice, bool value)
        {
            if(notice.Contains("end") && value == true)
            {
                enemy.PointsInMap = this.map.pathPointLines;
            }

        }


        protected override void Draw()
        {
            base.Draw();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Editor.spriteBatch.Begin(transformMatrix: camera.Transform);


            map.Draw(Editor.spriteBatch, new Vector2(map.CurrentCol * 100, map.CurrentRow * 100));


            if (gameState == 1)
                enemy.Draw(Editor.spriteBatch);

            //Editor.spriteBatch.Draw(cursor, getMousePos(), Color.White);

            //Editor.spriteBatch.DrawString(sf, $"C: {currentCol}, R:{currentRow}", mouseCenter, Color.Red);

            Editor.spriteBatch.End();
            
        }
    }
}
