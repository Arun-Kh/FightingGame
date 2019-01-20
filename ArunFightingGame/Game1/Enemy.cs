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
        public Enemy(Texture2D image, Vector2 position, Color tint)
            : base(image, position, tint)
        {


        }

        bool flip = false;
        public new bool isAttackingWeak = false;
        public new bool isAttackingPowerful = false;
        TimeSpan attackTime = TimeSpan.FromMilliseconds(1500);
        public new float Speed { get; set; } = 2.4f;

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
                Random rand = new Random();
                if (rand.Next(1, 5) == 1)
                {
                    ChangeState(characterState.Kick1);
                }
                else if (rand.Next(1, 5) == 2)
                {
                    ChangeState(characterState.Kick2);
                }
                else if (rand.Next(1, 5) == 3)
                {
                    ChangeState(characterState.Kick3);
                }
                else if (rand.Next(1, 5) == 4)
                {
                    ChangeState(characterState.Punch1);
                }
                else if (rand.Next(1, 5) == 5)
                {
                    ChangeState(characterState.Punch2);
                }

            }
        }
        public void Update(GameTime gameTime, Vector2 CharacterPostion, bool characterDead)
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
                    if (currentAnimation.X + 50 < CharacterPostion.X)
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
                    if (currentAnimation.X - 50 > CharacterPostion.X)
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
            currentPositon = new Vector2(100, 350);
            health = 200; //make a default health int
            dead = false;

        }


        //public void getsHit(GameTime gameTime)
        //{
        //    if (gametimer >= (reduceHealthTime + gameTime.ElapsedGameTime))
        //    {
        //        RealEnemyhealth--;
        //        gametimer = TimeSpan.Zero;
        //    }
        //}

        //if 3 seconds since last hit then take off health


        //Make the enemy attack the character
        //Make an attack function
        //Make the enemy jump and try to dodge attacks
        //Make the enemy have a health and then die
        //Make it less bad







    }
}
