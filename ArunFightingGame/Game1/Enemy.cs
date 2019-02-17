using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Enemy : Character
    {
        //Animation currentanimation;
        public Enemy(Texture2D image, Vector2 position, Color tint, Rectangle screen)
            : base(image, position, tint, screen)
        {
           // defaultPos = position;

        }

        

        bool flip = false;
        public new bool isAttackingWeak = false;
        public new bool isAttackingPowerful = false;
        TimeSpan attackTime = TimeSpan.FromMilliseconds(1500);
        public new float Speed { get; set; } = 2.4f;
        public int defaultHealth = 230; //default health used after respawing
        public float jumpspeed = 15f;


        TimeSpan enemyDeathTimer = TimeSpan.Zero;
        TimeSpan enemyDeathTime = TimeSpan.FromMilliseconds(150);

        TimeSpan enemyHitTimer = TimeSpan.Zero;
        TimeSpan reduceEnemyHealthTime = TimeSpan.FromMilliseconds(2);

        public bool characterDead = false;

        //TimeSpan reduceHealthTime = TimeSpan.FromMilliseconds(750);
        TimeSpan gametimer = TimeSpan.Zero;

        TimeSpan attackTimer = TimeSpan.Zero;

        public void attack()
        {
            if (!characterDead)
            {
                //Random rand = new Random();
                //if (rand.Next(1, 5) == 1)
                //{
                //    ChangeState(characterState.Kick1);
                //}
                //else if (rand.Next(1, 5) == 2)
                //{
                //    ChangeState(characterState.Kick2);
                //}
                //else if (rand.Next(1, 5) == 3)
                //{
                //    ChangeState(characterState.Kick3);
                //}
                //else if (rand.Next(1, 5) == 4)
                //{
                //    ChangeState(characterState.Punch1);
                //}
                //else if (rand.Next(1, 5) == 5)
                //{
                //    ChangeState(characterState.Punch2);
                //}

            }
        }
        public void Update(GameTime gameTime, Vector2 CharacterPostion, bool characterDead, int graphicsHeight)
        {
            if (health <= 0)
            {
                die(gameTime);
                //ChangeState(characterState.Death);
                //currentAnimation.LastFreezeFrame();
                ////stunBool = true;                    stun(gameTime);
                //dead = true;
                //enemyDeathTimer += gameTime.ElapsedGameTime;
                //if (enemyDeathTimer > enemyDeathTime)
                //{
                //    die();
                //}
              //  enemyDeathTimer = TimeSpan.Zero;
            }
            if (!dead)
            {
                //getHit = false;

                if (getHit == true)
                {
                    stun(gameTime);
                    enemyHitTimer += gameTime.ElapsedGameTime;
                    if (enemyHitTimer >= reduceEnemyHealthTime)
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
                        enemyHitTimer = TimeSpan.Zero;
                    }
                }
                
                if (health < 0)
                {
                    health = 0;
                }


                //if (health <= 0)
                //{
                //    ChangeState(characterState.Death);
                //    currentAnimation.LastFreezeFrame();
                //    //stunBool = true;                    stun(gameTime);

                //    enemyDeathTimer += gameTime.ElapsedGameTime;
                //    if (enemyDeathTimer > enemyDeathTime)
                //    {


                //        die();
                //    }
                //    enemyDeathTimer = TimeSpan.Zero;
                //}
                if (!stunBool && !characterDead)
                {
                    if (currentAnimation.X + 50 < CharacterPostion.X && !isCollidingRight)
                    {
                        ChangeState(characterState.Run);
                        if (currentState == characterState.Run)
                        {
                            if (!currentAnimation.FirstLoop)
                            {
                                ChangeState(characterState.Run);
                            }
                        }
                        if (flip)
                        {
                            currentAnimation.Effects = SpriteEffects.None;
                            isFlipped = false;
                            currentAnimation.Origin = Vector2.Zero;
                        }
                        Velocity = Speed;

                    }
                    if (currentAnimation.X - 50 > CharacterPostion.X && !isCollidingLeft)
                    {
                        ChangeState(characterState.Run);
                        if (currentState == characterState.Run)
                        {
                            if (!currentAnimation.FirstLoop)
                            {
                                ChangeState(characterState.Run);
                            }
                        }
                        currentAnimation.Effects = SpriteEffects.FlipHorizontally;
                        flip = true;
                        Velocity = -Speed;
                    }
                    if (currentAnimation.X > CharacterPostion.X + 100 || currentAnimation.X < CharacterPostion.X - 100)
                    {
                        jump(graphicsHeight, CharacterPostion);
                    }
                    //if (currentAnimation.X > CharacterPostion.X +100 || currentAnimation.X < CharacterPostion.X - 100)
                    //{
                    //    float initialJumpSpeed = 15f;
                    //    float gravity = .5f;
                    //    Vector2 initialPosition = currentPositon;
                    //    jumpspeed = initialJumpSpeed;
                    //    bool jumping = true;


                    //    if (jumping)
                    //    {
                    //        ChangeState(characterState.Jump);
                    //        isCollidingLeft = false;
                    //        isCollidingRight = false;
                    //        //jumping:

                    //        jumpspeed -= gravity;
                    //        currentAnimation.Y -= jumpspeed;
                    //        //if(currentAnimation.)
                    //        if (currentAnimation.FirstLoop && jumpspeed < 0)
                    //        {
                    //            currentAnimation.FirstFreezeFrame();
                    //        }
                    //        if (currentAnimation.Y + currentAnimation.SourceRectangle.Value.Height > graphicsHeight)
                    //        {
                    //            //hit the ground
                    //            jumping = false;
                    //            //set Y position to ground
                    //            currentAnimation.Y = 350;
                    //            ChangeState(characterState.Idle);

                    //        }

                    //    }
                    //}
                }

                currentAnimation.X += Velocity;
                Velocity -= (Velocity * Friction);


                if (!(currentAnimation.X + 85 < CharacterPostion.X || currentAnimation.X - 85 > CharacterPostion.X))
                {
                    attackTimer += gameTime.ElapsedGameTime;

                    if (attackTimer > (attackTime /*+ gameTime.ElapsedGameTime*/))
                    {
                        if (!currentAnimation.FirstLoop)
                        {
                            attack();
                            attackTimer = TimeSpan.Zero;
                        }
                    }
                }

                if (characterDead)
                {

                }

                hitbox.X = (int)currentAnimation.X;
                hitbox.Y = (int)currentAnimation.Y;
                hitbox.Width = (int)currentAnimation.SourceRectangle.Value.Width;
                hitbox.Height = (int)currentAnimation.SourceRectangle.Value.Height;

                if (currentState == characterState.Punch1 || currentState == characterState.Punch2)
                {
                    isAttackingPowerful = true;
                }
                if (currentState == characterState.Kick1 || currentState == characterState.Kick2 || currentState == characterState.Kick3)
                {
                    isAttackingWeak = true;
                }

                if (!currentAnimation.FirstLoop)
                {
                    ChangeState(characterState.Idle);
                }


                gametimer += gameTime.ElapsedGameTime;
                if (gametimer >= stunTimer)
                {


                    stunBool = false;
                    gametimer = TimeSpan.Zero;

                }
            }


          

            currentAnimation.Update(gameTime);

        }

        public void respawn()
        {
            //add a respawn animation effect
            ChangeState(characterState.Idle);
            currentPositon = new Vector2(100, 350); //make a default position 
            //currentPositon = defaultPos;
            //health = 200; //make a default health int
            health = defaultHealth;
            dead = false;
            currentAnimation.Tint = Color.Red; //make a default color 

        }

        public void jump(int graphicsHeight, Vector2 CharacterPostion)
        {
            if (currentAnimation.X > CharacterPostion.X + 100 || currentAnimation.X < CharacterPostion.X - 100)
            
            {
                float initialJumpSpeed = 15f;
                float gravity = .5f;
                Vector2 initialPosition = currentPositon;
                jumpspeed = initialJumpSpeed;
                bool jumping = true;


                if (jumping)
                {
                    ChangeState(characterState.Jump);
                    isCollidingLeft = false;
                    isCollidingRight = false;
                    //jumping:

                    jumpspeed -= gravity;
                    currentAnimation.Y -= jumpspeed;
                    //if(currentAnimation.)
                    if (currentAnimation.FirstLoop && jumpspeed < 0)
                    {
                        currentAnimation.FirstFreezeFrame();
                    }
                    if (currentAnimation.Y + currentAnimation.SourceRectangle.Value.Height > graphicsHeight)
                    {
                        //hit the ground
                        jumping = false;
                        //set Y position to ground
                        currentAnimation.Y = 350;
                        ChangeState(characterState.Idle);

                    }

                }
            }
        }


        //Make the enemy attack the character
        //Make an attack function
        //Make the enemy jump and try to dodge attacks
        //Make the enemy have a health and then die
        //Make it less bad







    }
}
