using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorWinform.Utils
{
    public class HUD
    {

        public Texture2D hudHolder, t1, t2, t3, t4, t5;
        public Texture2D hudSelector;

        private Vector2 scale;
        public int typeSelection = 0;
        private Vector2 screenSize;

        public HUD(Dictionary<int, Texture2D> textureList)
        {
            hudHolder = textureList[0];
            t1 = textureList[1];
            t2 = textureList[2];
            t3 = textureList[3];
            t4 = textureList[4];
            t5 = textureList[5];
            hudSelector = textureList[6];
        }

        KeyboardState lkst, kst;
        public void Update(int x = 0, int y = 0)
        {
            lkst = kst;
            kst = Keyboard.GetState();

            if (x != 0 && y != 0)
                screenSize = new Vector2(x, y);

            if (lkst.IsKeyDown(Keys.D1) && kst.IsKeyDown(Keys.D1))
            {
                typeSelection = TypeSelection.VOID;
            } else if (lkst.IsKeyDown(Keys.D2) && kst.IsKeyDown(Keys.D2))
            {
                typeSelection = TypeSelection.GRASS;
            }
            else if (lkst.IsKeyDown(Keys.D3) && kst.IsKeyDown(Keys.D3))
            {
                typeSelection = TypeSelection.TERRAIN;
            }
            else if (lkst.IsKeyDown(Keys.D4) && kst.IsKeyDown(Keys.D4))
            {
                typeSelection = TypeSelection.START;
            }
            else if (lkst.IsKeyDown(Keys.D5) && kst.IsKeyDown(Keys.D5))
            {
                typeSelection = TypeSelection.END;
            }
        }


        public void Draw(SpriteBatch sp)
        {
            Vector2 StartPos = new Vector2(screenSize.X/ 2 - (this.hudHolder.Width/2), screenSize.Y - 120);


            scale = new Vector2(0.7f, 0.7f);


            float xStart = StartPos.X;
            float wBlock = 0 * 105 * scale.X + 5 * scale.X;
            StartPos.Y += 5 * scale.Y;

            StartPos.X = xStart + wBlock;
            sp.Draw(texture: t1, position: StartPos, color: Color.White, scale: scale);


            wBlock = 1 * 105 * scale.X + 5 * scale.X;
            StartPos.X = xStart + wBlock;
            sp.Draw(texture: t2, position: StartPos, color: Color.White, scale: scale);

            wBlock = 2 * 105 * scale.X + 5 * scale.X;
            StartPos.X = xStart + wBlock;
            sp.Draw(texture: t3, position: StartPos, color: Color.White, scale: scale);


            wBlock = 3 * 105 * scale.X + 5 * scale.X;
            StartPos.X = xStart + wBlock;
            sp.Draw(texture: t4, position: StartPos, color: Color.White, scale: scale);


            wBlock = 4 * 105 * scale.X + 5 * scale.X;
            StartPos.X = xStart + wBlock;
            sp.Draw(texture: t5, position: StartPos, color: Color.White, scale: scale);


            StartPos = new Vector2(screenSize.X / 2 - (this.hudHolder.Width / 2), screenSize.Y - 120);

            sp.Draw(texture: hudHolder, position: StartPos, color: Color.White, scale: new Vector2(0.7f, 0.7f));

            if (typeSelection > 4)
                return;
            sp.Draw(texture: hudSelector, position: StartPos + new Vector2(typeSelection * 105 * .7f, 0), color: Color.White, scale: scale);

        }
    }
}
