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
        public static Texture2D Shield { get; private set; }

        public static Texture2D Score { get; private set; }
        public static Texture2D Ship { get; private set; }
        public static Texture2D LineParticle { get; private set; }
        public static Texture2D SquareParticle { get; private set; }
        public static Texture2D Background { get; private set; }
        public static Texture2D SmallStars { get; private set; }
        public static Texture2D BigStars { get; private set; }

        public static SpriteFont Font { get; private set; }


        public static void Load(ContentManager content)
        {
            //Player = content.Load<Texture2D>(@"Art/Player"); dont include file exntension.
            Planet = content.Load<Texture2D>(@"Art/Planet");
            Ship = content.Load<Texture2D>(@"Art/Ship");
            Font = content.Load<SpriteFont>(@"Art/Font1");
            Background = content.Load<Texture2D>(@"Art/Background");
            Score = content.Load<Texture2D>(@"Art/score-meter");
            LineParticle = content.Load<Texture2D>(@"Art/Laser");
            SquareParticle = content.Load<Texture2D>(@"Art/SquareParticle");
            SmallStars = content.Load<Texture2D>(@"Art/Small_Stars");
            BigStars = content.Load<Texture2D>(@"Art/Big_Stars");
            Shield = content.Load<Texture2D>(@"Art/Shield");

        }
    }
}
