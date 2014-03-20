using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invasion
{
    class Background : Entity
    {
        private static Rectangle imageBox = new Rectangle();
        public Background()
        {
            image = Art.Planet;
            Position = GameRoot.ScreenSize / 2;
            Orientation = 360f;
        }

        public void Update()
        {
                   
                if(Orientation <= 0)
                    Orientation -= .05f;
                else
                    Orientation = 360;
               
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }

}
