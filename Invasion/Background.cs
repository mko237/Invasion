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
        private static Rectangle imageBox = new Rectangle(500,500,GameRoot.Viewport.Width,GameRoot.Viewport.Height);
       
        public Background()
        {
            image = Art.Background;
            Position = GameRoot.ScreenSize / 2;
            Orientation = 360f;
        }

        public override void Update()
        {
            
                if(Orientation <= 0)
                    Orientation = 360f;
                else
                    Orientation -= .00005f;
               
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            
            if (image != null)
                spriteBatch.Draw(image, Position, null, Color.White, Orientation, Size / 2f,ObjectSize*2.5f,0,0);
            else
                Console.WriteLine("image was null");
        }
    }

}
