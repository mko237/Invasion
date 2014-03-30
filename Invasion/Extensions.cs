using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Invasion
{
   static class Extensions
    {
       public static float ToAngle(this Vector2 vector)
       {
           return (float)Math.Atan2(vector.Y, vector.X);
       }
       public static Point ToPoint(this Vector2 vector)
       {
           return new Point((int)vector.X, (int)vector.Y);
       }
       public static bool WithinRadius(this Vector2 vector, Vector2 center, float radius)
       {
           bool inRadius;
           float distance = Vector2.DistanceSquared(vector,center);
           if (distance <= radius*radius)
               inRadius = true;
           else
               inRadius = false;
           return inRadius;
       }
       public static void ReconstructPath(this List<Vector2> list, Planet source, Planet destination, int n)
       {
           bool reverse = false;

           if (source.Position.X > destination.Position.X)
               reverse = true;

           if (reverse)
               list.Reverse();

           //#region Bezier Curve
           //BezierCurve bc = new BezierCurve();

           //List<float> tempList = new List<float>();
           //for (int i = 0; i < list.Count; i++)
           //{
           //    tempList.Add(list[i].X);
           //    tempList.Add(list[i].Y);
           //}

           //list.Clear();
           //list.TrimExcess();

           //int POINTS_ON_CURVE = n;

           //float[] ptind = new float[tempList.Count];
           //float[] p = new float[POINTS_ON_CURVE];
           //tempList.CopyTo(ptind, 0);

           //bc.Bezier2D(ptind, (POINTS_ON_CURVE) / 2, p);

           //for (int i = 1; i < POINTS_ON_CURVE - 1; i += 2)
           //   list.Add(new Vector2(p[i + 1], p[i]));
           //#endregion

           #region Spline Stuff
           float xDistance = Math.Abs(list.First().X - list.Last().X);

           float[] x = list.ExtractValues('x');
           float[] y = list.ExtractValues('y');

           list.Clear();
           list.TrimExcess();

           int nInterpolated = (int)(xDistance / 5);
           float[] xs = new float[nInterpolated];

           for (int i = 0; i < nInterpolated; i++)
               xs[i] = ((float)i / (float)(nInterpolated - 1)) * xDistance + x[0];


           CubicSpline spline = new CubicSpline();
           float[] ys = spline.FitAndEval(x, y, xs, Single.NaN, Single.NaN, false);

           for (int j = 0; j < nInterpolated; j++)
           {
               Vector2 point = new Vector2(xs[j], ys[j]);
               foreach (Planet planet in EntityManager.planets)
               {
                   if (planet.ID != source.ID && planet.ID != destination.ID)
                   {
                       if ((point - planet.Position).LengthSquared() < 2.25f * planet.Radius * planet.Radius)
                       {
                           Vector2 direction = point - planet.Position;
                           direction.Normalize();
                           point = new Vector2(planet.Position.X + (direction * 1.5f * planet.Radius).X, planet.Position.Y + (direction * 1.5f * planet.Radius).Y);
                       }
                   }
               }
               list.Add(point);
           }

           if (reverse)
               list.Reverse();

           #endregion
       }
       
       public static float[] ExtractValues(this List<Vector2> list, char var)
       {
           float[] values = new float[list.Count];
           if (var == 'x')
           {
               for (int i = 0; i < list.Count; i++)
                   values[i] = list[i].X;
           }
           else if (var == 'y')
           {
               for (int i = 0; i < list.Count; i++)
                   values[i] = list[i].Y;
           }
           return values;
       }
       public static float NextFloat(this Random rand, float minValue, float maxValue)
       {
           return (float)rand.NextDouble() * (maxValue - minValue) + minValue;
       }
       public static Vector2 NextVector2(this Random rand, float minLength, float maxLength)
       {
           double theta = rand.NextDouble() * 2 * Math.PI;
           float length = rand.NextFloat(minLength, maxLength);
           return new Vector2(length * (float)Math.Cos(theta), length * (float)Math.Sin(theta));
       }
       public static float ConvertToGlobal(this float orientation, Vector2 origin)
       {
           return origin.ToAngle() + orientation;
       }
    }
}
