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


    }
}
