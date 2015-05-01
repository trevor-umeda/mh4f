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


            player1 = new Player(standing);
            player1.Sprite.AddAnimation("standing", 0, 0, 144, 288, 8, 0.1f, CharacterState.STANDING);
            player1.Sprite.AddAnimation("backwalk", 0, 288, 244, 288, 7, 0.1f, CharacterState.STANDING);
            player1.Sprite.AddAnimation("crouching", 0, 576, 176, 288, 2, 0.1f, CharacterState.CROUCHING, "crouchingidle");
            player1.Sprite.AddAnimation("crouchingidle", 0, 864, 176, 288, 6, 0.1f, CharacterState.CROUCHING);
            player1.Sprite.AddAnimation("crouchingup", 0, 1152, 176, 288, 4, 0.1f, CharacterState.CROUCHING);
            player1.Sprite.AddAnimation("walk", 0, 1440, 244, 288, 7, 0.1f, CharacterState.STANDING);
            player1.Sprite.AddAnimation("jumpup", 0, 1728, 136, 320, 1, 0.1f, CharacterState.AIRBORNE);
            player1.Sprite.AddAnimation("jumpdown", 0, 2048, 136, 380, 2, 0.1f, CharacterState.AIRBORNE);
            player1.Sprite.AddAnimation("jumptop", 0, 2428, 192, 280, 11, 0.1f, CharacterState.AIRBORNE);
            player1.Sprite.AddAnimation("rightdash", 0, 1440, 244, 288, 7, 0.1f, CharacterState.DASHING);
            player1.Sprite.AddAnimation("leftdash", 0, 1440, 244, 288, 7, 0.1f, CharacterState.DASHING);
            player1.Sprite.AddAnimation("aattack", 0, 2708, 264, 280, 9, 0.044f, CharacterState.STANDING, true);
            player1.Sprite.AddAnimation("backstep", 0, 2988, 240, 280, 7, 0.044f, CharacterState.BACKSTEP, true);

            player1.registerGroundMove("fireball",new List<string>{"2","3","6","A"});
            player1.registerGroundMove("aattack", new List<string> { "A" });
            player1.Position = new Vector2(100, 100);
            player1.Sprite.CurrentAnimation = "standing";
            player1.Direction = Direction.Left;
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
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player1.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
