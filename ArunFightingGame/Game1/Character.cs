﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Game1
{
    public class Character
    {
        //public struct Frame
        //{
        //    public Rectangle Source;
        //    public Vector2 Origin;
        //    public TimeSpan Length;

        //    public Frame(Rectangle SourceRect, Vector2 Origin, TimeSpan Length)
        //    {
        //        this.Origin = Origin;
        //        Source = SourceRect;
        //        this.Length = Length;
        //    }
        //}

        public Rectangle hitbox = new Rectangle(0, 0, 0, 0);
        protected Animation currentAnimation;

        public characterState currentState = characterState.None;

        public Vector2 currentPositon;

        public bool isAttackingPowerful = false;


        public bool isAttackingWeak = false;
        public int forcePowerful = 20;
        public int force = 10;

        public TimeSpan stunTimer = TimeSpan.FromMilliseconds(0/*1250*/);
        TimeSpan stunElapsedTimer = TimeSpan.Zero;
        //make the attack cooldown timer longer than the stun timer
        public float Velocity { get; set; }
        public float Speed { get; set; } = 3;
        public const float Friction = 0.25f;

        public bool dead = false;

        public bool stunBool = false;

        Dictionary<characterState, List<Frame>> animations;

        Vector2 initialPosition = Vector2.Zero;
        float gravity = .5f;
        float jumpspeed = 0;
        float initialjumpspeed = 15f;
        bool jumping = false;
        public int health = 200;

        public bool isFlipped = false;
        TimeSpan reduceHealthTime = TimeSpan.FromMilliseconds(200);
        TimeSpan hitTimer = TimeSpan.Zero;


        TimeSpan deathTime = TimeSpan.FromMilliseconds(3000);
        TimeSpan deathTimer = TimeSpan.Zero;

        public bool getHit = false;
        public int isPowerfulEnemy = -1;

        public enum characterState
        {
            None,
            Idle,
            Run,
            Jump,
            Punch1,
            Punch2,
            Kick1,
            Kick2,
            Kick3,
            Death,
            FreezeDeath
            //Add more here
        }

        // ChangeState
        // That should change state, and set currentFrame to 0
        // But only do that, if the state is actually changing



        public Character(Texture2D Image, Vector2 Position, Color Tint)

        {


            animations = new Dictionary<characterState, List<Frame>>();

            List<Frame> idleFrames = new List<Frame>();
            List<Frame> jumpFrames = new List<Frame>();
            List<Frame> runFrames = new List<Frame>();
            List<Frame> punch1Frames = new List<Frame>();
            List<Frame> punch2Frames = new List<Frame>();
            List<Frame> Kick1Frames = new List<Frame>();
            List<Frame> Kick2Frames = new List<Frame>();
            List<Frame> Kick3Frames = new List<Frame>();
            List<Frame> deathFrames = new List<Frame>();
            List<Frame> FreezeDeathFrames = new List<Frame>();
            string[] lines = File.ReadAllLines("Data.txt");
            foreach (var line in lines)
            {
                string[] part = line.Split(';');
                Frame frame = new Frame((new Rectangle(int.Parse(part[2]), int.Parse(part[3]), int.Parse(part[4]), int.Parse(part[5]))), new Vector2(0, 0)/*(int.Parse(part[8]),int.Parse(part[9]))*/, TimeSpan.FromMilliseconds(60));

                if (part[0].Contains("Idle"))
                {
                    idleFrames.Add(frame);
                }

                if (part[0].Contains("Jumping"))
                {
                    jumpFrames.Add(frame);
                    frame.Length = TimeSpan.FromMilliseconds(60);
                    //frame.Origin = new Vector2(int.Parse(part[6]), int.Parse(part[7]));

                }
                if (part[0].Contains("Walking"))
                {
                    runFrames.Add(frame);
                }
                if (part[0].Contains("Punch1"))
                {
                    punch1Frames.Add(frame);
                    frame.Length = TimeSpan.FromMilliseconds(85);
                    //  frame.Origin = new Vector2(int.Parse(part[7]), int.Parse(part[8]));
                    //frame.Origin = new Vector2(-100, -120);
                    //frame.Origin = new Vector2((int)(.5*(frame.Source.Width)), (int)(.5*(frame.Source.Height)));
                    frame.Origin = new Vector2(15, -5);

                    //punch1Frames[punch1Frames.Count - 1].Origin = new Vector2(10, 10);


                }
                if (part[0].Contains("Punch2"))
                {
                    punch2Frames.Add(frame);

                    frame.Length = TimeSpan.FromMilliseconds(80);

                }
                if (part[0].Contains("Kick1"))
                {
                    Kick1Frames.Add(frame);
                    frame.Length = TimeSpan.FromMilliseconds(75);
                }
                if (part[0].Contains("Kick2"))
                {
                    Kick2Frames.Add(frame);
                    frame.Length = TimeSpan.FromMilliseconds(75);

                    frame.Origin = new Vector2(20, 0);
                }
                if (part[0].Contains("Kick3"))
                {
                    Kick3Frames.Add(frame);
                    frame.Length = TimeSpan.FromMilliseconds(75);
                }
                if (part[0].Contains("Death") || part[0].Contains("FreezeDeath"))
                {
                    deathFrames.Add(frame);
                    frame.Length = TimeSpan.FromMilliseconds(100);

                    //if (part[1].Contains("frame52"))
                    //{
                    //    frame.Length = TimeSpan.FromDays(12);
                    //}


                }
                if (part[0].Contains("FreezeDeath") /*&& part[1].Contains("frame54")*/)
                {
                    FreezeDeathFrames.Add(frame);
                    frame.Length = TimeSpan.FromDays(12);
                }



                //adding frame to the correct animation

            }

            AddAnimation(characterState.Idle, idleFrames);

            AddAnimation(characterState.Jump, jumpFrames);
            AddAnimation(characterState.Run, runFrames);
            AddAnimation(characterState.Punch1, punch1Frames);
            AddAnimation(characterState.Punch2, punch2Frames);
            AddAnimation(characterState.Kick1, Kick1Frames);
            AddAnimation(characterState.Kick2, Kick2Frames);
            AddAnimation(characterState.Kick3, Kick3Frames);
            AddAnimation(characterState.Death, deathFrames);
            AddAnimation(characterState.FreezeDeath, FreezeDeathFrames);

            currentAnimation = new Animation(Image, Position, Tint, new Vector2(0, 0));

            ChangeState(characterState.Idle);


        }

        public void AddAnimation(characterState state, List<Frame> frameList)
        {
            animations.Add(state, frameList/*List<Frame> frames*/);
        }

        public void die()
        {
            currentAnimation.Y--;
            currentAnimation.Tint = new Color(currentAnimation.Tint.R - 1, currentAnimation.Tint.G - 1, currentAnimation.Tint.B - 1);

        }
        public void Update(GameTime gameTime, KeyboardState ks, KeyboardState lastKs, GraphicsDevice graphics, bool flip)
        {
            getHit = false;
            if (getHit == true)
            {
                stun(gameTime);
                hitTimer += gameTime.ElapsedGameTime;
                if (hitTimer >= reduceHealthTime)
                {
                    if (isPowerfulEnemy == 1)
                    {
                        health -= 50;
                        moveback(gameTime, true);
                    }
                    if (isPowerfulEnemy == 0)
                    {
                        health -= 20;
                        moveback(gameTime, false);
                    }
                    getHit = false;
                    isPowerfulEnemy = -1;
                    hitTimer = TimeSpan.Zero;
                }
            }

            if (health < 0)
            {
                health = 0;
            }
            Speed = 3;
            if (stunBool)
            {
                Speed = 0;
            }
            //DYING:
            if (health <= 0)
            {
                ChangeState(characterState.Death);

                currentAnimation.LastFreezeFrame();



                //dead = true;

                //trying to make the body go down to the ground when it dies and then float up off the screen
                deathTimer += gameTime.ElapsedGameTime;
                if (deathTimer > deathTime)
                {

                    die();

                }


            }

            if (flip)
            {
                currentAnimation.Effects = SpriteEffects.FlipHorizontally;
            }
            if (currentAnimation.Effects == SpriteEffects.FlipHorizontally)
            {
                isFlipped = true;
            }
            if (!stunBool)
            {
                //JUMPING:
                if (ks.IsKeyDown(Keys.W) && lastKs.IsKeyUp(Keys.W) && !jumping)
                {

                    initialPosition = currentAnimation.Position;
                    jumpspeed = initialjumpspeed;
                    // currentAnimation.Y -= jumpspeed;

                    jumping = true;

                }

                if (jumping)
                {
                    ChangeState(characterState.Jump);

                    //jumping:

                    jumpspeed -= gravity;
                    currentAnimation.Y -= jumpspeed;
                    //if(currentAnimation.)
                    if (currentAnimation.FirstLoop && jumpspeed < 0)
                    {
                        currentAnimation.FirstFreezeFrame();
                    }
                }
                if (currentAnimation.Y + currentAnimation.SourceRectangle.Value.Height > graphics.Viewport.Height)
                {
                    //hit the ground
                    jumping = false;
                    //set Y position to ground
                    currentAnimation.Y = 350;
                    ChangeState(characterState.Idle);

                }
            }

            //WALKIKNG:
            if (!stunBool)
            {
                if (ks.IsKeyDown(Keys.A))
                {
                    if (!jumping && !stunBool)
                    {
                        ChangeState(characterState.Run);
                    }
                    Velocity = -Speed;
                    currentAnimation.Effects = SpriteEffects.FlipHorizontally;
                    isFlipped = true;
                }

                if (ks.IsKeyDown(Keys.D) /*|| ks.IsKeyDown(Keys.A)*/)
                {
                    if (!jumping && !stunBool)
                    {
                        ChangeState(characterState.Run);
                    }
                    Velocity = Speed;
                    if (isFlipped)
                    {
                        currentAnimation.Effects = SpriteEffects.None;
                        isFlipped = false;
                        currentAnimation.Origin = Vector2.Zero;
                    }
                }
            }
            if (lastKs.IsKeyDown(Keys.D) && ks.IsKeyUp(Keys.D))
            {
                ChangeState(characterState.Idle);
            }
            if (lastKs.IsKeyDown(Keys.A) && ks.IsKeyUp(Keys.A))
            {
                ChangeState(characterState.Idle);
            }

            //PUNCHING:
            if (ks.IsKeyDown(Keys.E) && lastKs.IsKeyUp(Keys.E))
            {
                ChangeState(characterState.Punch1);
            }
            if (ks.IsKeyDown(Keys.F) && lastKs.IsKeyUp(Keys.F))
            {
                ChangeState(characterState.Punch2);
            }
            //KICKING:

            if (ks.IsKeyDown(Keys.R) && lastKs.IsKeyUp(Keys.R))
            {
                ChangeState(characterState.Kick1);

            }
            if (ks.IsKeyDown(Keys.C) && lastKs.IsKeyUp(Keys.C))
            {
                ChangeState(characterState.Kick2);
            }
            if (ks.IsKeyDown(Keys.G) && lastKs.IsKeyUp(Keys.G))
            {
                ChangeState(characterState.Kick3);
            }

            if (!currentAnimation.FirstLoop)
            {
                ChangeState(characterState.Idle);
            }


            //changing the origin if the image is flipped:
            if (isFlipped)
            {
                //currentAnimation.Origin = new Vector2(currentAnimation.Origin.X + currentAnimation.SourceRectangle.Value.Width, currentAnimation.Origin.Y);
                //currentAnimation.ChangeSourceRectanglePosition();

                //currentAnimation.Position = new Vector2(currentAnimation.Position.X + currentAnimation.SourceRectangle.Value.Width, currentAnimation.Y);
            }
            if (!isFlipped)
            {
                //currentAnimation.Origin = Vector2.Zero;
            }

            currentAnimation.X += Velocity;
            Velocity -= (Velocity * Friction);
            currentPositon = new Vector2(currentAnimation.X, currentAnimation.Y);

            hitbox.X = (int)currentAnimation.X;
            hitbox.Y = (int)currentAnimation.Y;
            hitbox.Width = (int)currentAnimation.SourceRectangle.Value.Width;
            hitbox.Height = (int)currentAnimation.SourceRectangle.Value.Height;

            //if (currentState == characterState.Punch1 || currentState == characterState.Punch2)
            //{
            //    isAttackingPowerful = true;
            //    isAttackingWeak = false;
            //}
            //if (currentState == characterState.Kick1 || currentState == characterState.Kick2 || currentState == characterState.Kick3)
            //{
            //    isAttackingWeak = true;
            //    isAttackingPowerful = false;
            //}
            //if (!(currentState == characterState.Punch1) || !(currentState == characterState.Punch2) || !(currentState == characterState.Kick1) || !(currentState == characterState.Kick2) || !(currentState == characterState.Kick3))
            //{
            //    isAttackingPowerful = false;
            //    isAttackingWeak = false;
            //}


            stunElapsedTimer += gameTime.ElapsedGameTime;
            if (stunElapsedTimer >= stunTimer)
            {
                stunBool = false;
                stunElapsedTimer = TimeSpan.Zero;
            }

            if (currentState == characterState.Death)
            {
                //  dead = true;
            }

            currentAnimation.Update(gameTime);
        }


        public void getsHit(GameTime gameTime, bool powerful)
        {
            if (!getHit)
            {
                getHit = true;
                isPowerfulEnemy = powerful ? 1 : 0;
            }
        }


        public void moveback(GameTime gameTime, bool powerful)
        {

            stun(gameTime);
            if (currentAnimation.Effects == SpriteEffects.FlipHorizontally)
            {

                if (powerful)
                {
                    Velocity += 30;
                }
                else if (!powerful)
                {
                    Velocity += 20;
                }
            }
            else if (currentAnimation.Effects == SpriteEffects.None)
            {
                if (powerful)
                {
                    Velocity = -30;
                }
                else if (!powerful)
                {
                    Velocity -= 20;
                }


            }

        }
        public void stun(GameTime gameTime)
        {
            ChangeState(characterState.Idle);
            stunBool = true;

        }


        public void ChangeState(characterState state)
        {
            if (!stunBool && !dead)
            {
                if (state != currentState)
                {
                    currentState = state;

                    currentAnimation.ChangeFrames(animations[state]);

                    for (int i = 0; i < currentAnimation.frameCount; i++)
                    {
                        currentAnimation.ChangeOrigin(animations[state][i].Origin/*, i*/);
                        //currentAnimation.ChangeFrameRate(animations[state][i].Length);
                    }

                    //currentAnimation.ChangeOrigin(animations[state][0].Origin);

                    currentAnimation.ChangeFrameRate(animations[state][0].Length);

                    currentAnimation.currentFrame = 0;

                    currentAnimation.ResetLoop();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            currentAnimation.Draw(spriteBatch, pixel, new Rectangle(
                (int)currentAnimation.Position.X,
                (int)currentAnimation.Position.Y,
                currentAnimation.SourceRectangle.Value.Width,
                currentAnimation.SourceRectangle.Value.Height));

        }
    }
}
