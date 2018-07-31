using MapEditorWinform.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorWinform.Turrets
{
    class Bullet
    {
        public Vector2 Position;
        public Vector2 CenterPosition;
        public Texture2D textura;
        public Rectangle rect;
        public int indexTarget;
        public bool Visible;

        public Bullet(Texture2D texture, Vector2 StartPostion, int indexTarget=-1)
        {
            this.Position = StartPostion;
            this.textura = texture;
            this.CenterPosition = StartPostion - new Vector2(textura.Width / 2, textura.Height / 2);
            this.rect = new Rectangle((int)Position.X, (int)Position.Y, textura.Width, textura.Height);
            Visible = true;
            this.indexTarget = indexTarget;
        }

        public void Update(Vector2? target)
        {

            Position += Calculus.velFromPoints(Position, target?? Position, 10);
            CenterPosition = Position - new Vector2(textura.Width / 2, textura.Height / 2);
            rect = new Rectangle((int)Position.X, (int)Position.Y, textura.Width, textura.Height);

            if(Calculus.distanceFromPoints(Position, target ?? Position) < 5)
            {
                Visible = false;
            }
        }


        public void Draw(SpriteBatch sp)
        {
            sp.Draw(texture: textura, position: CenterPosition, scale: new Vector2(0.5f, 0.5f), color: Color.White);
        }
    
    }
}
