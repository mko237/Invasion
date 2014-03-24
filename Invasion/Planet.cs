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
        public float enemyShipCount { get; set; }
        public Team team { get; private set; }
        public Team invadingTeam { get; set; }
        public State state { get; set; }


        private bool beingInvaded = false;

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
            productionRate = productionAccel * 2 * Radius - 50;
            ID = id;
            this.team = team;
            this.invadingTeam = team;

            image = Art.Planet;
            Vector2 textSize = text.MeasureString(ID.ToString());
            centerTextOffset = (textSize) * ObjectSize / 2;

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

        public void changeTeams()
        {
            this.team = invadingTeam;
            color = team.getColor();
        }

        public override void Update()
        {
            if (enemyShipCount > 0)
                beingInvaded = true;
            else
            {
                enemyShipCount = 0;
                beingInvaded = false;
            }

            if (team != null)
            {
                shipCount += (productionRate / 3600);
                enemyShipCount -= (productionRate / 3600);
            }

            if (shipCount < 0)
                changeTeams();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (beingInvaded)
            {
                Rectangle top = new Rectangle(0, 0, LevelSpawner.imageSize, (int)(LevelSpawner.imageSize * (enemyShipCount / (enemyShipCount + shipCount))));
                Rectangle bottom = new Rectangle(0, (int)(LevelSpawner.imageSize * (enemyShipCount / (enemyShipCount + shipCount))), LevelSpawner.imageSize, (int)(LevelSpawner.imageSize*(shipCount / (enemyShipCount + shipCount))));
                spriteBatch.Draw(image, Position, top, invadingTeam.getColor(), Orientation, Size / 2f, ObjectSize, 0, 0);
                spriteBatch.Draw(image, new Vector2(Position.X, Position.Y + ObjectSize * (int)(LevelSpawner.imageSize * (enemyShipCount / (enemyShipCount + shipCount)))), bottom, color, Orientation, Size / 2f, ObjectSize, 0, 0);
            }
            else
                spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, ObjectSize, 0, 0);

            spriteBatch.DrawString(text, ID.ToString(), Position-centerTextOffset, Color.White,Orientation,new Vector2(0,0),ObjectSize,0,0);
            spriteBatch.DrawString(text, shipCount.ToString("0"), Position, Color.White, Orientation, new Vector2(0, 0), ObjectSize, 0, 0);
        }

            
    }
}
