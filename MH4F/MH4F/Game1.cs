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
        Player player1;
        Player player2;
        Texture2D dummyTexture;
        Rectangle testHitbox;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            // TODO: use this.Content to load your game content here
            standing = Content.Load<Texture2D>("combinedsprite");


            player1 = new LongSwordPlayer(standing, 100);
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
            player1.Sprite.AddAnimation(standing, "aattack", 0, 2708, 264, 280, 9, 0.044f, CharacterState.STANDING, true);
            // For now an "attack" until i work out cancelable frames and moves
            //
            player1.Sprite.AddAnimation(standing, "backstep", 0, 2988, 240, 280, 7, 0.05f, CharacterState.BACKSTEP, true);
            player1.Sprite.AddAnimation(standing, "dash", 0, 3268, 320, 280, 13, 0.055f, CharacterState.DASHING);
            player1.Sprite.AddAnimation(standing, "hit", 0, 3548, 260, 300, 11, 0.055f, CharacterState.HIT);
            player1.registerGroundMove("fireball",new List<string>{"2","3","6","A"});
            player1.registerGroundMove("aattack", new List<string> { "A" });
            
            player1.Sprite.CurrentAnimation = "standing";
            player1.Direction = Direction.Right;

            // Set player 1 default controls
            //

            player1.ControlSetting.setControl("down", Keys.Down);
            player1.ControlSetting.setControl("right", Keys.Right);
            player1.ControlSetting.setControl("left", Keys.Left);
            player1.ControlSetting.setControl("up", Keys.Up);
            player1.ControlSetting.setControl("a", Keys.A);

            player2 = new LongSwordPlayer(standing, 600);
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
            // For now an "attack" until i work out cancelable frames and moves
            //
            player2.Sprite.AddAnimation(standing, "backstep", 0, 2988, 240, 280, 7, 0.05f, CharacterState.BACKSTEP, true);
            player2.Sprite.AddAnimation(standing, "dash", 0, 3268, 320, 280, 13, 0.055f, CharacterState.DASHING);
            player2.Sprite.AddAnimation(standing, "hit", 0, 3548, 260, 300, 11, 0.055f, CharacterState.HIT);

            player2.registerGroundMove("fireball", new List<string> { "2", "3", "6", "A" });
            player2.registerGroundMove("aattack", new List<string> { "A" });

            player2.Sprite.CurrentAnimation = "standing";
            player2.Direction = Direction.Left;

            // Setting player 2 default controls
            //
            player2.ControlSetting.setControl("down", Keys.K);
            player2.ControlSetting.setControl("right", Keys.L);
            player2.ControlSetting.setControl("left", Keys.J);
            player2.ControlSetting.setControl("up", Keys.I);
            player2.ControlSetting.setControl("a", Keys.F);

            // Create a 1x1 white texture.
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);

            dummyTexture.SetData(new Color[] { Color.White });
            player1.Sprite.dummyTexture = dummyTexture;
            player2.Sprite.dummyTexture = dummyTexture;
            testHitbox = new Rectangle(100, 100, 100, 100);

            // Load hitbox info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream("properties.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                while (sreader.Peek() >= 0)
                {
                    String hitboxInfo = sreader.ReadLine();
                    Console.WriteLine(hitboxInfo);
                    String[] sHb = hitboxInfo.Split(';');
                    Console.WriteLine(sHb[0]);
                   
                    player1.Sprite.AddHitbox(sHb[0], Convert.ToInt32(sHb[1]), new Hitbox(sHb[2],sHb[3],sHb[4],sHb[5]));
                    player2.Sprite.AddHitbox(sHb[0], Convert.ToInt32(sHb[1]), new Hitbox(sHb[2], sHb[3], sHb[4], sHb[5]));
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
                System.IO.Stream stream = TitleContainer.OpenStream("hurtbox.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                while (sreader.Peek() >= 0)
                {
                    String hurtboxInfo = sreader.ReadLine();
                    Console.WriteLine(hurtboxInfo);
                    String[] sHb = hurtboxInfo.Split(';');
                    Console.WriteLine(sHb[0]);

                    player1.Sprite.AddHurtbox(sHb[0], Convert.ToInt32(sHb[1]), new Hitbox(sHb[2], sHb[3], sHb[4], sHb[5]));
                    player2.Sprite.AddHurtbox(sHb[0], Convert.ToInt32(sHb[1]), new Hitbox(sHb[2], sHb[3], sHb[4], sHb[5]));
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            player1.Update(gameTime, Keyboard.GetState());

            player2.Update(gameTime, Keyboard.GetState());

            if(player1.Sprite.Hitbox.Intersects(player2.Sprite.Hurtbox))
            {
                
                System.Diagnostics.Debug.WriteLine("We ahve collision at " + player1.Sprite.CurrentMoveAnimation.CurrentFrame);
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

            // Draw the safe area borders.
            Color translucentRed = Color.Red * 0.5f;
            GraphicsDevice.Clear(Color.CornflowerBlue);
           
            // TODO: Add your drawing code here
            spriteBatch.Begin();
          
            spriteBatch.Draw(dummyTexture, testHitbox, translucentRed);
            
            player2.Draw(spriteBatch);
            player1.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
