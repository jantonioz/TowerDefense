using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorWinform.Utils
{
    public class MapManager
    {
        Tile[,] tiles;
        Dictionary<string, Texture2D> texturas;
        public Vector2 lastStart;
        public Vector2 lastEnd;

        public int CurrentRow { get; set; }
        public int CurrentCol { get; set; }
        public int SelectedType { get; set; }

        public MapManager(int cols, int rows, Dictionary<string, Texture2D> texturas)
        {
            this.texturas = texturas;
            setSize(cols, rows);
        }




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

        public void setStartMap(int col, int row)
        {

            if (lastStart != null)
            {
                int x = (int)lastStart.X;
                int y = (int)lastStart.Y;
                tiles[x, y].decoration = null;
                tiles[x, y].identifier = "Terrain";
                lastStart = new Vector2(CurrentCol, CurrentRow);
            }
            if (tiles[col, row].identifier == "Terrain")
            {
                tiles[col, row].decoration = texturas["start"];
                tiles[col, row].identifier = "Terrain-Start";
            }


        }

        public void setEndMap(int col, int row)
        {
            if (lastEnd != null)
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
            }

        }


        public void addTo()
        {
            if (CurrentCol >= 0 && CurrentRow >= 0 &&
                CurrentCol < tiles.GetLength(0) && CurrentRow < tiles.GetLength(1))
            {
                int type = SelectedType;
                Texture2D textSelection = (type == 0 ?
                texturas["nonType"] :
                (type == 1 ? texturas["type1"] : texturas["type2"]));

                Vector2 positionTile = new Vector2(CurrentCol, CurrentRow);


                switch (SelectedType)
                {
                    case 0:
                        tiles[CurrentCol, CurrentRow] = new Tile(new Vector2(CurrentCol, CurrentRow), textSelection, 0);
                        break;

                    case 1:
                        tiles[CurrentCol, CurrentRow] = new Tile(new Vector2(CurrentCol, CurrentRow), textSelection, 1);
                        break;

                    case 2:
                        tiles[CurrentCol, CurrentRow] = new Tile(new Vector2(CurrentCol, CurrentRow), textSelection, 2);
                        break;
                    case 3:
                        setStartMap(CurrentCol, CurrentRow);
                        break;
                    case 4:
                        setEndMap(CurrentCol, CurrentRow);
                        break;

                }

            }



        }

        public void clearWith(int type, bool replaceAll = true)
        {
            Texture2D textSelection = null;
            int idSelection = 0;
            string identifier = "";

            textSelection = (type == 0 ?
                texturas["nonType"] :
                (type == 1 ? texturas["type1"] : texturas["type2"]));

            idSelection = (type == 0 ? 0 : (type == 1 ? 1 : 2));

            identifier = (type == 0 ? "Default" : (type == 1 ? "Grass" : "Terrain"));

            foreach (Tile t in tiles)
            {
                if (replaceAll)
                {
                    t.textureID = idSelection;
                    t.textura = textSelection;
                    t.decoration = null;
                    t.identifier = identifier;
                }
                else if (t.textureID == 0)
                {
                    t.textureID = idSelection;
                    t.textura = textSelection;
                    t.identifier = identifier;
                }
            }
        }




        public void Draw(SpriteBatch sp, Vector2 mouseCenter)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(sp);
            }

            if (SelectedType != 0)
            {
                sp.Draw(
                    (SelectedType == 1 ? texturas["type1"] :
                    (SelectedType == 2 ? texturas["type2"] :
                    (SelectedType == 3 ? texturas["start"] :
                    (SelectedType == 4 ? texturas["end"] : null)))), mouseCenter, Color.White * 0.7f);
                sp.Draw(texturas["nonType"], mouseCenter, Color.Red);
            }

        }
    }
}
