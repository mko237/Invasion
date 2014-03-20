using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invasion
{
    class Planet : Entity
    {
        
        private static Planet instance;
        public static Planet Instance
        {
            get
            {
                if (instance == null)
                    instance = new Planet();
                return instance;
            }
        }

        public Planet()
        {
            //initialize planet here
            image = Art.Planet;
            Position = GameRoot.ScreenSize / 2;
        }
        public Planet(Vector2 position, Color col, float size)
        {
           Position = position;
           color = col;
           ObjectSize = size;
           image = Art.Planet;
        }

        public override void Update()
        {
            //planet logic here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, ObjectSize, 0, 0);
            //Console.WriteLine("planet position : " + Position + "PlanetSize: " + ObjectSize);
        }

            
    }
}
