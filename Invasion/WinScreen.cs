using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
namespace Invasion
{
    public static class WinScreen
    {
        public static bool drawScreen = false;
        public static bool lastDrawScreen = false;

        public static Team winTeam = null;
        public static Vector2 teamPosition = new Vector2(-50,GameRoot.ScreenSize.Y/2-50);
        public static Vector2 victoryPosition = new Vector2(GameRoot.ScreenSize.X+350, GameRoot.ScreenSize.Y / 2 + 150);
        private static Vector2 teamVelocity = new Vector2(8, 0);
        private static Vector2 victoryVelocity = new Vector2(8, 0); 
      
        private static SpriteFont text = Art.Font2;
        //private static Vector2 origin = text.MeasureString(teamText) / 2;
        private static string teamText = "TEAM";
        private static string victoryText = "WINS!";
        public static bool playMusic = true;


        public static void Update()
        {

            if (drawScreen)
            {
                teamText = winTeam.colorText;
                float teamDistance = (GameRoot.ScreenSize.X/2 ) - teamPosition.X;
                float victoryDistance = victoryPosition.X - (GameRoot.ScreenSize.X / 2 );

                if ( teamDistance>0)//teamPosition.X<GameRoot.ScreenSize.X/2 + text.MeasureString(teamText).X)//-text.MeasureString(teamText).X-9)
                {

                    teamVelocity = new Vector2(teamDistance / 15,0);
                    teamPosition += teamVelocity;

                }
                //else if ( teamPosition.X>GameRoot.ScreenSize.X/2 + text.MeasureString(teamText).X)//-text.MeasureString(teamText).X+9)
                //{
                //    teamVelocity = new Vector2(teamDistance / 15, 0);
                //    teamPosition -= teamVelocity;
                //}

                if (teamDistance<=1)//(teamPosition.X>GameRoot.ScreenSize.X/2-text.MeasureString(teamText).X-9)&&(teamPosition.X<GameRoot.ScreenSize.X/2-text.MeasureString(teamText).X+9))
                {
                    //if (victoryPosition.X < GameRoot.ScreenSize.X / 2 - text.MeasureString(teamText).X - 9)
                    //{
                    //    victoryVelocity = new Vector2(victoryDistance / 15, 0);
                    //    victoryPosition += victoryVelocity;
                    //}
                    if (victoryDistance>0)//victoryPosition.X > GameRoot.ScreenSize.X / 2 - text.MeasureString(teamText).X + 9)
                    {
                        victoryVelocity = new Vector2(victoryDistance / 15, 0);
                        victoryPosition -= victoryVelocity;
                    }
                }

                
            }
            
        }
        private static void DrawOutlinedText(SpriteBatch sb, string texts, Color backColor, Color frontColor, float scale, float rotation, Vector2 position)
        {

            //If we want to draw the text from the origin we need to find that point, otherwise you can set all origins to Vector2.Zero.

            Vector2 origin = new Vector2(text.MeasureString(texts).X / 2, text.MeasureString(texts).Y / 2);
            //position = position + new Vector2(, );

            //These 4 draws are the background of the text and each of them have a certain displacement each way.

            sb.DrawString(text, texts, position + new Vector2(1 * scale, 1 * scale),//Here’s the displacement

            backColor,

            rotation,

            origin,

            scale,

            SpriteEffects.None, 1f);

            sb.DrawString(text, texts, position + new Vector2(-1 * scale, -1 * scale),

            backColor,

            rotation,

            origin,

            scale,

            SpriteEffects.None, 1f);

            sb.DrawString(text, texts, position + new Vector2(-1 * scale, 1 * scale),

            backColor,

            rotation,

            origin,

            scale,

            SpriteEffects.None, 1f);

            sb.DrawString(text, texts, position + new Vector2(1 * scale, -1 * scale),

            backColor,

            rotation,

            origin,

            scale,

            SpriteEffects.None, 1f);

            //This is the top layer which we draw in the middle so it covers all the other texts except that displacement.

            sb.DrawString(text, texts, position,

            frontColor,

            rotation,

            origin,

            scale,

            SpriteEffects.None, 0f);

        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            if( !lastDrawScreen && drawScreen)
                playMusic = true;
            if(drawScreen)
            {
                if(playMusic)
                {
                    //create sound
                    Cue cue = GameRoot.soundBank.GetCue("winNoteInvasion");
                    cue.Play();
                    playMusic = false;
                }
                
                
                Color c = winTeam.getColor();
                spriteBatch.Draw(Art.winBack, new Vector2(0,0), new Color(c.R,c.G,c.B,.15f));
                //http://erikskoglund.wordpress.com/2009/09/10/super-simple-text-outlining-in-xna/
               
            }
            lastDrawScreen = drawScreen;
            
        }
        public static void DrawText(SpriteBatch spriteBatch)
        {
            if (drawScreen)
            {
                Vector2 origin = text.MeasureString(teamText) / 2;
                //spriteBatch.DrawString(text, teamText, teamPosition, Color.DarkGray, 0, origin, 2f, 0, 0);
                //spriteBatch.DrawString(text, teamText, teamPosition, Color.Black, 0, origin, 2.5f, 0, 0);
                DrawOutlinedText(spriteBatch, teamText, Color.Black, Color.LightGray, 2.5f, 0, teamPosition);
                DrawOutlinedText(spriteBatch, victoryText, Color.Black, Color.LightGray, 2.5f, 0, victoryPosition);

            }
        }
    }
}
