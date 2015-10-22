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
using System.Diagnostics;
using System.Threading;

namespace MH4F
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Player player1;
        public Player player2;
        Texture2D dummyTexture;
        Texture2D background;
        Texture2D menuBg;
        Rectangle testHitbox;
        Rectangle mainFrame;
        HitInfo testHitInfo;

        String player1CharacterId; 
        String player2CharacterId;

        ControlSetting player1Controls = new ControlSetting();
        ControlSetting player2Controls = new ControlSetting();

        private GameState gameState;
        private Thread backgroundThread;
        private bool isLoading = false;

        int comboNumber = 0;
        ComboManager comboManager;
        ThrowManager throwManager;
        SuperManager superManager;
        ProjectileManager projectileManager;

        RoundManager roundManager;
        ContentManager content;
       
        SpriteFont spriteFont;
        Camera2d cam;
        int frameRate = 0;
        int frameCounter = 0;

        private SoundEffect effect;

        CharacterSelectList characterSelection;

        // Amount of time (in seconds) to display each frame
        private float frameLength = 0.016f;


        // Amount of time that has passed since we last animated
        private float frameTimer = 0.0f;

        TimeSpan elapsedTime = TimeSpan.Zero;
        SoundEffectInstance soundEffectInstance;

        private int hitstop = 0;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = Config.Instance.ScreenHeight;
            graphics.PreferredBackBufferWidth = Config.Instance.ScreenWidth;
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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            cam = new Camera2d(Config.Instance.GameWidth, Config.Instance.ScreenWidth, Config.Instance.GameHeight, Config.Instance.ScreenHeight);
            cam.Pos = new Vector2(512.0f, 360.0f);
            mainFrame = new Rectangle(-450, 0, 2400, Config.Instance.GameHeight);
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("testf");
            comboManager = new ComboManager(spriteFont);

            BGMManager bgmManager = new BGMManager(Content);

            projectileManager = new ProjectileManager();
            throwManager = new OneButtonThrowManager();
            superManager = new BasicSuperManager(cam);
            background = Content.Load<Texture2D>("back_ggxxac_london");
            menuBg = Content.Load<Texture2D>("bg2");
           
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);

            dummyTexture.SetData(new Color[] { Color.White });
                        
   
            testHitbox = new Rectangle(100, 100, 100, 100);

            testHitInfo = new HitInfo(3, 20, Hitzone.HIGH);
            testHitInfo.IsHardKnockDown = true;
            testHitInfo.AirUntechTime = 8000;
            testHitInfo.AirXVelocity = 80;
            testHitInfo.AirYVelocity = -100;

            effect = Content.Load<SoundEffect>("slap_large");
            
            
            //MediaPlayer.Play(bgmManager.getRandomBGM());
            //MediaPlayer.Volume = 0.4f;

            player1Controls.setControl("down", Keys.Down);
            player1Controls.setControl("right", Keys.Right);
            player1Controls.setControl("left", Keys.Left);
            player1Controls.setControl("up", Keys.Up);
            player1Controls.setControl("a", Keys.A);
            player1Controls.setControl("b", Keys.S);
            player1Controls.setControl("c", Keys.D);
            player1Controls.setControl("d", Keys.Z);

            player2Controls.setControl("down", Keys.K);
            player2Controls.setControl("right", Keys.L);
            player2Controls.setControl("left", Keys.J);
            player2Controls.setControl("up", Keys.I);
            player2Controls.setControl("a", Keys.F);
            player2Controls.setControl("b", Keys.G);
            player2Controls.setControl("c", Keys.H);
            player2Controls.setControl("d", Keys.V);

            characterSelection = new CharacterSelectList(Content);
            gameState = GameState.LOADING;
            player1CharacterId = "LongSword";
            player2CharacterId = "LongSword";
           
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
            if (gameState == GameState.PLAYERSELECT)
            {
                characterSelection.moveCharacterSelection(Keyboard.GetState(), player1Controls.Controls, player2Controls.Controls);
                if (characterSelection.selectionLocked())
                {
                    player1CharacterId = characterSelection.Player1CharacterId;
                    player2CharacterId = characterSelection.Player1CharacterId;
                    gameState = GameState.LOADING;
                }
            }
            if (gameState == GameState.LOADING && !isLoading) //isLoading bool is to prevent the LoadGame method from being called 60 times a seconds
            {      
                backgroundThread = new Thread(LoadGame);
                isLoading = true;

                //start backgroundthread
                backgroundThread.Start();
            }
            else if (gameState == GameState.PLAYING)
            {
                frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //  if (frameTimer > frameLength)
                //{
                frameTimer = 0.0f;

                if (superManager.isInSuperFreeze())
                {
                    if (superManager.playerPerformingSuper() == 1)
                    {
                        player1.Update(gameTime, Keyboard.GetState(), false);
                    }
                    else
                    {
                        player2.Update(gameTime, Keyboard.GetState(), false);
                    }

                    superManager.processSuperFreeze();
                }
                else
                {
                    projectileManager.updateProjectileList(gameTime);
                    if (hitstop > 0)
                    {
                        player1.Update(gameTime, Keyboard.GetState(), true);

                        player2.Update(gameTime, Keyboard.GetState(), true);
                    }
                    else
                    {
                        player1.Update(gameTime, Keyboard.GetState(), false);

                        player2.Update(gameTime, Keyboard.GetState(), false);
                    }

                    if (hitstop == 0 && !superManager.isInSuperFreeze())
                    {

                        adjustPlayerPositioning();

                        keepPlayersInBound();

                        throwManager.updateCharacterState(1, player1);
                        throwManager.updateCharacterState(2, player2);

                        // Detect player collisions
                        //
                        if (player1.Sprite.Hitbox.Intersects(player2.Sprite.Hurtbox) && !player1.HasHitOpponent)
                        {
                            // TODO make this not hardcoded
                            //
                            hitstop = 7;
                            comboManager.player1LandedHit(player2.CharacterState);
                            player2.hitByEnemy(Keyboard.GetState(), player1.Sprite.CurrentMoveAnimation.HitInfo);
                            player1.hitEnemy();
                            System.Diagnostics.Debug.WriteLine("We have collision at " + player1.Sprite.CurrentMoveAnimation.CurrentFrame);
                            if (player2.CurrentHealth <= 0)
                            {
                                roundManager.roundEnd(1);
                            }
                        }
                        else if (player2.Sprite.Hitbox.Intersects(player1.Sprite.Hurtbox) && !player2.HasHitOpponent)
                        {
                            comboManager.player2LandedHit(player1.CharacterState);
                            player1.hitByEnemy(Keyboard.GetState(), player2.Sprite.CurrentMoveAnimation.HitInfo);
                            player2.hitEnemy();
                            if (player1.CurrentHealth <= 0)
                            {
                                roundManager.roundEnd(2);
                            }
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.P))
                        {
                            Console.WriteLine("Test STuff");
                            cam.Zoom = 1.2f;
                            //    player1.hitByEnemy(Keyboard.GetState(), testHitInfo);
                            player1.CurrentHealth -= 10;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.O))
                        {
                            cam.Y += 3;
                        }
                        projectileManager.checkHitOnPlayers(player1, player2, comboManager, roundManager, Keyboard.GetState());
                        elapsedTime += gameTime.ElapsedGameTime;

                        if (elapsedTime > TimeSpan.FromSeconds(1))
                        {
                            elapsedTime -= TimeSpan.FromSeconds(1);
                            frameRate = frameCounter;
                            frameCounter = 0;
                        }

                        // leftBorder.Width += 10;

                        adjustCamera();
                        comboManager.decrementComboTimer();

                        roundManager.decrementTimer(gameTime);
                        if (roundManager.isTimeOut())
                        {
                            roundManager.timeOut();
                        }

                        base.Update(gameTime);
                    }
                    else
                    {
                        
                        hitstop--;
                        if (hitstop < 0)
                        {
                            hitstop = 0;
                        }
                    }
                }
            }
                            
            //}
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
            if (gameState == GameState.PLAYERSELECT)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred,
                            BlendState.AlphaBlend,
                            null,
                            null,
                            null,
                            null,
                            cam.getTransformation(GraphicsDevice /*Send the variable that has your graphic device here*/));
                spriteBatch.Draw(menuBg, mainFrame, Color.White);
                characterSelection.Draw(spriteBatch);
                spriteBatch.End();
            }
            if (gameState == GameState.LOADING)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred,
                            BlendState.AlphaBlend,
                            null,
                            null,
                            null,
                            null,
                            cam.getTransformation(GraphicsDevice /*Send the variable that has your graphic device here*/));
                spriteBatch.Draw(menuBg, mainFrame, Color.White);
               
                spriteBatch.End();
            }
            if (gameState == GameState.PLAYING)
            {
                //spriteBatch.Begin();
                spriteBatch.Begin(SpriteSortMode.Deferred,
                            BlendState.AlphaBlend,
                            null,
                            null,
                            null,
                            null,
                            cam.getTransformation(GraphicsDevice /*Send the variable that has your graphic device here*/));

                if (superManager.isInSuperFreeze())
                {
                    Color backgroundTint = Color.Lerp(Color.White, Color.Yellow, 0.5f);
                    spriteBatch.Draw(background, mainFrame, backgroundTint);

                }
                else if (superManager.isInSuper())
                {
                    Color backgroundTint = Color.Lerp(Color.White, Color.Black, 0.5f);
                    spriteBatch.Draw(background, mainFrame, backgroundTint);
                }
                else
                {
                    spriteBatch.Draw(background, mainFrame, Color.White);
                }


                //    spriteBatch.Draw(dummyTexture, test, translucentRed);
                //   spriteBatch.Draw(dummyTexture, testHitbox, translucentRed);
                player2.Draw(spriteBatch);

                player1.Draw(spriteBatch);

                projectileManager.drawAllProjectiles(spriteBatch);

                string health = string.Format("Health: {0}", player1.CurrentHealth);

                spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 33), Color.Black);
                spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 32), Color.White);
                spriteBatch.DrawString(spriteFont, health, new Vector2(50, 50), Color.Black);

                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.DrawString(spriteFont, roundManager.displayTime(), new Vector2(500, 30), Color.Black);
                spriteBatch.DrawString(spriteFont, roundManager.displayTime(), new Vector2(501, 31), Color.White);
                comboManager.displayComboMessage(spriteBatch);


                player1.DrawGauges(spriteBatch);
                player2.DrawGauges(spriteBatch);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        protected void LoadGame()
        {
            PlayerFactory playerFactory = new PlayerFactory();
            playerFactory.DummyTexture = dummyTexture;
            player1 = playerFactory.createCharacter(player1CharacterId, Content, 1, comboManager, throwManager, superManager, projectileManager);

            // Set player 1 default controls
            //
            player1.ControlSetting = player1Controls;
           
            player2 = playerFactory.createCharacter(player2CharacterId, Content, 2, comboManager, throwManager, superManager, projectileManager);

            // Setting player 2 default controls
            //
            player2.ControlSetting = player2Controls;
            
            player1.AddSound(effect, "aattack");
            player1.AddSound(Content.Load<SoundEffect>("airbackdash_h"), "backstep");
            //player1.Sprite.AddResetInfo("aattack", 4);
          //  player1.Sprite.AddResetInfo("aattack", 6);

            roundManager = new RoundManager(player1, player2);

            gameState = GameState.PLAYING;
            isLoading = false;

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
            if ((Math.Abs(player1Center.X - player2Center.X) < 80) && player1.IsPhysical && player2.IsPhysical)
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
                if (player1.IsPhysical)
                {
                    player1.Direction = Direction.Right;
                }
                if (player2.IsPhysical)
                {
                    player2.Direction = Direction.Left;
                }
                
            }
            else
            {
                if (player1.IsPhysical)
                {
                    player1.Direction = Direction.Left;
                }
                
                if(player2.IsPhysical)
                {
                    player2.Direction = Direction.Right;
                }
               
            }
        }

        protected void keepPlayersInBound()
        {
            // Make sure the player doesn't go out of bound
            //
            if (player1.Sprite.BoundingBox.X < 0)
            {
                player1.Sprite.setXByBoundingBox(0);
            }
            if (player1.Sprite.BoundingBox.X + player1.Sprite.BoundingBox.Width > Config.Instance.GameWidth)
            {
                player1.Sprite.setXByBoundingBox(Config.Instance.GameWidth - player1.Sprite.BoundingBox.Width); 
            }

            if (player1.Sprite.BoundingBox.X < cam.LeftEdge)
            {
                player1.Sprite.setXByBoundingBox(cam.LeftEdge);         
            }

            if (player1.Sprite.BoundingBox.X + player1.Sprite.BoundingBox.Width > cam.RightEdge)
            {
                player1.Sprite.setXByBoundingBox(cam.RightEdge - player1.Sprite.BoundingBox.Width);
   
            }

            // Same out of bound checks for player 2
            //
            if (player2.Sprite.BoundingBox.X < 0)
            {
                player2.Sprite.setXByBoundingBox(0);
            }
            if (player2.Sprite.BoundingBox.X + player2.Sprite.BoundingBox.Width > Config.Instance.GameWidth)
            {
                player2.Sprite.setXByBoundingBox(Config.Instance.GameWidth - player2.Sprite.BoundingBox.Width); 
            }
            if (player2.Sprite.BoundingBox.X < cam.LeftEdge)
            {
                player2.Sprite.setXByBoundingBox(cam.LeftEdge);   
            }
            if (player2.Sprite.BoundingBox.X + player2.Sprite.BoundingBox.Width > cam.RightEdge)
            {
                player2.Sprite.setXByBoundingBox(cam.RightEdge - player2.Sprite.BoundingBox.Width);
            }
        }

        
    }
}
