using MapEditorWinform.Enemies;
using MapEditorWinform.Turrets;
using MapEditorWinform.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MapEditorWinform
{

    public delegate void MapManagerCallbackEvent(string notice, bool value);
    
    // la clase DrawTest extiende de MonoGame.Forms.Controls.UpdateWindow
    // Encapsula el juego en un componente de windows forms
    public class DrawTest : MonoGame.Forms.Controls.UpdateWindow
    {


        Camera camera;

        Texture2D nonType, selector, type1, type2, cursor, start, end, hud, dot, enemyTexture, enemyLifeBar;
        Texture2D turretBase, turretCannon, turretBullet, lineRange, range;

        // HUD
        Dictionary<int, Texture2D> listHUD = new Dictionary<int, Texture2D>();
        HUD Hud;


        // Solo debe aceptar los clicks cuando el raton está sobre el componente
        public bool isMouseOver = false;
        
        SpriteFont sf;
        Vector2 mouseCenter;

        struct MapIndex
        {
            public int col, row;
        }

        // Lista de torretas
        List<Turret> torretas = new List<Turret>();
        List<MapIndex> indices = new List<MapIndex>();
        Dictionary<int, Texture2D> textureListTower = new Dictionary<int, Texture2D>();

        // Lista de enemigos
        List<Enemy> enemigos = new List<Enemy>();
        

        Enemy enemy;
        public int gameState = 0;
        int enemiesToAdd = 0;

        // Editor de mapas
        public MapManager map;

        //Disparador de eventos
        MapManagerCallbackEvent callback;

        GraphicsDevice graphicsDevice;

        protected override void Initialize()
        {
            base.Initialize();

            graphicsDevice = this.GraphicsDevice;

            // inicializa la camara con los graficos de la pantalla actual
            camera = new Camera(GraphicsDevice.Viewport);

            // Enlaza el manejador de eventos a una funcion local
            callback = new MapManagerCallbackEvent(hasNotice);

            // carga las texturas necesarias para el editor y demás
            loadTextures();

            // crea un enemigo [TEST]
            enemy = new Enemy(enemyTexture, enemyLifeBar, new Vector2(100, 100));

            // HUD
            Hud = new HUD(listHUD);
        }


        // cambia el seleccionador del editor de mapa
        // el seleccionador crea una vista previa del tipo seleccionado
        private int SelectedTypeOnMap = 0;
        public void selectedType(int selectedType)
        {
            map.SelectedType = selectedType;
            SelectedTypeOnMap = selectedType;
            Hud.typeSelection = selectedType;
        }

        public int getSelectedType()
        {
            return SelectedTypeOnMap;
        }


        // Carga las texuras para el editor 
        // Las texturas deben estar compiladas y en la carpeta de ejecución de .exe
        // puede ser Debug, o Release según las configuraciones de visual studio
        public void loadTextures()
        {
            cursor = Editor.Content.Load<Texture2D>("cursor");
            nonType = Editor.Content.Load<Texture2D>("Tile");
            selector = Editor.Content.Load<Texture2D>("Selector");
            type1 = Editor.Content.Load<Texture2D>("DarkGrass");
            type2 = Editor.Content.Load<Texture2D>("Terrain");
            hud = Editor.Content.Load<Texture2D>("HUD");

            


            start = Editor.Content.Load<Texture2D>("MapEditorStart");
            end = Editor.Content.Load<Texture2D>("MapEditorEnd");

            enemyTexture = Editor.Content.Load<Texture2D>("EnemiTest");
            enemyLifeBar = Editor.Content.Load<Texture2D>("lifeBar");

            turretBase = Editor.Content.Load<Texture2D>("TowerBase");
            turretBullet = Editor.Content.Load<Texture2D>("TowerBullet");
            turretCannon = Editor.Content.Load<Texture2D>("TowerCanon");
            lineRange = Editor.Content.Load<Texture2D>("lineRange");
            range = Editor.Content.Load<Texture2D>("Range");



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
            dict.Add("selector", selector);
            dict.Add("type1", type1);
            dict.Add("type2", type2);
            dict.Add("start", start);
            dict.Add("end", end);
            dict.Add("dot", dot);


            textureListTower.Add(0, turretBase);
            textureListTower.Add(1, turretCannon);
            textureListTower.Add(2, turretBullet);
            textureListTower.Add(3, range);
            textureListTower.Add(4, lineRange);


            listHUD.Add(0, hud);
            listHUD.Add(1, nonType);
            listHUD.Add(2, type1);
            listHUD.Add(3, type2);
            listHUD.Add(4, start);
            listHUD.Add(5, end);
            listHUD.Add(6, selector);




            map = new MapManager(10, 10, dict, callback);
        }

        internal void removeAllturrets()
        {
            torretas.Clear();
            indices.Clear();
        }

        public void AddEnemies(int count)
        {
            enemiesToAdd = count;
        }

        // obtiene la posicion del ratón, la convierte a posiciones reales de la pantalla
        public Vector2 getMousePos()
        {
            return Vector2.Transform(Mouse.GetState().Position.ToVector2(), Matrix.Invert(camera.Transform));
        }

        public bool isMouseBounds()
        {
            return (getMousePos().X >= 0 && getMousePos().Y >= 0);
        }

        public void updateMapMgr()
        {
            mouseCenter = getMousePos();

            map.CurrentCol = (int)mouseCenter.X / 100;
            map.CurrentRow = (int)mouseCenter.Y / 100;

            mouseCenter.X -= 50;
            mouseCenter.Y -= 50;
            mouseCenter -= new Vector2(50, 50);





            // Cuando se haga click izquierdo y el ratón esté sobre el componente agrega un elemento al mapa 
            // segun el tipo seleccionado
            MouseState currentMS = Mouse.GetState();
            if (currentMS.LeftButton == ButtonState.Pressed && isMouseBounds() && getSelectedType() != TypeSelection.TURRET)
            {
                map.addTo();
            }
            else if(currentMS.LeftButton == ButtonState.Pressed && isMouseBounds() && getSelectedType() == TypeSelection.TURRET)
            {
                addTurret();
            }
            
        }

        public void addTurret()
        {
            if (map.CurrentCol < 0 || map.CurrentRow < 0)
                return;

            MapIndex mI = new MapIndex();
            mI.col = map.CurrentCol;
            mI.row = map.CurrentRow;

            if (indices.Contains(mI))
                return;

            Turret t = new Turret(textureListTower, mI.col, mI.row, 300, 35, 2, sf);
            torretas.Add(t);
            indices.Add(mI);

        }

        float ream = 100;
        int idEnemy = 0;

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            camera.UpdateCamera(GraphicsDevice.Viewport);

            if(enemiesToAdd >0)
            {
                ream -= gameTime.ElapsedGameTime.Milliseconds;
                if(ream < 0)
                {
                    enemigos.Add(new Enemy(enemyTexture, enemyLifeBar, new Vector2(100, 100), pointsinmap: map.pathPointLines, ID: idEnemy));
                    idEnemy++;
                    ream = 100;
                    enemiesToAdd--;
                }
            }



            if (gameState == 1 )
            {
                enemigos.ForEach(x => x.Update(gameTime));
                torretas.ForEach(x => x.Update(gameTime, enemigos));
                removeEnemies();
            }

            Hud.Update(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            if (Hud.typeSelection != -1)
                this.selectedType(Hud.typeSelection);

           
            updateMapMgr();

        }


        private void removeEnemies()
        {
            for (int i = 0; i < enemigos.Count; i++)
            {
                if(enemigos[i].Visible == false)
                {
                    enemigos.RemoveAt(i);
                    i--;
                }
            }
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
                enemigos.ForEach(x => x.Draw(Editor.spriteBatch));

            torretas.ForEach(x => x.Draw(Editor.spriteBatch));

            if (getSelectedType() == TypeSelection.TURRET)
            {
                Vector2 position = getMousePos();
                // base de la torreta
                Editor.spriteBatch.Draw(textureListTower[0], position -= new Vector2(50, 50), Color.White);

                // cañon de la torreta
                Editor.spriteBatch.Draw(textureListTower[1], position += new Vector2(50, 50), rotation: 0, color: Color.White * .5f, origin: new Vector2(50, 50));
            }

            
            Editor.spriteBatch.End();

            Editor.spriteBatch.Begin();

            Hud.Draw(Editor.spriteBatch);

            Editor.spriteBatch.End();
        }
    }
}
