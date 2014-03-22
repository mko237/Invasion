using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Invasion
{
    class InputDisplay
    {
        private static KeyboardState currentKeyboardState, oldKeyboardState;
        private static String textString = String.Empty;
        private static SpriteFont text = Art.Font;
        public static String Command = null;

        public InputDisplay()
        {
            String textString = String.Empty;
            SpriteFont text = Art.Font;
        }

        public static void UpdateInput()
        {
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            Keys[] pressedKeys;
            pressedKeys = currentKeyboardState.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if( oldKeyboardState.IsKeyUp(key) )
                {
                    if (key == Keys.Back) // overflows
                    {
                        if (textString.Length > 0)
                        textString = textString.Remove(textString.Length - 1, 1);
                    }                                               
                    else if (key == Keys.Space)
                        textString = textString.Insert(textString.Length, " ");
                    else if (key == Keys.Enter)
                    {
                        Command = textString;
                        textString = String.Empty;
                    }
                    else 
                        textString += key.ToString();
                }
            }
        }

       public static void Draw(SpriteBatch spriteBatch)
       {
           spriteBatch.DrawString(text, textString, GameRoot.ScreenSize - text.MeasureString(textString), Color.White);
       }

         
    }
}
