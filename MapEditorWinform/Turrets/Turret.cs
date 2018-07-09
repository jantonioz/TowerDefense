using MapEditorWinform.Enemies;
using MapEditorWinform.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorWinform.Turrets
{
    public class Turret
    {
        int Column { get; set; }
        int Row { get; set; }

        Dictionary<int, Texture2D> textureList;

        // indices del mapa en el que está la torreta
        Vector2 mapIndex;

        // posicion real en el mapa 
        Vector2 positionGrid;

        // centro exacto del tile en el mapa
        Vector2 centerRealPosition;

        int range;
        int damage;
        float attackSpeed;
        float reamTime, TotalTimeToShoot;

        float rotation = 0;

        // compensación de giro porque la textura está girada 45°
        float rotationOffSet = (float)Calculus.DegreeToRadian(45);


        // Lista de balas 
        List<Bullet> bullets = new List<Bullet>();

        // fuente
        SpriteFont sf;


        // Current enemy
        Enemy currentEnemy;

        public Turret(Dictionary<int, Texture2D> textures, int col, int row, int range, int damage, float attackSpeed, SpriteFont sf = null)
        {
            this.textureList = textures;
            this.Column = col;
            this.Row = row;
            this.range = range;
            this.damage = damage;
            this.attackSpeed = attackSpeed;

            this.mapIndex = new Vector2(col, row);
            this.positionGrid = new Vector2(col * 100, row * 100);
            this.centerRealPosition = new Vector2(col * 100 + 50, row * 100 + 50);

            this.TotalTimeToShoot = 1 / attackSpeed; // in seconds
            this.TotalTimeToShoot *= 1000; // convert to ms

            this.sf = sf;
        }

        struct DistanceEnemy
        {
            public DistanceEnemy(int i, double d)
            {
                this.index = i;
                this.distance = d;
            }
            public int index;
            public double distance;
        }

        public void findTarget(List<Enemy> enemies)
        {
            if (enemies.Count == 0)
                return;
            List<DistanceEnemy> distancias = new List<DistanceEnemy>();
            for (int i = 0; i < enemies.Count; i++)
            {
                DistanceEnemy dE = new DistanceEnemy(i, Calculus.distanceFromPoints(centerRealPosition, enemies[i].CenterPosition));
                distancias.Add(dE);
            }



            //distancias.OrderBy(x => x.distance);
            distancias.Sort((d1, d2) => d1.distance.CompareTo(d2.distance));

            // Si la distancia al objetivo más cercano es menor de 300 pixeles entonces actualizar el enemigo 
            // actual, si no asignar valor nulo
            currentEnemy = distancias[0].distance < 300 ? enemies[distancias[0].index] : null;
            if (currentEnemy == null)
                return;

            float angle = (float)Calculus.angleFrom(centerRealPosition, currentEnemy.CenterPosition);
            rotation = angle + rotationOffSet;
        }

        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            // calcula la distancia entre los enemigos y escoge al más cercano
            findTarget(enemies);


            // Cada vez resta lo que ha tardado el juego a llegar a este punto en milisegundos
            // si la resta es menor= que 0 entonces intenta disparar si hay un objetivo
            reamTime = (reamTime <= 0 ? TryShoot(enemies) : reamTime - gameTime.ElapsedGameTime.Milliseconds);


            // Actualiza cada bala
            // para cada bala se necesita un vector2 de objetivo
            // se busca el objetivo en la lista de enemigos según "indexTarget" que contiene cada bala
            if(enemies!=null)
                bullets.ForEach(b => b.Update(enemies.Where(e => e.ID == b.indexTarget).FirstOrDefault()?.CenterPosition));

            // Elimina cada bala que alcanza su objetivo
            RemoveBullets(enemies);

            if (currentEnemy == null)
                return;


        }

        public float TryShoot(List<Enemy> enemies)
        {
            // si no hay un objetivo reiniciar el tiempo de disparo
            if (currentEnemy == null)
                return TotalTimeToShoot;


            bullets.Add(new Bullet(textureList[2], centerRealPosition, currentEnemy.ID));



            return TotalTimeToShoot;
        }


        private void RemoveBullets(List<Enemy> enemigos)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                // La bala no es visible, significa que llegó a su objetivo, lo que significa que le 
                // quita vida a el enemigo
                if (bullets[i].Visible == false)
                {
                    Enemy myTarget = enemigos.Where(e => e.ID == bullets[i].indexTarget).FirstOrDefault();
                    if (myTarget != null)
                        enemigos.Where(e => e.ID == bullets[i].indexTarget).FirstOrDefault().DoDamage(damage);
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }


        public void Draw(SpriteBatch sp)
        {
            // base de la torreta
            sp.Draw(textureList[0], positionGrid, Color.White);

            // cañon de la torreta
            sp.Draw(textureList[1], positionGrid + new Vector2(50, 50), rotation: rotation, color: Color.White, origin: new Vector2(50, 50));

            bullets.ForEach(b => b.Draw(sp));

        }
    }
}