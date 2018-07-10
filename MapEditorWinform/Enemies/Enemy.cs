using MapEditorWinform.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MapEditorWinform.Enemies
{
    public class Enemy
    {
        Texture2D texture, lifeBarTexture;
        Vector2 positionCenter, lifeBarPos;
        Vector2 Velocity, targetPoint;

        Rectangle lifeBarRect;
        float healthPoints, maxHealthPoints;
        float damagePoints;
        public bool Visible = true;


        public List<Vector2> PointsInMap { get; set; }
        public Vector2 CenterPosition { get; internal set; }

        int indexPointsList = 0;
        float reamTime = 10, totalTime = 10;
        public int ID { get; }


        public Enemy(Texture2D texture, Texture2D lifeBarTexture, Vector2 startPos, int maxHealthPoints = 300, List<Vector2> pointsinmap = null, int ID = -1)
        {
            this.texture = texture;
            this.lifeBarTexture = lifeBarTexture;
            this.positionCenter = startPos;
            this.healthPoints = maxHealthPoints;
            this.maxHealthPoints = maxHealthPoints;
            this.PointsInMap = pointsinmap;
            this.ID = ID;
            Visible = true;
        }

        public void Update(GameTime gameTime)
        {
            if (healthPoints > 0)
                healthPoints -= 0.05f;

            float lifePercent = (healthPoints * 100) / maxHealthPoints;
            lifeBarRect = new Rectangle(0, 0, (int)lifePercent / 2, 3);
            lifeBarPos = new Vector2(positionCenter.X - texture.Width / 2, positionCenter.Y - texture.Height - 5);

            if (PointsInMap.Count > 0)
                moveToNext();



            positionCenter += Velocity;
            CenterPosition = positionCenter;

        }


        public void DoDamage(float damage)
        {
            healthPoints -= damage;
            if (healthPoints <= 0)
                Visible = false;
        }


        

        private bool isOverTarget()
        {
            Rectangle r1 = new Rectangle((int)positionCenter.X - 5, (int)positionCenter.Y + 5, 5, 5);
            Rectangle r2 = new Rectangle((int)positionCenter.X - 5, (int)positionCenter.Y + 5, 5, 5);
            if (r1.Intersects(r2))
                return true;
            return false;

        }

        private float moveToNext()
        {
            if (indexPointsList < PointsInMap.Count - 1)
                indexPointsList++;
            this.positionCenter = PointsInMap[indexPointsList];

            if (indexPointsList >= PointsInMap.Count-1)
                Visible = false;


            return totalTime;

        }



        public void Draw(SpriteBatch sp)
        {
            sp.Draw(texture: texture, position: positionCenter, origin: new Vector2(texture.Width / 2, texture.Height / 2));

            sp.Draw(texture: lifeBarTexture, position: lifeBarPos, color: Color.White, sourceRectangle: lifeBarRect);


        }

    }
}
