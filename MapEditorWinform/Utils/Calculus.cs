using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorWinform.Utils
{
    public class Calculus
    {

        internal static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        internal static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        internal static double distanceFromPoints(Vector2 p1, Vector2 p2)
        {
            return Math.Sqrt( Math.Pow(p2.X - p1.X , 2 ) + Math.Pow(p2.Y - p1.Y, 2));
        }

        internal static Vector2 velFromPoints(Vector2 p1, Vector2 p2, float multiplier = 1f)
        {

            float x1 = p1.X;
            float x2 = p2.X;

            float y1 = p1.Y;
            float y2 = p2.Y;

            float deltaX = x2 - x1;
            float deltaY = y2 - y1;

            float angle = (float)Math.Atan2(deltaY, deltaX);

            Vector2 vel = new Vector2(0, 0);
            vel.X = multiplier * (float)Math.Cos(angle);
            vel.Y = multiplier * (float)Math.Sin(angle);
            return vel;
        }

        internal static double angleFrom(Vector2 p1, Vector2 p2)
        {

            float x1 = p1.X;
            float x2 = p2.X;

            float y1 = p1.Y;
            float y2 = p2.Y;

            float deltaX = x2 - x1;
            float deltaY = y2 - y1;

            double angle = Math.Atan2(deltaY, deltaX);
            return angle;
        }
        

    }
}
