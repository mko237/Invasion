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
        private int ID = new int();
        SpriteFont text = Art.Font;
        Vector2 centerTextOffset;
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
           Vector2 textSize = text.MeasureString(ID.ToString());
           centerTextOffset = textSize * size / 2;

        }
        public Planet(Vector2 position, Color col, float size, float radius, int id)
        {
            Position = position;
            color = col;
            ObjectSize = size;
            Radius = radius;
            image = Art.Planet;
            state = State.NEUTRAL;
            productionRate = productionAccel * Radius;
            ID = id;
            Vector2 textSize = text.MeasureString(ID.ToString());
            centerTextOffset = textSize * size / 2;

        }

        public override void Update()
        {
            //planet logic here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, ObjectSize, 0, 0);
            
            spriteBatch.DrawString(text, ID.ToString(), Position-centerTextOffset, Color.White,Orientation,new Vector2(0,0),ObjectSize,0,0);
            //Console.WriteLine("planet position : " + Position + "PlanetSize: " + ObjectSize);
        }

            
    }
}
