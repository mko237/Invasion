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
        public int ID { get; private set; }
        SpriteFont text = Art.Font;
        Vector2 centerTextOffset;

        public enum State
        {
            OCCUPIED,
            NEUTRAL
        }

        private const float productionAccel = 2.5f;
        private float productionRate;
        public float shipCount { get; set; }
        public Team team;
        public State state { get; set; }

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
            Radius = (0.5f * LevelSpawner.imageSize * size);
            image = Art.Planet;
            productionRate = productionAccel * 2 * Radius - 50; 
            ID = id;
            Vector2 textSize = text.MeasureString(ID.ToString());
            centerTextOffset = (textSize) * size / 2;
            this.team = team;

            if (team == null)
            {
                state = State.NEUTRAL;
                shipCount = 10;
            }
            else
            {
                state = State.OCCUPIED;
                shipCount = 200;
            }
        }

        public void changeTeams(Team team)
        {
            this.team = team;
            color = team.getColor();
        }

        public override void Update()
        {
            if (team != null)
                shipCount += (productionRate / 3600);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, ObjectSize, 0, 0);  
            spriteBatch.DrawString(text, ID.ToString(), Position-centerTextOffset, Color.White,Orientation,new Vector2(0,0),ObjectSize,0,0);
            spriteBatch.DrawString(text, shipCount.ToString(), Position, Color.White, Orientation, new Vector2(0, 0), ObjectSize, 0, 0);
        }

            
    }
}
