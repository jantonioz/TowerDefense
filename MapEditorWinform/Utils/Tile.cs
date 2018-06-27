using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorWinform.Utils
{
    public class Tile
    {
        public Vector2 positionGrid, position;
        public Texture2D textura, decoration;
        public int textureID;
        public string identifier;

        public Tile(Vector2 positionGrid, Texture2D texture, int textureID)
        {
            this.positionGrid = positionGrid;
            this.position = new Vector2(positionGrid.X * 100, positionGrid.Y * 100);
            this.textura = texture;
            this.textureID = textureID;
            this.identifier = "Default";
            //this.identifier = identifier;
        }

        public void Draw(SpriteBatch sp)
        {
            if (textura == null)
                return;
            if(textureID == 0)
                sp.Draw(textura, position, Color.Black);
            else
                sp.Draw(textura, position, Color.White);

            if (identifier.ToLower().Contains("start") || identifier.ToLower().Contains("end"))
                sp.Draw(decoration, position, Color.White);

        }
    }
}
