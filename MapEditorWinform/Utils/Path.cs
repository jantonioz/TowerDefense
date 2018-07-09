using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorWinform.Utils
{
    class Path
    {
        private Tile[,] tiles;
        private int[,] gridPositions;


        public struct Index
        {
            public int x, y;

            public Index(int x = -1, int y = -1)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// Genera posiciones ordenadas de un path desde el inicio del mapa al final
        /// </summary>
        /// <param name="mapTiles">Arreglo de Tile del mapa</param>
        public Path(Tile[,] mapTiles)
        {
            tiles = new Tile[mapTiles.GetLength(0), mapTiles.GetLength(1)];
            for (int i = 0; i < mapTiles.GetLength(0); i++)
                for (int j = 0; j < mapTiles.GetLength(1); j++)
                    tiles[i, j] = mapTiles[i, j];

        }

        /// <summary>
        /// Busca el tile con el identificador "End"
        /// </summary>
        /// <returns>Struct con posiciones del arreglo Tile</returns>
        private Index getEnd()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j].identifier.ToLower().Contains("end"))
                        return new Index(i, j);
                }
            }

            return new Index();
        }

        /// <summary>
        /// Busca el tile con el identificador "Start"
        /// </summary>
        /// <returns>Struct con posiciones del arreglo Tile</returns>
        private Index getStart()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j].identifier.ToLower().Contains("start"))
                        return new Index(i, j);
                }
            }

            return new Index();
        }

        /// <summary>
        /// Crea un patrón del mapa desde el inicio hasta el final
        /// </summary>
        /// <returns>Arreglo de las posiciones de los tiles en el path</returns>
        public int[,] createPath()
        {
            List<Index> idxs = searchAround();
            if (idxs == null)
                return null;
            int[,] a = new int[idxs.Count, 2];
            for (int i = 0; i < idxs.Count; i++)
            {
                a[i, 0] = idxs[i].x;
                a[i, 1] = idxs[i].y;
            }
            return a;
        }

        /// <summary>
        /// Comprueba que el tile tiene un identificador "Terrain" que es
        /// lo que se busca para continuar con el path
        /// </summary>
        /// <param name="x">Columna</param>
        /// <param name="y">Renglon</param>
        /// <returns></returns>
        public bool isTerrain(int x, int y)
        {
            if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1))
                return false;
            return (tiles[x, y].isTerrain());
        }

        /// <summary>
        /// Según el tile actual busca en las 4 posiciones laterales para crear un nuevo tile actual
        /// y así tener el path
        /// </summary>
        /// <returns></returns>
        private List<Index> searchAround()
        {
            // indicadores del inicio y final del path
            Index start = getStart();
            Index end = getEnd();

            // si el mapa no tiene indicadores por default obtendrán valores X:-1 Y:-1
            if (start.x == -1 && end.x == -1)
                return null;

            // Lista para guardar las posiciones del path
            List<Index> indexs = new List<Index>();

            // Se agrega primero el inicio que sería el primer tile del path
            indexs.Add(start);

            // Posiciones del último Tile, con el que se busca en las 4 posiciones laterales
            Index lastTile = start;


            // hasta que se llegue a un punto muerto o al final del mapa
            bool hasChanges = true;
            while (hasChanges)
            {

                hasChanges = false;
                int col = lastTile.x, row = lastTile.y;

                // comprueba que está en el tile del final del mapa
                if (col == end.x && row == end.y)
                    break;


                // look for right block
                if (isTerrain(col + 1, row) && !indexs.Contains(new Index(col + 1, row))) {
                    lastTile = new Index(col + 1, row);
                    indexs.Add(lastTile);
                    hasChanges = true;
                }
                //look for top block
                else if (isTerrain(col, row - 1) && !indexs.Contains(new Index(col, row - 1))) {
                    lastTile = new Index(col, row - 1);
                    indexs.Add(lastTile);
                    hasChanges = true;
                }
                // look for bottom block
                else if (isTerrain(col, row + 1) && !indexs.Contains(new Index(col, row + 1))) {
                    lastTile = new Index(col, row + 1);
                    indexs.Add(lastTile);
                    hasChanges = true;
                }
                // look for left block
                else if (isTerrain(col - 1, row) && !indexs.Contains(new Index(col - 1, row))) {
                    lastTile = new Index(lastTile.x - 1, lastTile.y);
                    indexs.Add(lastTile);
                    hasChanges = true;
                }
            }

            return indexs;

        }
    }
}
