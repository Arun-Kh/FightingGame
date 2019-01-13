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
    //fix the level system

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

        TimeSpan levelUpTime = TimeSpan.FromMilliseconds(4000);

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

            Character1 = new Character(sheet, new Vector2(350, 350), Color.White);

            Enemy1 = new Enemy(sheet, new Vector2(100, 350), Color.Red);

            font = Content.Load<SpriteFont>("font");

            CharacterHealthLabel = new Label(health, new Vector2(10, 10), Color.Green, font);

            EnemyHealthLabel = new Label(health, new Vector2(100, 10), Color.Red, font);

            PlayGameLabel = new Label(("press 'space' to start"), new Vector2(300, 100), Color.Red, font);

            LevelLabel = new Label(("Level 0"), new Vector2(590, 10), Color.Wheat, font);

            LoseLabel = new Label("You Lose!", new Vector2(350, 200), Color.LightYellow, font);

            //       enemies.Add(new Enemy(sheet, new Vector2(90, 350), Color.Red));




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
                    enemies[i].Update(gameTime, Character1.currentPositon, Character1.dead);
                }

                Enemy1.Update(gameTime, Character1.currentPositon, Character1.dead);
                

            }

            if (Character1.hitbox.Intersects(Enemy1.hitbox))
            {
                //add health info here

                if (Character1.currentState == Character.characterState.Kick1 || Character1.currentState == Character.characterState.Kick2 || Character1.currentState == Character.characterState.Kick3)
                {
                    Enemy1.getsHit(gameTime, true);
                    //Enemy1.health -= 100;
                    ////subtract a lot of health from enemy
                    //Enemy1.RealEnemyhealth -= 10;
                    //Enemy1.stun(gameTime);
                    //Enemy1.moveback(true);
                    //Enemy1.getHit(true);
                }
                if ((Character1.currentState == Character.characterState.Punch1) || (Character1.currentState == Character.characterState.Punch2))
                {
                    Enemy1.getsHit(gameTime, false);
                    //Enemy1.health -= 100;
                    //Enemy1.stun(gameTime);
                    //Enemy1.moveback(false);
                    // Enemy1.getHit(false);
                    //subtract a little of health from enemy
                }

                if (Enemy1.currentState == Character.characterState.Kick1 || Enemy1.currentState == Character.characterState.Kick2 || Enemy1.currentState == Character.characterState.Kick3)
                {
                    Character1.getsHit(gameTime, true);
                    //Character1.health -= 100;
                    //Character1.stun(gameTime);
                    //Character1.moveback(true);
                    //Character1.health -= 10;
                    //subtract a lot of health from character
                    //Character1.getHit(true);
                }
                if (Enemy1.currentState == Character.characterState.Punch1 || Enemy1.currentState == Character.characterState.Punch2)
                {
                    Character1.getsHit(gameTime, false);
                    //Character1.health -= 100;
                    //Character1.stun(gameTime);
                    //Character1.moveback(false);
                    //Character1.health -= 5;
                    //subtract a little of health from character
                    //Character1.getHit(false);
                }
                //Enemy1.health -= 180;


            }

            if (Enemy1.currentState == Character.characterState.Death)
            {
                Enemy1.dead = true;
            }

            //if (Enemy1.dead)
            //{
            //    levelUpTimer += gameTime.ElapsedGameTime;


            //    level++;

            //    if (levelUpTimer >= levelUpTime)
            //    {
            //        Enemy1.health = 200;
            //        //         enemies.Add(new Enemy(sheet, new Vector2(90, 350), Color.Red));


            //        Enemy1.dead = false;
            //        levelUpTimer = TimeSpan.Zero;
            //    }
            //}
            if (Character1.health <= 0)
            {
                Enemy1.characterDead = true;
            }


            CharacterHealthLabel.text = Character1.health.ToString();

            EnemyHealthLabel.text = Enemy1.health.ToString();

            LevelLabel.text = level.ToString();

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
                Enemy1.Draw(spriteBatch, pixel);
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Draw(spriteBatch);
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

            spriteBatch.DrawString(font, (Character1.hitbox.Intersects(Enemy1.hitbox)).ToString(), Vector2.Zero, Color.Yellow);

            spriteBatch.DrawString(font, Enemy1.getHit.ToString(), new Vector2(100, 0), Color.Teal); //always true

            spriteBatch.DrawString(font, Character1.getHit.ToString(), new Vector2(170, 0), Color.OrangeRed); //not always true


            //spriteBatch.DrawString(font, Enemy1.getsHit.)

            spriteBatch.DrawString(font, Character1.getHit.ToString(), new Vector2(0, 50), Color.LimeGreen);



            //    idle.Draw(spriteBatch);            
            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
