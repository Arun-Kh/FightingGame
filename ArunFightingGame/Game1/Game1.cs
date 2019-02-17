using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using Game1.Content;

namespace Game1
{
    //TODO:
    //Make a fighting game
    //Get a sprite sheet
    //Add more enemies
    //Get another sprite sheet
    //Make the attack animations not look bad when they are flipped horizontally (change the orgin)(try adding the width of the image to the X of the origin)
    //Change the origin for each individual frame
    //Add a health to the enemy and the player
    //Give the enemy a different sprite sheet
    //fix the freeze death frames
    //Fix the issue where the character dies immediatly 
    //Make sure that the enemy doesn't continue to move after it is set to death state
    //Make more enemies
    //Fix the issue with the list of enemies
    //change the font
    //make levels and stuff
    //change the font please
    //give the character a dumb name
    //fix the label that displays the level
    //fix the die (make the body go onto the ground when it dies and then float up off the screen or fade away)
    //fix the issue where the enemy's health doesn't go down
    //make the characters not go off the screen
    //change the font

        //fix the collision between character and enemy - and between several enemies - set 'isCollidingLeft' and 'isCollidingRight' to false
        //add several enemies

    //create boundries for the character and fix issues with thatS

    //newest version 10/28/18

    //Get a real background
    //Everything Else



    // Sprite
    // Animation: collection of Frames
    // Frame: Rectangle & Origin - doesn't inherit from sprite
    // Character:Inhert from Sprite; List of Animations, enmum State, 
    //     image of the character: the whole spritesheet
    //     list of animations changes the source rectangle
    //     Dictionary: State, Animation

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random rand = new Random();
        Texture2D sheet;
        Texture2D pixel;

        Label LevelLabel;

        Label CharacterHealthLabel;

        Label EnemyHealthLabel;

        Label PlayGameLabel;

        Label LoseLabel;

        SpriteFont font;

        KeyboardState lastKS;

        TimeSpan levelUpTimer = TimeSpan.Zero;

        TimeSpan levelUpTime = TimeSpan.FromMilliseconds(15000);

        Character Character1;

        string health = "10h00";

        int level = 1;

        bool playing = false;

        Enemy Enemy1;

        List<Enemy> enemies = new List<Enemy>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            sheet = Content.Load<Texture2D>("fighter");

            Character1 = new Character(sheet, new Vector2(350, 350), Color.White, GraphicsDevice.Viewport.Bounds);

            //Enemy1 = new Enemy(sheet, new Vector2(100, 350), Color.Red, GraphicsDevice.Viewport.Bounds);

            font = Content.Load<SpriteFont>("font");

            CharacterHealthLabel = new Label(health, new Vector2(10, 10), Color.Green, font);

            EnemyHealthLabel = new Label(health, new Vector2(100, 10), Color.Red, font);

            PlayGameLabel = new Label(("press 'space' to start"), new Vector2(300, 100), Color.Red, font);

            LevelLabel = new Label(("Level 0"), new Vector2(590, 10), Color.Wheat, font);

            LoseLabel = new Label("You Lose!", new Vector2(350, 200), Color.LightYellow, font);

            enemies.Add(new Enemy(sheet, new Vector2(90 + rand.Next(10, 300), 350), Color.Red, GraphicsDevice.Viewport.Bounds));

            //for (int i = 0; i < 10; i++)
            //{
            //    enemies.Add(new Enemy(sheet, new Vector2(90 + rand.Next(10, 300), 350), Color.Red, GraphicsDevice.Viewport.Bounds));
            //}



