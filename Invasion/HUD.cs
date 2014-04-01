using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invasion
{
    public static class HUD
    {
        
        private static SpriteFont text = Art.Font;
        private static Texture2D score = Art.Score;
        private static Vector2 scoreSize = new Vector2(score.Width, score.Height);
        private static float leftRatio = .5f;
        //private static float rightRatio = .5f;

       
     

       

        public static void Update()
        {
            //getShipCount();

        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < TeamManager.teamShipCount.Count(); i++)
            {
                //calculate and create Rate string
                string production = ((int)TeamManager.teamProductionRate[i]).ToString();
                float tempprod;
                if((int)TeamManager.teamProductionRate[i]>1000)
                {
                    tempprod = TeamManager.teamProductionRate[i]/1000f;
                    production = tempprod.ToString("0.00")+"K";
                }
                string r = "Rate: " + production;
                
                //calculate and create ships string
                string ships = ((int)TeamManager.teamShipCount[i]).ToString();
                float tempships;
                if ((int)TeamManager.teamProductionRate[i] > 1000)
                {
                    tempships = TeamManager.teamShipCount[i] / 1000f;
                    ships = tempships.ToString("0.00") + "K";
                }
                string s =  "Ships: " + ships;

                //create players string
                string players;
                if(i == 0)
                {
                    players = Players.Team1.Count.ToString();
                }
                else
                {
                    players = Players.Team2.Count.ToString();
                }
                string p = "Players: " + players;

                
                
                //team name
                spriteBatch.DrawString(text, TeamManager.teams[i].colorText, new Vector2(900 * i + 25, GameRoot.Viewport.Height - 90), TeamManager.teams[i].getColor());
                //Ships
                spriteBatch.DrawString(text, s, new Vector2(900 * i + 25, GameRoot.Viewport.Height - 50), TeamManager.teams[i].getColor());
                //Rate
                spriteBatch.DrawString(text, r, new Vector2(900 * i + 250, GameRoot.Viewport.Height - 50), TeamManager.teams[i].getColor());
                //Players
                spriteBatch.DrawString(text, p, new Vector2(900 * i + 500, GameRoot.Viewport.Height - 50), TeamManager.teams[i].getColor());



            }
            if (TeamManager.teamShipCount[1] + TeamManager.teamShipCount[0] > 0)
            {
                leftRatio = TeamManager.teamShipCount[0] / (float)(TeamManager.teamShipCount[1] + TeamManager.teamShipCount[0]);
                //rightRatio = TeamManager.teamShipCount[1] /(float)(TeamManager.teamShipCount[0] + TeamManager.teamShipCount[1]);
            }
            int boxMargin = 0;
            Rectangle leftrect = new Rectangle(0, 0, (int)(score.Width  * leftRatio+boxMargin), score.Height);
            Rectangle rightrect = new Rectangle((int)(score.Width * leftRatio + boxMargin), 0, score.Width, score.Height);
            spriteBatch.Draw(score, new Vector2((GameRoot.ScreenSize.X/2), 40), leftrect, TeamManager.teams[0].getColor(), 0, scoreSize / 2f, 1f, 0, 0);
            spriteBatch.Draw(score, new Vector2(((GameRoot.ScreenSize.X/2)) + (int)(score.Width * leftRatio), 40), rightrect, TeamManager.teams[1].getColor(), 0, scoreSize / 2f, 1f, 0, 0);
        }

    }
}
