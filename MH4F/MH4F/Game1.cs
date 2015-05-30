using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MH4F
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D standing;
         public Player player1;
        public Player player2;
        Texture2D dummyTexture;
        Rectangle testHitbox;

        HitInfo testHitInfo;

        int gameWidth = 1024;
        int gameHeight = 720;

        ContentManager content;
       
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = gameHeight;
            graphics.PreferredBackBufferWidth = gameWidth;
            Content.RootDirectory = "Content";

            IsFixedTimeStep = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("testf");

            // TODO: use this.Content to load your game content here
            standing = Content.Load<Texture2D>("combinedsprite");
            Texture2D blocking = Content.Load<Texture2D>("standblock");
            Texture2D knockdown = Content.Load<Texture2D>("KNOCKDOWN");
            Texture2D wakeup = Content.Load<Texture2D>("WAKEUP");
            Texture2D falldown = Content.Load<Texture2D>("FALLDOWN");
            Texture2D down = Content.Load<Texture2D>("DOWN");
            Texture2D hitground = Content.Load<Texture2D>("HITGROUND");
           
            player1 = new LongSwordPlayer(standing, 100, 288);
            player1.Sprite.AddAnimation(standing, "standing", 0, 0, 144, 288, 8, 0.1f, CharacterState.STANDING);
            player1.Sprite.AddAnimation(standing, "backwalk", 0, 288, 244, 288, 7, 0.1f, CharacterState.STANDING);
            player1.Sprite.AddAnimation(standing, "crouching", 0, 576, 176, 288, 2, 0.1f, CharacterState.CROUCHING, "crouchingidle");
            player1.Sprite.AddAnimation(standing, "crouchingidle", 0, 864, 176, 288, 6, 0.1f, CharacterState.CROUCHING);
            player1.Sprite.AddAnimation(standing, "crouchingup", 0, 1152, 176, 288, 4, 0.1f, CharacterState.CROUCHING);
            player1.Sprite.AddAnimation(standing, "walk", 0, 1440, 244, 288, 7, 0.1f, CharacterState.STANDING);
            player1.Sprite.AddAnimation(standing, "jumpup", 0, 1728, 136, 320, 1, 0.1f, CharacterState.AIRBORNE);
            player1.Sprite.AddAnimation(standing, "jumpdown", 0, 2048, 136, 380, 2, 0.1f, CharacterState.AIRBORNE);
            player1.Sprite.AddAnimation(standing, "jumptop", 0, 2428, 192, 280, 11, 0.1f, CharacterState.AIRBORNE);
            //player1.Sprite.AddAnimation(standing, "rightdash", 0, 1440, 244, 288, 7, 0.1f, CharacterState.DASHING);
            player1.Sprite.AddAnimation(standing, "aattack", 0, 2708, 264, 280, 9, 0.05f, CharacterState.STANDING, true);
            player1.Sprite.AddAnimation(standing, "battack", 0, 2708, 264, 280, 9, 0.044f, CharacterState.STANDING, true);
            
            player1.Sprite.AddAnimation(standing, "backstep", 0, 2988, 240, 280, 7, 0.05f, CharacterState.BACKSTEP, true);
            player1.Sprite.AddAnimation(standing, "dash", 0, 3268, 320, 280, 13, 0.055f, CharacterState.DASHING);
            player1.Sprite.AddAnimation(standing, "hit", 0, 3548, 260, 300, 11, 0.055f, CharacterState.HIT, "standing");
            player1.Sprite.AddAnimation(blocking, "block", 0, 0, 160, 280, 1, 0.1f, CharacterState.HIT,"standing");
            player1.Sprite.AddAnimation(knockdown, "knockdown", 0, 0, 300, 280, 8, 0.1f, CharacterState.KNOCKDOWN, "wakeup");

            player1.Sprite.AddAnimation(wakeup, "wakeup", 0, 0, 280, 272, 7, 0.1f, CharacterState.KNOCKDOWN, "standing");
            player1.Sprite.AddAnimation(falldown, "falldown",0,0, 208, 320, 8, 0.1f, CharacterState.KNOCKDOWN,"standing");
            player1.Sprite.AddAnimation(hitground, "hitground", 0, 0, 288, 192, 7, 0.1f, CharacterState.KNOCKDOWN, "wakeup");

            player1.RegisterGroundMove("fireball",new List<string>{"2","3","6","A"});
            player1.RegisterGroundMove("battack", new List<string> { "B" });
            player1.RegisterGroundMove("aattack", new List<string> { "A" });

            player1.SetAttackMoveProperties("aattack", 3, 2, Hitzone.MID);
            player1.SetAttackMoveProperties("battack", 5, 10, Hitzone.MID);
            player1.Sprite.CurrentAnimation = "standing";
            player1.Direction = Direction.Right;

            // Set player 1 default controls
            //

            player1.ControlSetting.setControl("down", Keys.Down);
            player1.ControlSetting.setControl("right", Keys.Right);
            player1.ControlSetting.setControl("left", Keys.Left);
            player1.ControlSetting.setControl("up", Keys.Up);
            player1.ControlSetting.setControl("a", Keys.A);
            player1.ControlSetting.setControl("b", Keys.S);

            player2 = new LongSwordPlayer(standing, 600, 288);
            player2.Sprite.AddAnimation(standing, "standing", 0, 0, 144, 288, 8, 0.1f, CharacterState.STANDING);
            player2.Sprite.AddAnimation(standing, "backwalk", 0, 288, 244, 288, 7, 0.1f, CharacterState.STANDING);
            player2.Sprite.AddAnimation(standing, "crouching", 0, 576, 176, 288, 2, 0.1f, CharacterState.CROUCHING, "crouchingidle");
            player2.Sprite.AddAnimation(standing, "crouchingidle", 0, 864, 176, 288, 6, 0.1f, CharacterState.CROUCHING);
            player2.Sprite.AddAnimation(standing, "crouchingup", 0, 1152, 176, 288, 4, 0.1f, CharacterState.CROUCHING);
            player2.Sprite.AddAnimation(standing, "walk", 0, 1440, 244, 288, 7, 0.1f, CharacterState.STANDING);
            player2.Sprite.AddAnimation(standing, "jumpup", 0, 1728, 136, 320, 1, 0.1f, CharacterState.AIRBORNE);
            player2.Sprite.AddAnimation(standing, "jumpdown", 0, 2048, 136, 380, 2, 0.1f, CharacterState.AIRBORNE);
            player2.Sprite.AddAnimation(standing, "jumptop", 0, 2428, 192, 280, 11, 0.1f, CharacterState.AIRBORNE);
            //player1.Sprite.AddAnimation(standing, "rightdash", 0, 1440, 244, 288, 7, 0.1f, CharacterState.DASHING);
            player2.Sprite.AddAnimation(standing, "aattack", 0, 2708, 264, 280, 9, 0.044f, CharacterState.STANDING, true);
            player2.Sprite.AddAnimation(standing, "battack", 0, 2708, 264, 280, 9, 0.044f, CharacterState.STANDING, true);
            // For now an "attack" until i work out cancelable frames and moves
            //
            player2.Sprite.AddAnimation(standing, "backstep", 0, 2988, 240, 280, 7, 0.05f, CharacterState.BACKSTEP, true);
            player2.Sprite.AddAnimation(standing, "dash", 0, 3268, 320, 280, 13, 0.055f, CharacterState.DASHING);
            player2.Sprite.AddAnimation(standing, "hit", 0, 3548, 260, 300, 11, 0.04f, CharacterState.HIT, "standing");
            player2.Sprite.AddAnimation(blocking, "block", 0, 0, 160, 280, 1, 0.1f, CharacterState.HIT, "standing");


            player2.RegisterGroundMove("fireball", new List<string> { "2", "3", "6", "A" });
            player2.RegisterGroundMove("battack", new List<string> { "B" });
            player2.RegisterGroundMove("aattack", new List<string> { "A" });
           

            player2.Sprite.CurrentAnimation = "standing";
            player2.Direction = Direction.Left;

            // Setting player 2 default controls
            //
            player2.ControlSetting.setControl("down", Keys.K);
            player2.ControlSetting.setControl("right", Keys.L);
            player2.ControlSetting.setControl("left", Keys.J);
            player2.ControlSetting.setControl("up", Keys.I);
            player2.ControlSetting.setControl("a", Keys.F);
            player2.ControlSetting.setControl("b", Keys.G);
            // Create a 1x1 white texture.
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);

            dummyTexture.SetData(new Color[] { Color.White });
            player1.Sprite.dummyTexture = dummyTexture;
            player2.Sprite.dummyTexture = dummyTexture;
            testHitbox = new Rectangle(100, 100, 100, 100);

            loadCharacterData("LongSword", player1);
            loadCharacterData("LongSword", player2);

            testHitInfo = new HitInfo(300, 20, Hitzone.HIGH);
            testHitInfo.IsHardKnockDown = true;
            testHitInfo.AirXVelocity = 80;
            testHitInfo.AirYVelocity = -100;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
              
            // TODO: Add your update logic here
            player1.Update(gameTime, Keyboard.GetState());

            player2.Update(gameTime, Keyboard.GetState());

            Vector2 player1Center = player1.Sprite.PositionCenter;
            Vector2 player2Center = player2.Sprite.PositionCenter;
            
            // Detect Player Collision. Ghetto atm and full of bugs but its a start as a base
            //

            int currentPlayer1XVel = Math.Abs(player1.Sprite.CurrentXVelocity);
            int currentPlayer2XVel = Math.Abs(player2.Sprite.CurrentXVelocity);

            // If the players are close enough, and they are heading in the opposite directions, then we can calculate collision movement
            //
            if((Math.Abs(player1Center.X - player2Center.X) < 80) )
            {
                //&& (player1.Sprite.CurrentXVelocity * player2.Sprite.CurrentXVelocity < 0)
                int velocityDiff = currentPlayer1XVel - currentPlayer2XVel;
                // Case where the velocities are equal towards each other.
                //
                if (velocityDiff == 0)
                {
                    player1.Sprite.MoveBy(-player1.Sprite.CurrentXVelocity, 0);
                    player2.Sprite.MoveBy(-player2.Sprite.CurrentXVelocity, 0);
                }
                if (player1.Direction == Direction.Right)
                {
                    if ((currentPlayer1XVel > currentPlayer2XVel) && player1.Sprite.CurrentXVelocity > 0)
                    {                        
                        player2.Sprite.MoveBy(player1.Sprite.CurrentXVelocity, 0);
                        player1.Sprite.MoveBy(-player2.Sprite.CurrentXVelocity, 0);
                    }
                    else if(player2.Sprite.CurrentXVelocity < 0)
                    {
                        player1.Sprite.MoveBy(player2.Sprite.CurrentXVelocity, 0);
                        player2.Sprite.MoveBy(-player1.Sprite.CurrentXVelocity, 0);
                    }
                }
                else
                {
                    if ((currentPlayer1XVel > currentPlayer2XVel) && player1.Sprite.CurrentXVelocity < 0)
                    {
                        
                        player2.Sprite.MoveBy(player1.Sprite.CurrentXVelocity, 0);
                        player1.Sprite.MoveBy(-player2.Sprite.CurrentXVelocity, 0);
                    }
                    else if(player2.Sprite.CurrentXVelocity > 0) 
                    {
                       
                        player1.Sprite.MoveBy(player2.Sprite.CurrentXVelocity, 0);
                        player2.Sprite.MoveBy(-player1.Sprite.CurrentXVelocity, 0);
                    }
                }
            }
            // Check to see which direction the player is facing
            //
            if (player1Center.X < player2Center.X)
            {
                player1.Direction = Direction.Right;
                player2.Direction = Direction.Left;
            }
            else
            {
                player1.Direction = Direction.Left;
                player2.Direction = Direction.Right;
            }

            // Make sure the player doesn't go out of bound
            //
            if (player1.X < 0)
            {
                player1.X = 0;
            }
            if (player1.X + player1.Sprite.CurrentMoveAnimation.FrameWidth > gameWidth)
            {
                player1.X = gameWidth - player1.Sprite.CurrentMoveAnimation.FrameWidth;
            }

            // Same out of bound checks for player 2
            //
            if (player2.X < 0)
            {
                player2.X = 0;
            }
            if (player2.X + player2.Sprite.CurrentMoveAnimation.FrameWidth > gameWidth)
            {
                player2.X = gameWidth - player2.Sprite.CurrentMoveAnimation.FrameWidth;
            }
            
            // Detect player collisions
            //
            if(player1.Sprite.Hitbox.Intersects(player2.Sprite.Hurtbox) && !player1.HasHitOpponent)
            {                

                player2.hitByEnemy(Keyboard.GetState(), player1.Sprite.CurrentMoveAnimation.HitInfo);
                player1.hitEnemy();
                System.Diagnostics.Debug.WriteLine("We ahve collision at " + player1.Sprite.CurrentMoveAnimation.CurrentFrame);
            }
            else if (player2.Sprite.Hitbox.Intersects(player1.Sprite.Hurtbox) && !player2.HasHitOpponent)
            {
                player1.hitByEnemy(Keyboard.GetState(), player2.Sprite.CurrentMoveAnimation.HitInfo);
                player2.hitEnemy();
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Console.WriteLine("Test STuff");

                player1.hitByEnemy(Keyboard.GetState(), testHitInfo);
            }
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            if (gameTime.IsRunningSlowly)
            {
                Console.WriteLine("iS real slow");
            }
           // leftBorder.Width += 10;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format("fps: {0}", frameRate);

            // Draw the safe area borders.
            Color translucentRed = Color.Red * 0.5f;
            GraphicsDevice.Clear(Color.CornflowerBlue);
           
            // TODO: Add your drawing code here
            spriteBatch.Begin();
          
            //spriteBatch.Draw(dummyTexture, testHitbox, translucentRed);
            
            player2.Draw(spriteBatch);
            player1.Draw(spriteBatch);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 33), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 32), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void loadCharacterData(String character, Player player)
        {
            // Load Sprite Info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character+"Sprites.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                bool readingAnimations = false;
                Dictionary<String, Texture2D> spriteTextures = new Dictionary<String, Texture2D>();
                while (sreader.Peek() >= 0)
                {
                    String spriteLine = sreader.ReadLine();
                    Console.WriteLine(spriteLine);
                    if (spriteLine == "-SPRITEANIMATIONS-")
                    {
                        readingAnimations = true;
                    }
                    else if (spriteLine == "-SPRITESHEET-")
                    {
                        readingAnimations = false;
                    }
                    else if (!readingAnimations)
                    {
                        Texture2D test = Content.Load<Texture2D>(spriteLine);
                        spriteTextures[spriteLine] = test;
                    }
                    else
                    {
                        String[] sHb = spriteLine.Split(';');
                        Console.WriteLine("Using " + sHb[0]);
                        //player2.Sprite.AddAnimation(standing, "standing", 0, 0, 144, 288, 8, 0.1f, CharacterState.STANDING);                       
                        CharacterState moveState;
                        Enum.TryParse(sHb[8], true, out moveState);
                        player.Sprite.AddAnimation(spriteTextures[sHb[0]], sHb[1], int.Parse(sHb[2]), int.Parse(sHb[3]), int.Parse(sHb[4]),
                           int.Parse(sHb[5]), int.Parse(sHb[6]), float.Parse(sHb[7]), moveState);
                    }



                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load hitbox info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character+"Hitbox.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                while (sreader.Peek() >= 0)
                {
                    String hitboxInfo = sreader.ReadLine();
                    Console.WriteLine(hitboxInfo);
                    String[] sHb = hitboxInfo.Split(';');
                    Console.WriteLine(sHb[0]);
                   
                    player.Sprite.AddHitbox(sHb[0], Convert.ToInt32(sHb[1]), new Hitbox(sHb[2], sHb[3], sHb[4], sHb[5]));
                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character+"Hurtbox.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                while (sreader.Peek() >= 0)
                {
                    String hurtboxInfo = sreader.ReadLine();
                    Console.WriteLine(hurtboxInfo);
                    String[] sHb = hurtboxInfo.Split(';');
                    Console.WriteLine(sHb[0]);                    
                    player.Sprite.AddHurtbox(sHb[0], Convert.ToInt32(sHb[1]), new Hitbox(sHb[2], sHb[3], sHb[4], sHb[5]));
                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }
        }
    }
}
