using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{

    public class Sprite
    {
        public Texture2D Image { get; set; }

        public Vector2 Position { get; set; }

        public float X
        {
            get
            {
                return Position.X;
            }
            set
            {
                Position = new Vector2(value, Position.Y);
            }
        }
        public float Y
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Position = new Vector2(Position.X, value);
            }
        }

        public Color Tint { get; set; }

        public float Rotation { get; set; }

        public Vector2 Origin { get; set; }

        public float Scale { get; set; }

        public SpriteEffects Effects { get; set; }

        public virtual Rectangle? SourceRectangle { get; set; }

        public Sprite(Texture2D Image, Vector2 Position, Color Tint, Vector2 Origin)
        {
            this.Image = Image;
            this.Position = Position;
            this.Tint = Tint;

            Rotation = 0;
            this.Origin = Origin;/*Vector2.Zero;*/
            Scale = 1;
            Effects = SpriteEffects.None;
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, SourceRectangle, Tint, Rotation, Origin, Scale, Effects, 0);
        }
        
        public void Draw(SpriteBatch spriteBatch, Texture2D pixel, Rectangle box)
        {
            Draw(spriteBatch);
            spriteBatch.Draw(pixel, box, box, Color.Red * 0.40f, Rotation, Origin, SpriteEffects.None, 0);
        }
    }
}
