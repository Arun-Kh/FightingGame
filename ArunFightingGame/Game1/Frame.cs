using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Frame
    {
        public Rectangle Source;
        public Vector2 Origin;
        public TimeSpan Length;

        public Frame(Rectangle SourceRect, Vector2 Origin, TimeSpan Length)
        {
            this.Origin = Origin;
            Source = SourceRect;
            this.Length = Length;
        }

        public void OriginChange(Vector2 origin)
        {
            Origin = origin;
        }


    }
}
