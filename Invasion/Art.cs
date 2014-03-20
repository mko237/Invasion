using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Invasion
{
   
    class Art
    {
        //image fields :
        // example:
        //public static Texture2D Player { get; private set; }
        public static Texture2D Planet { get; private set; }
        public static Texture2D Ship { get; private set; }
        public static Texture2D Background { get; private set; }
        public static SpriteFont Font { get; private set; }

        public static void Load(ContentManager content)
        {
            //Player = content.Load<Texture2D>(@"Art/Player"); dont include file exntension.
            Planet = content.Load<Texture2D>(@"Art/Planet");
            Ship = content.Load<Texture2D>(@"Art/Ship");
            Font = content.Load<SpriteFont>(@"Art/Font1");
            Background = content.Load<Texture2D>(@"Art/Milky Way Space View HD Wide Wallpaper for Widescreen");

        }
    }
}
