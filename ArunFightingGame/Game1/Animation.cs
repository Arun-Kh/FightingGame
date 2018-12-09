using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Animation : Sprite
    {

        private List<Frame> FrameList;


        TimeSpan FrameTimer = TimeSpan.Zero;
        TimeSpan FrameRate = TimeSpan.FromMilliseconds(60);


        public int currentFrame = 0;
        
        public int frameCount
        {
            get
            {
                return FrameList.Count;
            }
        }

        
        public bool FirstLoop
        {
            get
            {
                return counterLoop < 1; 
            }
        }
        public bool SecondLoop
        {
            get
            {
                return counterLoop < 2;
            }
        }
        

        int counterLoop = 0;        
        
        public override Rectangle? SourceRectangle
        {
            get { return new Rectangle(FrameList[currentFrame].Source.X, FrameList[currentFrame].Source.Y, FrameList[currentFrame].Source.Width, FrameList[currentFrame].Source.Height); }
        }
       
        public Animation(Texture2D image, Vector2 position, Color tint, Vector2 Origin, params Frame[] frames)
            : base(image, position, tint, Origin)
        {
            FrameList = new List<Frame>(frames);
            this.Origin = Origin;

        }

        public void ResetLoop()
        {
            counterLoop = 0;
        }

        public void Update(GameTime gametime)
        {
            FrameTimer += gametime.ElapsedGameTime;
            if (FrameTimer >= FrameRate)
            {
                FrameTimer = TimeSpan.Zero;
                currentFrame++;
                //Origin = new Vector2(FrameList[currentFrame].Origin.X, FrameList[currentFrame].Origin.Y);
                if (currentFrame >= FrameList.Count)
                {
                    currentFrame = 0;
                    counterLoop++;
                }
            }

        }
        public void FreezeFrame()
        {
            FrameTimer = TimeSpan.Zero;
        }
        public void FirstFreezeFrame()
        {
            if (counterLoop <= 1)
            {
                currentFrame = 0;
                FrameTimer = TimeSpan.Zero;
            }
        }
        public void LastFreezeFrame()
        {
            if(currentFrame == (FrameList.Count - 1))
            {
                FrameTimer = TimeSpan.Zero;
            }

        }

        
        public void ChangeFrames(List<Frame> frames)
        {
            FrameList = frames;
            
            //Origin = FrameList[currentFrame].Origin;
            //    Origin = new Vector2(FrameList[currentFrame].Origin.X, FrameList[currentFrame].Origin.Y);
        }        
        public void ChangeFrameRate(TimeSpan timeSpan)
        {
            FrameRate = timeSpan;
        }
        public void ChangeSourceRectanglePosition()
        {
            //SourceRectangle = new Rectangle(SourceRectangle.Value.X - SourceRectangle.Value.Width, SourceRectangle.Value.Y, SourceRectangle.Value.Width, SourceRectangle.Value.Height);
        }
        public void ChangeOrigin(Vector2 origin/*, int f*/)
        {
            Origin = origin;
            //FrameList[f].OriginChange(origin);
            //FrameList[f].Origin = new Vector2(origin.X, origin.Y);
            for (int i = 0; i < frameCount; i++)
            {
                FrameList[i].OriginChange(origin);
            }
            //make this change the orgin for each individual frame
        }
        public void AddFrame(Frame frame)
        {
            FrameList.Add(frame);
        }
    }
}
