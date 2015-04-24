using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MH4F
{
    class Player
    {
         // The SpriteAnimation object that holds the graphical and animation data for this object
        SpriteAnimation sprite;

        InputManager inputManager;

        // The speed at which the sprite will close with it's target
        float speed = 1f;

        KeyboardState prevKeyboardState;

        // These two integers represent a clipping range for determining bounding-box style
        // collisions.  They return the bounding box of the sprite trimmed by a horizonal and
        // verticle offset to get a collision cushion
        int collisionBufferX = 0;
        int collisionBufferY = 0;

        // Determine the status of the sprite.  An inactive sprite will not be updated but will be drawn.
        bool active = true;

        // Determines if the sprite should track towards a v2Target.  If set to false, the sprite
        // will not move on it's own towards v2Target, and will not process pathing information
        bool movingTowardsTarget = true;

        // If true, the sprite can collide with other objects.  Note that this is only provided as a flag
        // for testing with outside code.
        bool bCollidable = true;

        // If true, the sprite will be drawn to the screen
        bool bVisible = true;

        // Whether the character is crouching
        bool isCrouching = false;

        bool isAirborne = false;

        Vector2 currentVelocity = new Vector2(0, 0);

        bool isInInterruptableAnimation = false;

        readonly Vector2 gravityModifierV = new Vector2(0, 200f);

        readonly Vector2 initialJumpSpeed = new Vector2(0, -300);

        readonly float jumpHorizontalSpeed = 100f;


        public enum CharacterStates : byte
        {
            Idle,
            WalkForward,
            WalkBackward,
            Guard,
            Jump
        }
        public SpriteAnimation Sprite
        {
            get { return sprite; }
        }

        public InputManager InputManager
        {
            get { return inputManager; }
        }
        public Vector2 Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
        }

        public int X
        {
            get { return sprite.X; }
            set { sprite.X = value; }
        }

        ///
        /// The Y position of the sprite's upper left corner pixel.
        ///
        public int Y
        {
            get { return sprite.Y; }
            set { sprite.Y = value; }
        }

        public int HorizontalCollisionBuffer
        {
            get { return collisionBufferX; }
            set { collisionBufferX = value; }
        }

        public int VerticalCollisionBuffer
        {
            get { return collisionBufferY; }
            set { collisionBufferY = value; }
        }

        public bool IsVisible
        {
            get { return bVisible; }
            set { bVisible = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        public bool IsMoving
        {
            get { return movingTowardsTarget; }
            set { movingTowardsTarget = value; }
        }

        public bool IsCollidable
        {
            get { return bCollidable; }
            set { bCollidable = value; }
        }

        public bool IsCrouching
        {
            get { return isCrouching; }
            set { isCrouching = value; }
        }

        public bool IsAirborne
        {
            get { return isAirborne; }
            set { isAirborne = value; }
        }

        public bool IsNotAttacking
        {
            get { return Sprite.CurrentMoveAnimation != null && (Sprite.CurrentMoveAnimation.IsAttack && Sprite.CurrentMoveAnimation.IsDone || !Sprite.CurrentMoveAnimation.IsAttack); }
        }

        public Vector2 CurrentVelocity
        {
            get { return currentVelocity; }
            set { currentVelocity = value; }
        }

        public Vector2 InitialJumpVelocity
        {
            get { return initialJumpSpeed; }
        }

        public float JumpHorizontalSpeed
        {
            get { return jumpHorizontalSpeed; }
        }

        public bool IsInInterruptableAnimation
        {
            get { return isInInterruptableAnimation; }
            set { isInInterruptableAnimation = value; }
        }

        public Rectangle BoundingBox
        {
            get { return sprite.BoundingBox; }
        }

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    sprite.BoundingBox.X + collisionBufferX,
                    sprite.BoundingBox.Y + collisionBufferY,
                    sprite.Width - (2 * collisionBufferX),
                    sprite.Height - (2 * collisionBufferY));
            }
        }

        public void registerMove(String name, List<String> input)
        {
            InputManager.registerMove(name, input);
        }

        public Player(Texture2D texture)
        {
            sprite = new SpriteAnimation(texture);
            inputManager = new InputManager();
        }

        public void Update(GameTime gameTime, KeyboardState ks)
        {
            
                InputManager.checkMoves(ks);
            
           
            if (!IsAirborne)
            {
                if (ks.IsKeyDown(Keys.A))
                {
                    Sprite.CurrentAnimation = "aattack";
                }
                if (IsNotAttacking)
                {
                    if (ks.IsKeyDown(Keys.Down))
                    {
                        Crouch();
                    }
                    else
                    {
                        UnCrouch();
                    }
                    if (ks.IsKeyDown(Keys.Right))
                    {
                        Sprite.CurrentAnimation = "backwalk";
                        Sprite.MoveBy(3, 0)
        ;
                    }
                    if (ks.IsKeyDown(Keys.Left))
                    {
                        Sprite.CurrentAnimation = "walk";
                        Sprite.MoveBy(-3, 0);
                    }
                    if (ks.IsKeyDown(Keys.Up))
                    {
                        if (ks.IsKeyDown(Keys.Right))
                        {
                            CurrentVelocity = new Vector2(JumpHorizontalSpeed, 0);
                        }
                        else if (ks.IsKeyDown(Keys.Left))
                        {
                            CurrentVelocity = new Vector2(-JumpHorizontalSpeed, 0);
                        }
                        IsAirborne = true;
                        CurrentVelocity += InitialJumpVelocity;
                        Sprite.CurrentAnimation = "jumpup";
                    }
                }
                
                if (!ks.IsKeyDown(Keys.Right) && !ks.IsKeyDown(Keys.Left) && !ks.IsKeyDown(Keys.Down) && !ks.IsKeyDown(Keys.Up) && Sprite.CurrentMoveAnimation != null)
                {
                    if (IsNotAttacking)
                    {
                        Sprite.CurrentAnimation = "standing";
                    }
                    
                }
            }

            if (IsAirborne)
            {
                float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Position += CurrentVelocity * time;
                
                CurrentVelocity += gravityModifierV * time;
                if (currentVelocity.Y > 0)
                {
                    Sprite.CurrentAnimation = "jumpdown";
                }
                else if (currentVelocity.Y > -230)
                {
                    Sprite.CurrentAnimation = "jumptop";
                }
                if (Y >= 100)
                {
                    IsAirborne = false;
                    CurrentVelocity = new Vector2(0, 0);
                }
            }
            
      
            if (active)
                sprite.Update(gameTime);

            prevKeyboardState = Keyboard.GetState();
        }

        public void Crouch()
        {
            if (!IsCrouching)
            {
                Sprite.CurrentAnimation = "crouching";
            }
            IsCrouching = true; 
        }

        public void UnCrouch()
        {
            if (IsCrouching)
            {
                Sprite.CurrentAnimation = "crouchingup";
            }
            IsCrouching = false;

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (bVisible)
            {
                sprite.Draw(spriteBatch, 0, 0);
            }
        }
    }
    
}
