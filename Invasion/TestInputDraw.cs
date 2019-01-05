using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Invasion
{
    class TestInputDraw
    {
        //private SpriteFont Font1;
        private static Vector2 Position;
        public static ConcurrentStack<string> Q = new ConcurrentStack<string>();
        public static string QQ = null;
        private static Random rand = new Random();
        public static string Command = "Started";
        public static Color Color = Color.Red;

        public static void Update()
        {
            if (QQ != null)
            {
                string text = null;
                text = QQ.Replace("\r", string.Empty);
                text = QQ.Replace("\n", string.Empty);
                text = QQ.Replace("\0", string.Empty);
                //Command = text;
                InputParser.Command = text;
                InputParser.Update();
            }
        }
       

        
        public static void Draw(SpriteBatch spriteBatch)
        {
           
             
           
                //string text = Command;
                ////int i = GameRoot.Viewport.Width;
                ////int j = GameRoot.Viewport.Height;
                ////Position = new Vector2(500, 500);

                
                
                ////Q.TryPop(out text);
                //text = QQ.Replace("\r", string.Empty);
                //text = QQ.Replace("\n", string.Empty);
                //text = QQ.Replace("\0", string.Empty);
                spriteBatch.DrawString(Art.Font, Command, Position, Color);
           
        }
    }
}
