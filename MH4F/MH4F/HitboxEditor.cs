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
    public class HitboxEditor : Microsoft.Xna.Framework.Game
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
      
        ControlSetting player1Controls = new ControlSetting();
        ControlSetting player2Controls = new ControlSetting();

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
   
        private int hitstop = 0;

        Hitbox hitbox;
        Hitbox hurtboxInfo;

        String hitboxType = "HITBOX";
        List<string> animationsList;
        int index = 0;
        KeyboardState lastState;
        public HitboxEditor()
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

            projectileManager = new ProjectileManager(Content);
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
           
            characterSelection = new CharacterSelectList(Content);
           
            player1CharacterId = "LongSword";
     

            PlayerFactory playerFactory = new PlayerFactory();
            playerFactory.DummyTexture = dummyTexture;
            player1 = playerFactory.createCharacter(player1CharacterId, Content, 1, comboManager, throwManager, superManager, projectileManager);

            //player1.Sprite.a
            roundManager = new RoundManager(player1, player2);
            animationsList = player1.Sprite.AnimationsList;
            player1.Sprite.CurrentAnimation = animationsList[index];
            player1.Sprite.CurrentAnimation = "hit";
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
                hitbox = player1.Sprite.CurrentMoveAnimation.CurrentHitboxInfo;
                hurtboxInfo = player1.Sprite.CurrentMoveAnimation.CurrentHurtboxInfo;
                // Ready hitbox info
                //
                if (hitboxType == "HURTBOX")
                {
                    if (hurtboxInfo != null)
                    {
                        player1.Sprite.Hurtbox = hurtboxInfo.getHitboxRectangle(player1.Sprite.Hurtbox, player1.Direction, player1.Sprite.Position, player1.Sprite.CurrentMoveAnimation.FrameWidth);
                    }
                    else
                    {
                        player1.Sprite.Hurtbox = new Rectangle(100, 100, 100, 100);
                        hurtboxInfo = new Hitbox("100", "100", "100", "100");
                    }

                }
                if (hitboxType == "HITBOX")
                {
                    if (hitbox != null)
                    {
                        player1.Sprite.Hitbox = hitbox.getHitboxRectangle(player1.Sprite.Hitbox, player1.Direction, player1.Sprite.Position, player1.Sprite.CurrentMoveAnimation.FrameWidth);
                    }
                    else
                    {
                        player1.Sprite.Hitbox = new Rectangle();
                    }

                }
               
                // Set up the hurtbox for the move
                //
               
                if (lastState.IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    player1.Sprite.CurrentMoveAnimation.CurrentFrame++;
                }
                if (lastState.IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    player1.Sprite.CurrentMoveAnimation.CurrentFrame--;
                }
                if (lastState.IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    // player1.Sprite.CurrentAnimation;
                    index--;
                    if (index < 0)
                    {
                        index = 0;
                    }
                    player1.Sprite.CurrentAnimation = animationsList[index];
                }
                if (lastState.IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    // player1.Sprite.CurrentAnimation;
                    index++;
                    if (index >= animationsList.Count)
                    {
                        index = animationsList.Count - 1;
                    }
                    player1.Sprite.CurrentAnimation = animationsList[index];
                }
                if (hitboxType == "HURTBOX")
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        hurtboxInfo.XPos++;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        hurtboxInfo.XPos--;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        hurtboxInfo.YPos--;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        hurtboxInfo.YPos++;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.K))
                    {
                        hurtboxInfo.Width++;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.H))
                    {
                        hurtboxInfo.Width--;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.J))
                    {
                        hurtboxInfo.Height--;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.U))
                    {
                        hurtboxInfo.Height++;
                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        hitbox.XPos++;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        hitbox.XPos--;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        hitbox.YPos--;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        hitbox.YPos++;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.K))
                    {
                        hitbox.Width++;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.H))
                    {
                        hitbox.Width--;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.J))
                    {
                        hitbox.Height--;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.U))
                    {
                        hitbox.Height++;
                    }
                }
            }
            lastState = Keyboard.GetState();
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

            //spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        cam.getTransformation(GraphicsDevice /*Send the variable that has your graphic device here*/));

                //    spriteBatch.Draw(dummyTexture, test, translucentRed);

            player1.Draw(spriteBatch);

            projectileManager.drawAllProjectiles(spriteBatch);

            string health = string.Format("Move: {0}", player1.Sprite.CurrentAnimation);

            spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 33), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 32), Color.White);
            spriteBatch.DrawString(spriteFont, health, new Vector2(50, 50), Color.Black);
            if (hitbox != null)
            {
                string hitboxData = string.Format("Hitbox {5} : {4} : {0} {1} {2} {3}", hitbox.XPos, hitbox.YPos, hitbox.Width, hitbox.Height, player1.Sprite.CurrentMoveAnimation.CurrentFrame, player1.Sprite.CurrentAnimation);

                spriteBatch.DrawString(spriteFont, hitboxData, new Vector2(50, 110), Color.Black);
            }
            if (hurtboxInfo != null)
            {
                string hurtboxData = string.Format("Hurtbox {5} : {4} : {0} {1} {2} {3}", hurtboxInfo.XPos, hurtboxInfo.YPos, hurtboxInfo.Width, hurtboxInfo.Height, player1.Sprite.CurrentMoveAnimation.CurrentFrame, player1.Sprite.CurrentAnimation);

                spriteBatch.DrawString(spriteFont, hurtboxData, new Vector2(50, 80), Color.Black);
            }
          
           
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, roundManager.displayTime(), new Vector2(500, 30), Color.Black);
            spriteBatch.DrawString(spriteFont, roundManager.displayTime(), new Vector2(501, 31), Color.White);
            comboManager.displayComboMessage(spriteBatch);
    
            spriteBatch.End();
            
            base.Draw(gameTime);
        }  
    }
}
