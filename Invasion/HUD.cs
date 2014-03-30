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
                string s = "Team " + i + ": " + TeamManager.teamShipCount[i];
                spriteBatch.DrawString(text, s, new Vector2(300 * i + 25, 20), Color.White);
            }
            if (TeamManager.teamShipCount[1] + TeamManager.teamShipCount[0] > 0)
            {
                leftRatio = TeamManager.teamShipCount[0] / (float)(TeamManager.teamShipCount[1] + TeamManager.teamShipCount[0]);
                //rightRatio = TeamManager.teamShipCount[1] /(float)(TeamManager.teamShipCount[0] + TeamManager.teamShipCount[1]);
            }
            int boxMargin = 0;
            Rectangle leftrect = new Rectangle(0, 0, (int)(score.Width  * leftRatio+boxMargin), score.Height);
            Rectangle rightrect = new Rectangle((int)(score.Width * leftRatio + boxMargin), 0, score.Width, score.Height);
            spriteBatch.Draw(score, new Vector2(900 + 25, 20), leftrect, TeamManager.teams[0].getColor(), 0, scoreSize / 2f, 1f, 0, 0);
            spriteBatch.Draw(score, new Vector2(900 + 25 + (int)(score.Width * leftRatio), 20), rightrect, TeamManager.teams[1].getColor(), 0, scoreSize / 2f, 1f, 0, 0);
        }

    }
}
