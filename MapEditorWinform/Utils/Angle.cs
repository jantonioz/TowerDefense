using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorWinform.Utils
{
    public class Angle
    {
        private double rad;
        private double deg;
        private Vector2 aPos, bPos;


        public Angle(Vector2 a, Vector2 b)
        {
            // get rad angle 
            // the value is -3 to 3
            rad = Calculus.angleFrom(a, b);
            deg = Calculus.RadianToDegree(rad);
            aPos = a;
            bPos = b;

            if (rad < 0)
                rad += Math.PI * 2;

            deg = Calculus.RadianToDegree(rad);
        }

        public Angle(float rad)
        {
            Update(rad);
        }

        public Angle(double rad)
        {
            //if(rad > Math.PI *2)
            //{
            //    int e = (int)(rad / Math.PI * 2);
            //    rad -= e * Math.PI * 2;
            //}
            Update(rad);
        }

        public void Update(double val)
        {
            this.rad = val;
            this.deg = Calculus.RadianToDegree(val);
        }


        public Angle()
        {
            this.rad = 0;
            this.deg = 0;
        }

        public float GetRadians()
        {
            return (float)rad;
        }

        public float GetDegrees()
        {
            return (float)deg;
        }

        public void SetRadians(float radians)
        {
            Update(radians);
        }

        public void SetDegrees(float degrees)
        {
            Update(Calculus.DegreeToRadian(degrees));
            this.deg = degrees;
        }

        public int GetCuadrant()
        {
            int c = 0;
            if (rad < 0 && rad >= -Math.PI / 2)
                return 1;
            else if (rad < -Math.PI / 2 && rad >= -Math.PI)
                return 2;
            else if (rad < Math.PI && rad >= Math.PI / 2)
                return 3;
            else if (rad >= 0 && rad < Math.PI / 2)
                return 4;
            return c;

        }


        public void CloserTo(Angle aimAngle)
        {

            float deg1 = aimAngle.GetDegrees();
            float deg2 = GetDegrees();


            if (deg2 > 360)
                deg2 -= 360;

            if (deg1 < 0)
                deg1 += 360;


            double dist = deg1 - deg2;
            if (dist < 0 && dist > -180)
                SetDegrees(deg2 - 2);

            else if (dist > 0 && dist < 180)
                SetDegrees(deg2 + 2);

            if (dist < 0 && dist < -180)
                SetDegrees(deg2 + 2);

            else if (dist > 0 && dist > 180)
                SetDegrees(deg2 - 2);


            if (GetDegrees() < 0)
                SetDegrees(GetDegrees() + 360);

        }


        public  bool isInsideCone(Angle currentAngle, int v)
        {
            double left, right;

            left  = currentAngle.GetDegrees() - v / 2;
            right = currentAngle.GetDegrees() + v / 2;


            if (GetDegrees() > left && GetDegrees() < right)
                return true;
            return false;

            

        }


        private float Abs(float val)
        {
            return (float)Math.Abs(val);
        }

        private void SwapRads(ref float rad1, ref float rad2)
        {
            float tmp = rad1;
            rad1 = rad2;
            rad2 = tmp;
        }

        
    }
}
