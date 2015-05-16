using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MH4F
{
    abstract class Player
    {
         // The SpriteAnimation object that holds the graphical and animation data for this object
        SpriteAnimation sprite;

        SpecialInputManager specialInputManager;

        ControlSetting controlSetting;

        // The speed at which the sprite will close with it's target
        float speed = 1f;

        KeyboardState prevKeyboardState;

        Direction directionFacing;

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

        bool canCancelMove = false;

        Vector2 currentVelocity = new Vector2(0, 0);

        bool isInInterruptableAnimation = false;

        readonly Vector2 gravityModifierV = new Vector2(0, 600f);

        readonly Vector2 initialJumpSpeed = new Vector2(0, -400);

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

        public SpecialInputManager SpecialInputManager
        {
            get { return specialInputManager; }
        }

        public ControlSetting ControlSetting
        {
            get { return controlSetting; }
            set
            {
                this.controlSetting = value;
                specialInputManager.ControlSetting = value;
            }
        }

        public Vector2 Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
        }

        public Direction Direction
        {
            get { return directionFacing; }
            set { directionFacing = value; }
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

        public bool IsCancealable
        {
            get { return Sprite.CurrentMoveAnimation != null && 
                (Sprite.CurrentMoveAnimation.IsAttack && Sprite.CurrentMoveAnimation.IsDone ||
                !Sprite.CurrentMoveAnimation.IsAttack); }
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

        public void registerGroundMove(String name, List<String> input)
        {
            SpecialInputManager.registerGroundMove(name, input);
        }

        public Player(Texture2D texture, int xPosition)
        {
            sprite = new SpriteAnimation(texture);
            specialInputManager = new SpecialInputManager();
            Position = new Vector2(xPosition, 100);
            ControlSetting = new ControlSetting();
        }

        public void processBasicMovement(GameTime gameTime, KeyboardState ks)
        {
            // I dislike having this in the player class, but atm it fits really well. 
            //
            if (Sprite.CurrentMoveAnimation == null || 
                Sprite.CurrentMoveAnimation.CharacterState != CharacterState.AIRBORNE)
            {            
                if (IsCancealable)
                {
                    if (ks.IsKeyDown(controlSetting.Controls["down"]))
                    {
                        Crouch();
                    }
                    else
                    {
                        UnCrouch();
                    }
                    if (ks.IsKeyDown(controlSetting.Controls["right"]))
                    {
                        if (Direction == Direction.Right)
                        {
                            ForwardWalk();
                        }
                        else
                        {
                            BackWalk();
                        }
                    }
                    if (ks.IsKeyDown(controlSetting.Controls["left"]))
                    {
                        if (Direction == Direction.Right)
                        {
                            BackWalk(); 
                        }
                        else
                        {
                            ForwardWalk();    
                        }            
                    }
                    if (ks.IsKeyDown(controlSetting.Controls["up"]))
                    {
                        if (ks.IsKeyDown(controlSetting.Controls["right"]))
                        {
                            Jump(Direction.Right);
                        }
                        else if (ks.IsKeyDown(controlSetting.Controls["left"]))
                        {
                            Jump(Direction.Left);
                        }
                        else
                        {
                            Jump();
                        }
                        
                    }
                }

                if (!ks.IsKeyDown(controlSetting.Controls["right"]) && !ks.IsKeyDown(controlSetting.Controls["left"]) && !ks.IsKeyDown(controlSetting.Controls["down"]) && !ks.IsKeyDown(controlSetting.Controls["up"]) && Sprite.CurrentMoveAnimation != null)
                {
                    if (IsCancealable)
                    {
                        Neutral();
                    }

                }
            }
            else if (IsAirborne)
            {
                Jumping(gameTime);
            }

        }

        public void Update(GameTime gameTime, KeyboardState ks)
        {
            if (Sprite.CurrentMoveAnimation != null && (Sprite.CurrentMoveAnimation.CharacterState != CharacterState.HIT && IsCancealable) || canCancelMove)
            {
                String moveName = SpecialInputManager.checkMoves(Sprite.CurrentMoveAnimation.CharacterState, Direction, ks);
                if (moveName == null)
                {
                    processBasicMovement(gameTime, ks);
                }
                else
                {
                    canCancelMove = false;
                    Sprite.CurrentAnimation = moveName;
                }

            }
           
           // If dashing adjust velocity
           //

            // Parse the special inputs here
            //
            if (Sprite.CurrentAnimation == "backstep")
            {
                Backstep();
            }
            if (Sprite.CurrentAnimation == "dash")
            {
                Dash();
            }

            //if(Sprite.CurrentMoveAnimation.IsAttack)

           float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
           Position += CurrentVelocity * time;
            if (active)
                sprite.Update(gameTime, Direction);

            prevKeyboardState = Keyboard.GetState();
        }
        public virtual void Backstep()
        {  
        }
        public virtual void Dash()
        {  
        }
        public virtual void BackWalk()
        {
            Sprite.CurrentAnimation = "backwalk";
        }

        public virtual void ForwardWalk()
        {

            Sprite.CurrentAnimation = "walk";
        }

        public virtual void Neutral()
        {
            if (CurrentVelocity.X > 0)
            {
                currentVelocity.X -= 1000;
            }
            if (CurrentVelocity.X < 0)
            {
                currentVelocity.X += 100;
            }
            Sprite.CurrentAnimation = "standing";
        }

        public virtual void Jump(Direction directionJumped)
        {
            if (directionJumped == Direction.Right)
            {
                CurrentVelocity = new Vector2(JumpHorizontalSpeed, 0);
            }
            else if (directionJumped == Direction.Left)
            {
                CurrentVelocity = new Vector2(-JumpHorizontalSpeed, 0);
            }
            IsAirborne = true;
            CurrentVelocity += InitialJumpVelocity;
            Sprite.CurrentAnimation = "jumpup";
        }

        public virtual void Jump()
        {
            IsAirborne = true;
            CurrentVelocity += InitialJumpVelocity;
            Sprite.CurrentAnimation = "jumpup";
        }

        public virtual void Jumping(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            CurrentVelocity += gravityModifierV * time;
            if (currentVelocity.Y > 0)
            {
                Sprite.CurrentAnimation = "jumpdown";
            }
            else if (currentVelocity.Y > -230)
            {
                Sprite.CurrentAnimation = "jumptop";
            }

            // Code to detect when to stop jumping. Its really bad right now
            //
            if (Y + Sprite.CurrentMoveAnimation.FrameHeight >= 100 + 288 && currentVelocity.Y > 0)
            {
                IsAirborne = false;
                CurrentVelocity = new Vector2(0, 0);
                Position = new Vector2(Position.X, 100);
                Sprite.CurrentAnimation = "standing";
            }
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

        public void hitByEnemy()
        {
            Sprite.CurrentAnimation = "hit";
        }

        public void hitEnemy()
        {
            canCancelMove = true;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (bVisible)
            {
                sprite.Draw(spriteBatch, 0, 0, Direction);
            }
        }
    }
    
}
