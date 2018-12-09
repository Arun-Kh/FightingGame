using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Content
{

    
    //font position color text
   public class Label
    {
        public string text;
        public Vector2 position;
        public Color tint;
        public SpriteFont font;
        

        public Label(string text, Vector2 position, Color tint, SpriteFont font)
        {
            this.text = text;
            this.position = position;
            this.tint = tint;
            this.font = font;
        }


        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, tint);
           
        }

        


    }
}
