using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invasion
{
    public class Planet : Entity
    {

        public enum State
        {
            RED,
            BLUE,
            NEUTRAL
        }

        private static float productionAccel = 0.5f;

        private float productionRate;

        State state { get; set; }

        public Planet()
        {
            //initialize planet here
            image = Art.Planet;
            Position = GameRoot.ScreenSize / 2;
        }

        public Planet(Vector2 position, Color col, float size, float radius)
        {
           Position = position;
           color = col;
           ObjectSize = size;
           Radius = radius;
           image = Art.Planet;
           state = State.NEUTRAL;
           productionRate = productionAccel * Radius;
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
