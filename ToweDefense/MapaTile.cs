using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToweDefense
{
    public class MapaTile
    {
        public Texture2D textura;
        public Vector2 position;
        public int xGrid, yGrid;
        public string type;

        public MapaTile(Texture2D textura, string type, Vector2? position = null , int? xGrid = null, int? yGrid = null)
        {
            this.type = type;
            this.textura = textura;
            if (position != null && xGrid != null && yGrid != null)
            {
                this.position = (Vector2)position;
                this.xGrid = (int)xGrid;
                this.yGrid = (int)yGrid;
            }
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Draw(textura, position, Color.White);
        }
    }
}
