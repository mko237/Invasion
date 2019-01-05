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
        private static Tuple<int,int> GameTime;

        public static void Update(Tuple<int,int> gameTime)
        {
            GameTime = gameTime;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            float[] ratios = new float[TeamManager.teams.Count];
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
                
                if ((int)TeamManager.teamShipCount[i] > 1000)
                {
                    tempships = (float)TeamManager.teamShipCount[i] / 1000f;
                    ships = tempships.ToString("0.00") + "K";
                }
                string s =  "Ships: " + ships;

                //create players string

                string p = "Players: " + Players.Teams[i].Count.ToString();
                if(TeamManager.teams.Count == 4)
                    p = "Plyrs: " + Players.Teams[i].Count.ToString();

                //create gametime string
                string time = GameTime.Item1.ToString() + " : " + GameTime.Item2.ToString();

                //create team ratio
                if (TeamManager.getTotalShipCount() > 0)
                    ratios[i] = TeamManager.teamShipCount[i] / TeamManager.getTotalShipCount();

                //create team percentage
                string t;
                if (TeamManager.teams[i].isDead)
                {
                    if (TeamManager.teams[i].gaveUp)
                        t = TeamManager.teams[i].colorText + " (QUIT)";
                    else
                        t = TeamManager.teams[i].colorText + " (DEAD)";
                }
                else
                {
                    if (TeamManager.teams[i].canSendAll)
                        t = TeamManager.teams[i].colorText + " (" + (100 * ratios[i]).ToString("0.0") + "%) *";
                    else
                        t = TeamManager.teams[i].colorText + " (" + (100 * ratios[i]).ToString("0.0") + "%)";
                }

               



                float size = 1 - 0.05f * TeamManager.teams.Count;

                int spacing = 0;
                int padding = 0;
                if (TeamManager.teams.Count == 2)
                {
                    spacing = 900;
                    padding = 50;
                }
                else if (TeamManager.teams.Count == 3)
                {
                    spacing = 550;
                    padding = 25;
                }
                else
                {
                    spacing = 400;
                    padding = 1;
                }

               

                //team name
                spriteBatch.DrawString(text, t, new Vector2(spacing * i + padding, GameRoot.Viewport.Height - 80), TeamManager.teams[i].getColor(), 0, new Vector2(0,0), size, 0, 0);
                //Ships
                spriteBatch.DrawString(text, s, new Vector2(spacing * i + padding, GameRoot.Viewport.Height - 40), TeamManager.teams[i].getColor(), 0, new Vector2(0, 0), size, 0, 0);
                //Rate
                spriteBatch.DrawString(text, r, new Vector2(spacing * i + padding + text.MeasureString(s).X + padding, GameRoot.Viewport.Height - 40), TeamManager.teams[i].getColor(), 0, new Vector2(0, 0), size, 0, 0);
                //Players
                spriteBatch.DrawString(text, p, new Vector2(spacing * i + padding + text.MeasureString(s).X + padding + text.MeasureString(r).X + padding, GameRoot.Viewport.Height - 40), TeamManager.teams[i].getColor(), 0, new Vector2(0, 0), size, 0, 0);
                //Gametime:
                spriteBatch.DrawString(text, time, new Vector2((GameRoot.ScreenSize.X / 2) + score.Width + 55, 10), Color.White);
            }

            int boxMargin = 0;

            int x = 0;
            for (int i = 0; i < TeamManager.teams.Count; i++)
            {
                spriteBatch.Draw(score, new Vector2((GameRoot.ScreenSize.X / 2) + x, 25), new Rectangle(x, 0, (int)(ratios[i] * score.Width), score.Height), TeamManager.teams[i].getColor(), 0, scoreSize / 2f, 1f, 0, 0);
                x += (int)(score.Width * ratios[i] + boxMargin);
            }
        }

    }
}
