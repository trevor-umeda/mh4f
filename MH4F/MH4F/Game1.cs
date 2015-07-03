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
        Texture2D background;
        Rectangle testHitbox;
        Rectangle mainFrame;
        HitInfo testHitInfo;

        int gameWidth = 1512;
        int gameHeight = 720;

        int screenWidth = 1024;
        int screenHeight = 720;

        int healthBarMargin = 0;
        int healthBarMargin2 = 0;

        int comboNumber = 0;
        ComboManager comboManager;

        ContentManager content;
       
        SpriteFont spriteFont;
        Camera2d cam;
        int frameRate = 0;
        int frameCounter = 0;

        // Amount of time (in seconds) to display each frame
        private float frameLength = 0.016f;


        // Amount of time that has passed since we last animated
        private float frameTimer = 0.0f;

        TimeSpan elapsedTime = TimeSpan.Zero;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
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
            cam = new Camera2d(gameWidth, screenWidth);
            cam.Pos = new Vector2(512.0f, 360.0f);
            mainFrame = new Rectangle(0, 0, gameWidth, gameHeight);            
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("testf");
            comboManager = new ComboManager(spriteFont);
            // TODO: use this.Content to load your game content here
            standing = Content.Load<Texture2D>("combinedsprite");
            Texture2D blocking = Content.Load<Texture2D>("standblock");
            Texture2D knockdown = Content.Load<Texture2D>("KNOCKDOWN");
            Texture2D wakeup = Content.Load<Texture2D>("WAKEUP");
            Texture2D falldown = Content.Load<Texture2D>("FALLDOWN");
            Texture2D down = Content.Load<Texture2D>("DOWN");
            Texture2D hitground = Content.Load<Texture2D>("HITGROUND");

            background = Content.Load<Texture2D>("back_ggxxac_london");

            player1 = new LongSwordPlayer(standing, 100, 288, comboManager);
            loadCharacterData("LongSword", player1);
            player1.RegisterGroundMove("fireball",new List<string>{"2","3","6","A"});
            player1.RegisterGroundMove("battack", new List<string> { "B" });
            player1.RegisterGroundMove("aattack", new List<string> { "A" });

            player1.Sprite.CurrentAnimation = "standing";
            player1.Direction = Direction.Right;

            player1.HealthBar = Content.Load<Texture2D>("HealthBar2");

            healthBarMargin = ((screenWidth / 2) - player1.HealthBar.Width) / 2;
            // Set player 1 default controls
            //

            player1.ControlSetting.setControl("down", Keys.Down);
            player1.ControlSetting.setControl("right", Keys.Right);
            player1.ControlSetting.setControl("left", Keys.Left);
            player1.ControlSetting.setControl("up", Keys.Up);
            player1.ControlSetting.setControl("a", Keys.A);
            player1.ControlSetting.setControl("b", Keys.S);


            player2 = new LongSwordPlayer(standing, 600, 288, comboManager);

            loadCharacterData("LongSword", player2);

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

            player2.HealthBar = Content.Load<Texture2D>("HealthBar2");
            healthBarMargin2 = (((screenWidth / 2) - player1.HealthBar.Width) / 2) + (screenWidth / 2);

            // Create a 1x1 white texture.
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);

            dummyTexture.SetData(new Color[] { Color.White });
            player1.Sprite.dummyTexture = dummyTexture;
            player2.Sprite.dummyTexture = dummyTexture;
            testHitbox = new Rectangle(100, 100, 100, 100);

            testHitInfo = new HitInfo(3, 20, Hitzone.HIGH);
            testHitInfo.IsHardKnockDown = true;
            testHitInfo.AirUntechTime = 8000;
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
            frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (frameTimer > frameLength)
            {
                frameTimer = 0.0f;
                // TODO: Add your update logic here
                player1.Update(gameTime, Keyboard.GetState());

                player2.Update(gameTime, Keyboard.GetState());

                adjustPlayerPositioning();

                keepPlayersInBound();

                // Detect player collisions
                //
                if (player1.Sprite.Hitbox.Intersects(player2.Sprite.Hurtbox) && !player1.HasHitOpponent)
                {
                    comboManager.player1LandedHit(player2.CharacterState);
                    player2.hitByEnemy(Keyboard.GetState(), player1.Sprite.CurrentMoveAnimation.HitInfo);
                    player1.hitEnemy();
                    System.Diagnostics.Debug.WriteLine("We ahve collision at " + player1.Sprite.CurrentMoveAnimation.CurrentFrame);
                }
                else if (player2.Sprite.Hitbox.Intersects(player1.Sprite.Hurtbox) && !player2.HasHitOpponent)
                {
                    comboManager.player2LandedHit(player1.CharacterState);
                    player1.hitByEnemy(Keyboard.GetState(), player2.Sprite.CurrentMoveAnimation.HitInfo);
                    player2.hitEnemy();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    Console.WriteLine("Test STuff");

                    player1.hitByEnemy(Keyboard.GetState(), testHitInfo);
                    player1.CurrentHealth -= 10;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.O))
                {
                    cam.Move(new Vector2(1, 0));

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

                adjustCamera();
                comboManager.decrementComboTimer();
                base.Update(gameTime);
            }
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
            //spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        cam.getTransformation(GraphicsDevice /*Send the variable that has your graphic device here*/));
 
            spriteBatch.Draw(background, mainFrame,Color.White);
            //spriteBatch.Draw(dummyTexture, testHitbox, translucentRed);
            player2.Draw(spriteBatch);
            player1.Draw(spriteBatch);

            string health = string.Format("Health: {0}", player1.CurrentHealth);
            
            spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 33), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 32), Color.White);
            spriteBatch.DrawString(spriteFont, health, new Vector2(50, 50), Color.Black);
        
            spriteBatch.End();

            spriteBatch.Begin();
            // Player 1 health and special bar
            //
            spriteBatch.Draw(player1.HealthBar, new Rectangle(healthBarMargin,
                       20, (int)(player1.HealthBar.Width * ((double)player1.CurrentHealth / player1.MaxHealth)), 44), new Rectangle(0, 45, player1.HealthBar.Width, 44), Color.Red);

            //Draw the box around the health bar
            spriteBatch.Draw(player1.HealthBar, new Rectangle(healthBarMargin,
                  20, player1.HealthBar.Width, 44), new Rectangle(0, 0, player1.HealthBar.Width, 44), Color.White);

            spriteBatch.Draw(player1.HealthBar, new Rectangle(healthBarMargin,
                       675, (int)(player1.HealthBar.Width * ((double)player1.CurrentSpecial / player1.MaxSpecial)), 44), new Rectangle(0, 45, player1.HealthBar.Width, 44), Color.Blue);

            //Draw the box around the health bar
            spriteBatch.Draw(player1.HealthBar, new Rectangle(healthBarMargin,
                  675, player1.HealthBar.Width, 44), new Rectangle(0, 0, player1.HealthBar.Width, 44), Color.White);


            // Player 2 health and special bar
            //
            spriteBatch.Draw(player2.HealthBar, new Rectangle(healthBarMargin2,
                  20, (int)(player2.HealthBar.Width * ((double)player2.CurrentHealth / player2.MaxHealth)), 44), new Rectangle(0, 45, player2.HealthBar.Width, 44), Color.Red);

            //Draw the box around the health bar
            spriteBatch.Draw(player2.HealthBar, new Rectangle(healthBarMargin2,
                  20, player2.HealthBar.Width, 44), new Rectangle(0, 0, player2.HealthBar.Width, 44), Color.White);

            spriteBatch.Draw(player2.HealthBar, new Rectangle(healthBarMargin2,
                       675, (int)(player2.HealthBar.Width * ((double)player2.CurrentSpecial / player2.MaxSpecial)), 44), new Rectangle(0, 45, player2.HealthBar.Width, 44), Color.Blue);

            //Draw the box around the health bar
            spriteBatch.Draw(player2.HealthBar, new Rectangle(healthBarMargin2,
                  675, player2.HealthBar.Width, 44), new Rectangle(0, 0, player2.HealthBar.Width, 44), Color.White);

            comboManager.displayComboMessage(spriteBatch);
          
            
            spriteBatch.End();

           
            base.Draw(gameTime);
        }

        protected void adjustCamera()
        {       
            int newCamPosition = (player1.CenterX + player2.CenterX) / 2;
            cam.X = newCamPosition;
        }

        protected void adjustPlayerPositioning()
        {
            Vector2 player1Center = player1.Sprite.PositionCenter;
            Vector2 player2Center = player2.Sprite.PositionCenter;
            // Detect Player Collision. Ghetto atm and full of bugs but its a start as a base
            //

            int currentPlayer1XVel = Math.Abs(player1.Sprite.CurrentXVelocity);
            int currentPlayer2XVel = Math.Abs(player2.Sprite.CurrentXVelocity);

            // If the players are close enough, and they are heading in the opposite directions, then we can calculate collision movement
            //
            if ((Math.Abs(player1Center.X - player2Center.X) < 80))
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
                    else if (player2.Sprite.CurrentXVelocity < 0)
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
                    else if (player2.Sprite.CurrentXVelocity > 0)
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
        }

        protected void keepPlayersInBound()
        {
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

            if (player1.X < cam.LeftEdge)
            {
                player1.X = cam.LeftEdge;
            }

            if (player1.X + player1.Sprite.Width > cam.RightEdge)
            {
                player1.X = cam.RightEdge - player1.Sprite.Width;
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
            if (player2.X < cam.LeftEdge)
            {
                player2.X = cam.LeftEdge;
            }
            if (player2.X + player2.Sprite.Width > cam.RightEdge)
            {
                player2.X = cam.RightEdge - player2.Sprite.Width;
            }
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
                        if (sHb.Length >= 10)
                        {
                            // Hacky way to add moves
                            //
                            if (sHb[9] == "true")
                            {
                                player.Sprite.AddAnimation(spriteTextures[sHb[0]], sHb[1], int.Parse(sHb[2]), int.Parse(sHb[3]), int.Parse(sHb[4]),
                                                          int.Parse(sHb[5]), int.Parse(sHb[6]), float.Parse(sHb[7]), moveState, true);
                            }
                            else
                            {
                                player.Sprite.AddAnimation(spriteTextures[sHb[0]], sHb[1], int.Parse(sHb[2]), int.Parse(sHb[3]), int.Parse(sHb[4]),
                                                          int.Parse(sHb[5]), int.Parse(sHb[6]), float.Parse(sHb[7]), moveState, sHb[9]);
                            }
                            
                        }
                        else
                        {
                           player.Sprite.AddAnimation(spriteTextures[sHb[0]], sHb[1], int.Parse(sHb[2]), int.Parse(sHb[3]), int.Parse(sHb[4]),
                           int.Parse(sHb[5]), int.Parse(sHb[6]), float.Parse(sHb[7]), moveState);
                        }
                        
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
            
            // Load Hurtbox info
            //
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

            // Load Move info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "Moves.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                while (sreader.Peek() >= 0)
                {
                    String movesInfo = sreader.ReadLine();
                    if (movesInfo.Contains("- MoveName"))
                    {

                    }
                    else
                    {
                        Console.WriteLine(movesInfo);
                        String[] sHb = movesInfo.Split(';');
                        Console.WriteLine(sHb[0]);
                        Hitzone hitZone;
                        Enum.TryParse(sHb[3], true, out hitZone);
                        Boolean isHardKnockdown = false;
                        if (sHb[5] == "true")
                        {
                            isHardKnockdown = true;
                        }
                        HitInfo hitInfo = new HitInfo(int.Parse(sHb[1]), int.Parse(sHb[2]), hitZone);
                        hitInfo.IsHardKnockDown = isHardKnockdown;
                        hitInfo.Damage = int.Parse(sHb[4]);
                        hitInfo.AirUntechTime = int.Parse(sHb[6]);
                        hitInfo.AirXVelocity = int.Parse(sHb[7]);
                        hitInfo.AirYVelocity = -int.Parse(sHb[8]);

                        player.SetAttackMoveProperties(sHb[0], hitInfo);
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
        }
    }
}
