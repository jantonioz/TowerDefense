using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorWinform.Utils
{


    public class MapManager
    {
        // arreglo que contiene los datos de todo el mapa
        Tile[,] tiles;
        // diccionario que contiene las texturas
        Dictionary<string, Texture2D> texturas;

        // un punto blanco 3x3
        Texture2D dot;

        // posiciones de inicio y fin del camino en el mapa
        public Vector2 lastStart = new Vector2(-1, -1);
        public Vector2 lastEnd = new Vector2(-1, -1);

        // posiciones int, int de cada punto que genera el pathfinder
        private int[,] pathPositions;

        // Renglon en el que se encuentra el ratón
        public int CurrentRow { get; set; }
        // Columna en el que se encuentra el ratón
        public int CurrentCol { get; set; }

        // Seleccion actual
        public int SelectedType { get; set; }

        // Event handler
        MapManagerCallbackEvent _callback;


        /// <summary>
        /// Crea un editor de mapa
        /// </summary>
        /// <param name="cols">Columnas totales del mapa</param>
        /// <param name="rows">Renglones totales del mapa</param>
        /// <param name="texturas">Diccionario de texturas</param>
        /// <param name="callback">Callback para manejar eventos</param>
        public MapManager(int cols, int rows, Dictionary<string, Texture2D> texturas, MapManagerCallbackEvent callback = null)
        {
            this.texturas = texturas;
            setSize(cols, rows);
            dot = texturas["dot"];

            if(callback != null)
                this._callback = callback;
        }



        /// <summary>
        /// Re establece el tamaño del mapa, si contiene datos no los sobre escribe 
        /// trata de mantener el estado original
        /// </summary>
        /// <param name="cols">Nuevo número de columnas</param>
        /// <param name="rows">Nuevo número de renglones</param>
        public void setSize(int cols, int rows)
        {
            Tile[,] copy = tiles;
            tiles = new Tile[cols, rows];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (copy != null && copy.GetLength(0) > col && copy.GetLength(1) > row)
                        tiles[col, row] = copy[col, row];

                    else
                        tiles[col, row] = new Tile(new Vector2(col, row), texturas["nonType"], 0);
                }
            }
        }

        public Vector2 GetSize()
        {
            return new Vector2(tiles.GetLength(0), tiles.GetLength(1));
        }


        /// <summary>
        /// Hacer una referencia en el mapa donde el camino inicia
        /// </summary>
        /// <param name="col">Posición en la columna</param>
        /// <param name="row">Posición en el renglon</param>
        public void setStartMap(int col, int row)
        {
            // si lastStart aún no tiene valor
            if (lastStart != -Vector2.One)
            {
                int x = (int)lastStart.X;
                int y = (int)lastStart.Y;
                tiles[x, y].decoration = null;
                tiles[x, y].identifier = "Terrain";
                lastStart = new Vector2(CurrentCol, CurrentRow);
            }
            // si lastStart es nuevo y no tiene referencias extra
            if (tiles[col, row].identifier == "Terrain")
            {
                tiles[col, row].decoration = texturas["start"];
                tiles[col, row].identifier = "Terrain-Start";
                lastStart = new Vector2(CurrentCol, CurrentRow);
            }
        }

        public void setEndMap(int col, int row)
        {
            if (lastEnd != -Vector2.One)
            {
                int x = (int)lastEnd.X;
                int y = (int)lastEnd.Y;
                tiles[x, y].decoration = null;
                tiles[x, y].identifier = "Terrain";
                lastEnd = new Vector2(CurrentCol, CurrentRow);
            }
            if (tiles[col, row].identifier == "Terrain")
            {
                tiles[col, row].decoration = texturas["end"];
                tiles[col, row].identifier = "Terrain-End";
                lastEnd = new Vector2(CurrentCol, CurrentRow);
            }

            getPath();

        }

        // Lista de posiciones para dibujar lineas según el path del mapa
        public List<Vector2> pathPointLines = new List<Vector2>();

        /// <summary>
        /// Obtiene un path y crea puntos entre cada tile del path
        /// </summary>
        public void getPath()
        {

            Path p = new Path(this.tiles);
            pathPositions = p.createPath();

            pathPointLines.Clear();

            // Por cada dos tiles crea una linea entre medio
            for (int i = 0; i < pathPositions.GetLength(0) - 1; i++)
            {

                Vector2 a = new Vector2();
                a.X = pathPositions[i, 0] * 100 + 50;
                a.Y = pathPositions[i, 1] * 100 + 50;

                Vector2 b = new Vector2();
                b.X = pathPositions[i + 1, 0] * 100 + 50;
                b.Y = pathPositions[i + 1, 1] * 100 + 50;
                

                Vector2 pos = a, vel = Vector2.Zero;
                vel = Calculus.velFromPoints(a, b, 4);

                while (true)
                {

                    // Si la distancia entre el punto acutal al objetivo es menor a 1 
                    // entonces se termina el ciclo
                    if (Calculus.distanceFromPoints(pos, b) < 1)
                        break;

                    pathPointLines.Add(pos);
                    pos += vel;
                }

            }
            if(_callback != null && pathPointLines.Count>0)
                _callback("end", true);
        }

        /// <summary>
        /// Añade un tile con el tipo, columna y renglon seleccionado
        /// </summary>
        public void addTo()
        {
            if (CurrentCol >= 0 && CurrentRow >= 0 &&
                CurrentCol < GetSize().X && CurrentRow < GetSize().Y)
            {

                int type = SelectedType;

                // obtiene el valor de textura según el tipo seleccionado
                Texture2D textSelection = 
                    (type == 0 ? texturas["nonType"] : (type == 1 ? texturas["type1"] : texturas["type2"]));

                // Indicadores del tile seleccionado
                Vector2 positionTile = new Vector2(CurrentCol, CurrentRow);

                // Según el tipo seleccionado cambiar los valores del arreglo Tile
                switch (SelectedType)
                {
                    case TypeSelection.VOID:
                        tiles[CurrentCol, CurrentRow] = new Tile(positionTile, textSelection, 0);
                        break;

                    case TypeSelection.GRASS:
                        tiles[CurrentCol, CurrentRow] = new Tile(positionTile, textSelection, 1, "Grass");
                        break;

                    case TypeSelection.TERRAIN:
                        tiles[CurrentCol, CurrentRow] = new Tile(positionTile, textSelection, 2, "Terrain");
                        break;
                    case TypeSelection.START:
                        setStartMap(CurrentCol, CurrentRow);
                        break;
                    case TypeSelection.END:
                        setEndMap(CurrentCol, CurrentRow);
                        break;
                        
                }

            }

        }

        /// <summary>
        /// Añade un tile con el tipo, columna y renglon especificado
        /// </summary>
        /// <param name="col">Columna especifica en el mapa</param>
        /// <param name="row">Renglon especifico en el mapa</param>
        /// <param name="type">Tipo de tile especifico para el mapa</param>
        public void addTo(int col, int row, int type)
        {
            if (col >= 0 && row >= 0 &&
                col < tiles.GetLength(0) && row < tiles.GetLength(1))
            {

                //int type = SelectedType;

                // obtiene el valor de textura según el tipo seleccionado
                Texture2D textSelection =
                    (type == 0 ? texturas["nonType"] : (type == 1 ? texturas["type1"] : texturas["type2"]));

                // Indicadores del tile seleccionado
                Vector2 positionTile = new Vector2(col, row);

                // Según el tipo seleccionado cambiar los valores del arreglo Tile
                switch (type)
                {
                    case TypeSelection.VOID:
                        tiles[col, row] = new Tile(positionTile, textSelection, 0);
                        break;

                    case TypeSelection.GRASS:
                        tiles[col, row] = new Tile(positionTile, textSelection, 1, "Grass");
                        break;

                    case TypeSelection.TERRAIN:
                        tiles[col, row] = new Tile(positionTile, textSelection, 2, "Terrain");
                        break;

                    case TypeSelection.START:
                        setStartMap(col, row);
                        break;
                    case TypeSelection.END:
                        setEndMap(col, row);
                        break;

                }

            }

        }

        public void randomGen()
        {
            Random r = new Random();
            for (int i = 0; i < tiles.GetLength(0) * tiles.GetLength(1); i++)
            {
                int col = i % tiles.GetLength(0);
                int row = i / tiles.GetLength(0);
               
                int x = r.Next(1,3);
                addTo(col, row, x);
            }
        }

        /// <summary>
        /// Sobre escribe los valores del mapa con el tipo especificado
        /// </summary>
        /// <param name="type">Tipo de tile con el que se va a sobre escribir el mapa</param>
        /// <param name="replaceAll">Reemplazar todos los valores sin excepciones</param>
        public void clearWith(int type, bool replaceAll = true)
        {
            
            Texture2D textureSelection = null;
            int idSelection = 0;
            string identifier = "";

            textureSelection = (type == 0 ?
                texturas["nonType"] :
                (type == 1 ? texturas["type1"] : texturas["type2"]));

            idSelection = (type == 0 ? 0 : (type == 1 ? 1 : 2));

            identifier = (type == 0 ? "Default" : (type == 1 ? "Grass" : "Terrain"));

            foreach (Tile t in tiles)
            {
                if (replaceAll)
                {
                    t.textureID = idSelection;
                    t.textura = textureSelection;
                    t.decoration = null;
                    t.identifier = identifier;
                }
                // Si no debe reemplazar todo solo cambia los tiles con id de textura = 0
                else if (t.textureID == 0)
                {
                    t.textureID = idSelection;
                    t.textura = textureSelection;
                    t.identifier = identifier;
                }
            }
        }


        public void ReadFile(string fileDir = "data.txt")
        {
            try
            {
                StreamReader sr = new StreamReader(fileDir);

                int cols = Int32.Parse(sr.ReadLine());
                int rows = Int32.Parse(sr.ReadLine());

                tiles = new Tile[cols, rows];

                for (int col = 0; col < cols; col++)
                {
                    for (int row = 0; row < rows; row++)
                    {
                        float xPos = float.Parse(sr.ReadLine());
                        float yPos = float.Parse(sr.ReadLine());
                        int textureId = Int32.Parse(sr.ReadLine());
                        string identifier = sr.ReadLine();

                        Texture2D textSelection = (textureId == 0 ? texturas["nonType"] : (textureId == 1 ? texturas["type1"] : texturas["type2"]));

                        tiles[col, row] = new Tile(new Vector2(xPos / 100, yPos / 100), textSelection, textureId, identifier);

                        if (identifier.Contains("-"))
                        {
                            Texture2D _decoration = (identifier.ToLower().Contains("start") ? texturas["start"] : texturas["end"]);
                            tiles[col, row].decoration = _decoration;
                        }

                        


                    }
                }

                sr.Close();

            }
            catch (Exception ex)
            {

            }
        }

        public void WriteToFile()
        {
            try
            {
                StreamWriter sw = new StreamWriter("data.txt");

                sw.WriteLine(tiles.GetLength(0));
                sw.WriteLine(tiles.GetLength(1));

                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        sw.WriteLine(tiles[i, j].position.X);
                        sw.WriteLine(tiles[i, j].position.Y);
                        sw.WriteLine(tiles[i, j].textureID);
                        sw.WriteLine(tiles[i, j].identifier);
                    }
                }

                foreach (Vector2 v in pathPointLines)
                {
                    sw.WriteLine(v.X + ", " + v.Y);
                }

                sw.Close();


            }
            catch (Exception ex)
            {

            }
        }

        public void DrawLine(SpriteBatch sp, Vector2 start, Vector2 end, Color color)
        {
            float length = (end - start).Length();
            float rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            sp.Draw(this.dot, start, null, color, rotation, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch sp, Vector2 mouseCenter)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(sp);
            }

            if (SelectedType != 0 && SelectedType < 4)
            {
                sp.Draw(
                    (SelectedType == 1 ? texturas["type1"] :
                    (SelectedType == 2 ? texturas["type2"] :
                    (SelectedType == 3 ? texturas["start"] : texturas["end"] ))), mouseCenter, Color.White * 0.7f);
                sp.Draw(texturas["nonType"], mouseCenter, Color.Red);
            }

            if (pathPositions == null)
                return;

            foreach (Vector2 v in pathPointLines)
                sp.Draw(dot, v, Color.White);
        }
    }
}

