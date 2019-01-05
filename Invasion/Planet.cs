using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Invasion
{
   
    public class Planet : Entity
    {
        public int ID { get; private set; }
        SpriteFont text = Art.Font;
        Vector2 centerTextOffset;
        private Random rand = new Random();

        public enum State
        {
            OCCUPIED,
            NEUTRAL
        }

        private float productionAccel = (185)/((1-LevelSpawner.minSize)*LevelSpawner.imageSize);
        private float b = 200-(185/(1-LevelSpawner.minSize));
        public float productionRate;
        public float shipCount { get; set; }
        public float enemyShipCount { get; set; }
        public Team team { get; set; }
        public Team invadingTeam { get; set; }
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
            productionRate = productionAccel *2* Radius + b;
            ID = id;
            this.team = team;
            this.invadingTeam = team;

            image = Art.Planet;
            Vector2 textSize = text.MeasureString(ID.ToString());
            centerTextOffset = (textSize) * ObjectSize / 2;

            if (team == null)
            {
                state = State.NEUTRAL;
                shipCount = 162.5f*(ObjectSize-.56f)+10;
            }
            else
            {
                state = State.OCCUPIED;
                shipCount = 200;
            }
        }

        public void changeTeams()
        {
            if (team != null)
                TeamManager.removePlanet(team, this);
            if (invadingTeam != null)
            {
                this.team = invadingTeam;
                TeamManager.addPlanet(team, this);
            }
            color = team.getColor();
            Cue cue = GameRoot.soundBank.GetCue("Metal");
            cue.Play();

            //float hue1 = rand.NextFloat(0, 6);
            //float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
            //Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
            //Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);

            for (int i = 0; i < (int)(60*(Math.Pow(ObjectSize,2))); i++)
            {
                float speed = (18f * (1f - 1 / rand.NextFloat(1, 10)))*(float)(Math.Pow(ObjectSize,2));
                var pstate = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.Planet,
                    LengthMultiplier = 1
                };

                //Color colorr = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                GameRoot.ParticleManager.CreateParticle(Art.LineParticle, Position, color, 190f, 1.5f, pstate);
            }


        }
        public void removeTeam()
        {
            
            //if (invadingTeam != null)
            //{
            //    this.team = invadingTeam;
            //    TeamManager.addPlanet(team, this);
            //}
            team = null;
            state = State.NEUTRAL;
            color = Color.White;
            Cue cue = GameRoot.soundBank.GetCue("Metal");
            cue.Play();

            //float hue1 = rand.NextFloat(0, 6);
            //float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
            //Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
            //Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);

            for (int i = 0; i < (int)(60 * (Math.Pow(ObjectSize, 2))); i++)
            {
                float speed = (18f * (1f - 1 / rand.NextFloat(1, 10))) * (float)(Math.Pow(ObjectSize, 2));
                var pstate = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.Planet,
                    LengthMultiplier = 1
                };

                //Color colorr = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                GameRoot.ParticleManager.CreateParticle(Art.LineParticle, Position, color, 190f, 1.5f, pstate);
            }
        }

        public override void Update()
        {
            if (enemyShipCount < 0)
                enemyShipCount = 0;
           
            if (team != null)
            {
                shipCount += (productionRate / 3600);
                if(enemyShipCount > 0)
                {
                    enemyShipCount -= (productionRate / 3600);
                }
                
            }

            if (shipCount <= 0)
                changeTeams();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //new Color(color.R,color.G,color.B,1f)
            spriteBatch.Draw(Art.Shield, Position+new Vector2(-3.5f*ObjectSize,-2.9f*ObjectSize), null, color, Orientation, Size / 2f, ObjectSize+(ObjectSize*.049f), 0, 0);
            if (enemyShipCount > 0)
            {
                Rectangle top = new Rectangle(0, 0, LevelSpawner.imageSize, (int)(LevelSpawner.imageSize * (enemyShipCount / (enemyShipCount + shipCount))));
                Rectangle bottom = new Rectangle(0, (int)(LevelSpawner.imageSize * (enemyShipCount / (enemyShipCount + shipCount))), LevelSpawner.imageSize, (int)(LevelSpawner.imageSize*(shipCount / (enemyShipCount + shipCount))));
                spriteBatch.Draw(image, Position, top, invadingTeam.getColor(), Orientation, Size / 2f, ObjectSize, 0, 0);
                spriteBatch.Draw(image, new Vector2(Position.X, Position.Y + ObjectSize * (int)(LevelSpawner.imageSize * (enemyShipCount / (enemyShipCount + shipCount)))), bottom, color, Orientation, Size / 2f, ObjectSize, 0, 0);
            }
            else
                spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, ObjectSize, 0, 0);

            spriteBatch.DrawString(text, ID.ToString(), Position-centerTextOffset, Color.White,Orientation,new Vector2(0,0),ObjectSize+((1-ObjectSize)*.15f),0,0);
            spriteBatch.DrawString(text, shipCount.ToString("0"), Position, Color.White, Orientation, new Vector2(0, 0), ObjectSize, 0, 0);
            
        }

            
    }
}
