using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Invasion
{
    public abstract class Entity
    {
        protected Texture2D image;
        protected Color color = Color.White;

        public Vector2 Position, Velocity;
        public float Orientation;
        public float LocalOrientation;
        public float Radius = 20;
        public bool IsExpired;
        public float ObjectSize = 1f;

        public Vector2 Size
        {
            get
            {
                return image == null ? Vector2.Zero : new Vector2(image.Width, image.Height);
            }
        }

        public abstract void Update();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, ObjectSize, 0, 0);
            
        }
    }


}
