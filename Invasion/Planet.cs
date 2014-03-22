using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invasion
{
    //this is mikes comment
    public class Planet : Entity
    {
        private int ID = new int();
        SpriteFont text = Art.Font;
        Vector2 centerTextOffset;

        public string SHUT_THE_FUCK = "<----";

        public enum State
        {
            NEUTRAL,
            OCCUPIED
        }

        private const float productionAccel = 2.0f;
        private float productionRate;
        private float shipCount;
        private Team occupiedTeam;

        State state { get; set; }

        public Planet()
        {
            //initialize planet here
            image = Art.Planet;
            Position = GameRoot.ScreenSize / 2;
        }

        public Planet(Vector2 position, Color col, float size, int id, Team team)
        {
            Position = position;
            color = col;
            ObjectSize = size;
            Radius = 0.5f * LevelSpawner.imageSize * size;
            image = Art.Planet;
            productionRate = productionAccel * 2 * Radius - 50;
            ID = id;
            Vector2 textSize = text.MeasureString(ID.ToString());
            centerTextOffset = textSize * size / 2;
            occupiedTeam = team;
        }

        //public void changeShipCount(Ship ship, int n)
        //{
        //    if (ship.Team == occupiedTeam)
        //    {
        //        shipCount += n;
        //    }
        //    else
        //    {
        //        shipCount -= n;
        //    }
        //}

        public override void Update()
        {
            if (occupiedTeam != null)
            {
                Console.WriteLine(shipCount);
                shipCount += (productionRate / 3600);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, ObjectSize, 0, 0);  
            spriteBatch.DrawString(text, ID.ToString(), Position-centerTextOffset, Color.White,Orientation,new Vector2(0,0),ObjectSize,0,0);
        }

            
    }
}