            //add every animation to characther here

            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {

            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Space))
            { playing = true; }
            if (playing)
            {

                Character1.Update(gameTime, ks, lastKS, GraphicsDevice, false);

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Update(gameTime, Character1.currentPositon, Character1.dead, GraphicsDevice.Viewport.Height);
                    enemies[i].jump(GraphicsDevice.Viewport.Height, Character1.currentPositon);

                }

                //   Enemy1.Update(gameTime, Character1.currentPositon, Character1.dead);


            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!(Character1.hitbox.Intersects(enemies[i].hitbox)))
                {
                    Character1.isCollidingLeft = false;
                    Character1.isCollidingRight = false;
              //      Enemy1.isCollidingLeft = false;
                //    Enemy1.isCollidingRight = false;
                    
                    
                        enemies[i].isCollidingLeft = false;
                        enemies[i].isCollidingRight = false;
                    
                }
            }
            //for (int i = 0; i < enemies.Count; i++)
            //{
            //    for (int f = 0; i < enemies.Count; f++)
            //    {
            //        if (i == f)
            //        {
            //           f++;
            //        }
            //        else if (!(i == f))
            //        {
            //            if (enemies[i].hitbox.Intersects(enemies[f].hitbox))
            //            {
            //                if (enemies[i].isFlipped)
            //                {
            //                    enemies[i].isCollidingLeft = true;
            //                }
            //                else
            //                {
            //                    enemies[i].isCollidingRight = true;
            //                }
            //            }
            //            else if (!(enemies[i].hitbox.Intersects(enemies[f].hitbox)))
            //            {
            //                enemies[i].isCollidingLeft = false;
            //                enemies[i].isCollidingRight = false;
            //            }
            //        }
            //    }
            //}
            for (int i = 0; i < enemies.Count; i++)
            {
                if(Character1.hitbox.Right >= enemies[i].hitbox.Left-15 && Character1.hitbox.Right <= enemies[i].hitbox.Left + 15)
                {
                    Character1.isCollidingRight = true;
                    enemies[i].isCollidingLeft = true;
                    Character1.isCollidingLeft = false;
                    enemies[i].isCollidingRight = false;
                }
                if(Character1.hitbox.Left >= enemies[i].hitbox.Right-15 && Character1.hitbox.Left <= enemies[i].hitbox.Right + 15)
                {
                    Character1.isCollidingLeft = true;
                    enemies[i].isCollidingRight = true;
                    Character1.isCollidingRight = false;
                    enemies[i].isCollidingLeft = false;
                }

                if (Character1.hitbox.Intersects(enemies[i].hitbox))
                {
                    //add health info here
                    ////add collision here
                    
                    //if (Character1.isFlipped)
                    //{
                    //    Character1.isCollidingLeft = true;
                    //}
                    //else
                    //{
                    //    Character1.isCollidingLeft = false;
                    //}
                    //if (!Character1.isFlipped)
                    //{
                    //    Character1.isCollidingRight = true;
                    //}
                    //else
                    //{
                    //    Character1.isCollidingRight = false;
                    //}
                    //if (enemies[i].isFlipped)
                    //{
                    //    enemies[i].isCollidingLeft = true;
                    //}
                    //else if (!enemies[i].isFlipped) // facing right
                    //{
                    //    enemies[i].isCollidingRight = true;
                    //}
                    //else
                    //{
                    //    enemies[i].isCollidingRight = false;
                    //}


                    if (Character1.currentState == Character.characterState.Kick1 || Character1.currentState == Character.characterState.Kick2 || Character1.currentState == Character.characterState.Kick3)
                    {
                        enemies[i].getsHit(gameTime, true);

                    }
                    if ((Character1.currentState == Character.characterState.Punch1) || (Character1.currentState == Character.characterState.Punch2))
                    {
                        enemies[i].getsHit(gameTime, false);

                    }

                    if (enemies[i].currentState == Character.characterState.Kick1 || enemies[i].currentState == Character.characterState.Kick2 || enemies[i].currentState == Character.characterState.Kick3)
                    {
                        Character1.getsHit(gameTime, true);

                    }
                    if (enemies[i].currentState == Character.characterState.Punch1 || enemies[i].currentState == Character.characterState.Punch2)
                    {
                        Character1.getsHit(gameTime, false);
                    }
                }
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].currentState == Character.characterState.Death)
                {
                    enemies[i].dead = true;
                }

                if (enemies[i].dead)
                {
                    levelUpTimer += gameTime.ElapsedGameTime;

                    if (levelUpTimer >= levelUpTime)
                    {

                        level++;
                        //actually make it get harder with each level
                        enemies.RemoveAt(i);
                        //Enemy1.health = 200;
                        //         enemies.Add(new Enemy(sheet, new Vector2(90, 350), Color.Red));

                        enemies.Add(new Enemy(sheet, new Vector2(90 + rand.Next(10, 300), 350), Color.Red, GraphicsDevice.Viewport.Bounds));

                        //  Enemy1.dead = false;
                        levelUpTimer = TimeSpan.Zero;
                    }

                }
                if (Character1.health <= 0)
                {
                    enemies[i].characterDead = true;
                }
                
            }
            CharacterHealthLabel.text = Character1.health.ToString();

            EnemyHealthLabel.text = enemies[0].health.ToString();

            LevelLabel.text = "LEVEL " + level.ToString();

            lastKS = ks;
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (playing)
            {
                Character1.Draw(spriteBatch, pixel);

               // Enemy1.Draw(spriteBatch/*, pixel*/);
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Draw(spriteBatch, pixel);
                    spriteBatch.DrawString(font, enemies[i].jumpspeed.ToString(), new Vector2(0, 100), Color.Fuchsia);

                }

                CharacterHealthLabel.draw(spriteBatch);
                EnemyHealthLabel.draw(spriteBatch);
                LevelLabel.draw(spriteBatch);
            }
            if (!playing)
            {
                PlayGameLabel.draw(spriteBatch);
            }

            if(Character1.currentState == Character.characterState.Death)
            {
                LoseLabel.draw(spriteBatch);
            }

            //spriteBatch.DrawString(font,"Intersect:" + (Character1.hitbox.Intersects(Enemy1.hitbox)).ToString(), Vector2.Zero, Color.Yellow);

            //spriteBatch.DrawString(font, "Enemy GetHit: " + Enemy1.getHit.ToString(), new Vector2(150, 0), Color.Teal); 

            //spriteBatch.DrawString(font, "Character GetHit: " + Character1.getHit.ToString(), new Vector2(0, 50), Color.LimeGreen);

            //spriteBatch.DrawString(font,"Enemy: " + Enemy1.currentState.ToString(), new Vector2(0, 100), Color.HotPink);

            //spriteBatch.DrawString(font, "Character: " + Character1.currentState.ToString(), new Vector2(0, 150), Color.PowderBlue);

            //spriteBatch.DrawString(font, Enemy1.hitbox.ToString(), new Vector2(0, 200), Color.CadetBlue);


            //   spriteBatch.DrawString(font, levelUpTimer.ToString(), new Vector2(420, 0), Color.DarkOrange);

            //spriteBatch.DrawString(font, "Enemy dead: " + Enemy1.dead.ToString(), new Vector2(500, 0), Color.DarkSalmon);
            //    idle.Draw(spriteBatch);            
            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
