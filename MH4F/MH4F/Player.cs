using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MH4F
{
    public abstract class Player
    {
         // The SpriteAnimation object that holds the graphical and animation data for this object
        SpriteAnimationManager sprite;

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

        readonly private int GROUND_POS_Y = 675;

        // Health
        //
        private int health;
        private int maxHealth;
        Texture2D healthBar;

        // Special bar
        //
        private int special;
        private int maxSpecial = 100;
        Texture2D specialBar;

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
       
        Vector2 currentVelocity = new Vector2(0, 0);

        bool isInInterruptableAnimation = false;

        readonly Vector2 gravityModifierV = new Vector2(0, 900f);

        readonly Vector2 initialJumpSpeed = new Vector2(0, -600);

        readonly float jumpHorizontalSpeed = 200f;

        // Some hacky stuff to give a player slight momentum
        //
        int momentumCounter = 0;

        int momentumXMovement = 0;

        int currentXVelocity = 0;

        int airJumpLimit = 1;
        int timesJumped = 0;

        int untechTime = 0;
        public Player(Texture2D texture, int xPosition, int yHeight)
        {
            sprite = new SpriteAnimationManager(texture);
            specialInputManager = new SpecialInputManager();
            Position = new Vector2(xPosition, GROUND_POS_Y - yHeight);
            ControlSetting = new ControlSetting();
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
       
        public bool IsAirborne
        {
            get {
                return Y + Sprite.CurrentMoveAnimation.FrameHeight < GROUND_POS_Y - 30; 
            }            
        }

        public bool isGettingHit
        {
            get {
                return (Sprite.CurrentMoveAnimation.CharacterState == CharacterState.HIT || Sprite.CurrentMoveAnimation.CharacterState == CharacterState.KNOCKDOWN ||
                Sprite.CurrentMoveAnimation.CharacterState == CharacterState.AIRBORNEHIT);
            }
        }

        public bool IsCancealable
        {
            get { return Sprite.CurrentMoveAnimation != null &&
                (!isGettingHit) &&
                (Sprite.CurrentMoveAnimation.IsAttack && Sprite.CurrentMoveAnimation.IsDone ||
                !Sprite.CurrentMoveAnimation.IsAttack); }
        }

        public CharacterState CharacterState
        {
            get { return Sprite.CurrentMoveAnimation.CharacterState; }
        }

        public bool HasHitOpponent
        {
            get { return Sprite.CurrentMoveAnimation.CanCancelMove; }
            set { Sprite.CurrentMoveAnimation.CanCancelMove = value; }
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

        // Methods to set up character
        //
        public void RegisterGroundMove(String name, List<String> input)
        {
            SpecialInputManager.registerGroundMove(name, input);
        }

        public void SetAttackMoveProperties(String moveName, HitInfo hitInfo)
        {
            sprite.SetAttackMoveProperties(moveName, hitInfo);
        }

        public void SetAttackMoveProperties(String moveName, int hitstun, int blockstun, Hitzone hitzone, int damage)
        {
            HitInfo hitInfo = sprite.SetAttackMoveProperties(moveName, hitstun, blockstun, hitzone);
            hitInfo.Damage = damage;
        }

        // Methods to modify player movement and such
        //
        public void GivePlayerMomentum(int timeToGiveMomentum, int amountOfMomentum, bool momentumInDirectionFacing)
        {
            MomentumCounter = timeToGiveMomentum;
            int momentumValue = amountOfMomentum;
            if ((momentumInDirectionFacing && Direction == Direction.Left) || (!momentumInDirectionFacing && Direction == Direction.Right))
            {
                MomentumXMovement = -momentumValue;
            }
            else
            {
                MomentumXMovement = momentumValue;
            }  
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
                }
                if (!ks.IsKeyDown(controlSetting.Controls["right"]) && !ks.IsKeyDown(controlSetting.Controls["left"]) && !ks.IsKeyDown(controlSetting.Controls["down"]) && !ks.IsKeyDown(controlSetting.Controls["up"]) && Sprite.CurrentMoveAnimation != null)
                {
                    if (IsCancealable)
                    {
                        Neutral();
                    }

                }
            }
            else if (Sprite.CurrentMoveAnimation == null ||
                Sprite.CurrentMoveAnimation.CharacterState == CharacterState.AIRBORNE)
            {
                if (IsCancealable && timesJumped < airJumpLimit && prevKeyboardState.IsKeyUp(controlSetting.Controls["up"]) && ks.IsKeyDown(controlSetting.Controls["up"]))
                {
                    timesJumped++;
                    if (ks.IsKeyDown(controlSetting.Controls["right"]))
                    {
                        AirJump(Direction.Right);
                    }
                    else if (ks.IsKeyDown(controlSetting.Controls["left"]))
                    {
                        AirJump(Direction.Left);
                    }
                    else
                    {
                        AirJump();
                    }
                }
            }
        }

        public void Update(GameTime gameTime, KeyboardState ks)
        {
            Sprite.CurrentXVelocity = 0;
            if (Sprite.CurrentMoveAnimation != null && 
                IsCancealable || HasHitOpponent)
            {
                String moveName = SpecialInputManager.checkMoves(Sprite.CurrentMoveAnimation.CharacterState, Direction, ks);                
                if (moveName == null)
                {
                    // Handle basic stuff like moving, jumping and crouching
                    //
                    processBasicMovement(gameTime, ks);
                }
                else
                {
                    UnCrouch();
                    HasHitOpponent = false;
                    // Some janky logic to make sure you aren't air dashing when you shouldn't be
                    // "
                    if (moveName != "backstep" && moveName != "dash")
                    {
                        Sprite.CurrentAnimation = moveName;
                    }
                    else if(timesJumped < 1)
                    {
                        Sprite.CurrentAnimation = moveName;
                        if (IsAirborne)
                        {
                            timesJumped++;
                        }                      
                    }
                }

            }  
            // Parse the ground special inputs here
            //
            if (!IsAirborne)
            {
                performGroundSpecialMove(ks, Sprite.CurrentAnimation);
            }
            
            if (IsAirborne)
            {
                // Do airdash stuff
                //
                if (Sprite.CurrentAnimation == "backstep")
                {                                 
                    AirBackdash();                    
                }
                else if (Sprite.CurrentAnimation == "dash")
                {                    
                    AirDash();                                        
                }
                else
                {
                    // If not doing an airdash, do typical jump movement
                    //
                    AirborneMovement(gameTime);
                  
                    // Check to see if we can tech here
                    //
                    if (isGettingHit && untechTime <= 0)
                    {
                        if(ks.IsKeyDown(controlSetting.Controls["a"]))
                        {
                            Console.WriteLine("DID A TECH");
                            Sprite.CurrentAnimation = "standing";
                        }
                    }
                }                
            }
            // This is basically for jumps as only jump movement is calculated via velocity
            //
           float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
           Position += CurrentVelocity * time;
           
           sprite.Update(gameTime, Direction);
            
           // Move the player a bit for a small amount of time
           //
           if (momentumCounter > 0)
           {
               momentumCounter--;
               sprite.MoveBy(momentumXMovement, 0);
           }
           if (momentumCounter == 0)
           {
               momentumXMovement = 0;

           }

            // Logic to handle when they arelanding
            // If bottom of sprite is touching the "floor" then you are landed
            //
           if (Y + Sprite.CurrentMoveAnimation.FrameHeight >= GROUND_POS_Y && currentVelocity.Y > 0)
           {
               // Stop velocity
               //
               CurrentVelocity = new Vector2(0, 0);
               
               // Once they land, make sure we reset how many times we've jumped
               //
               timesJumped = 0;

               // If they were getting hit though, they'll hit the ground instead of just landing
               //
               if (CharacterState.KNOCKDOWN == Sprite.CurrentMoveAnimation.CharacterState || 
                   CharacterState.AIRBORNEHIT == sprite.CurrentMoveAnimation.CharacterState)
               {
                   Sprite.CurrentAnimation = "hitground";
               }
               else
               {
                   Sprite.CurrentAnimation = "standing";

               }
               // Set their position to be on the ground depending on what animation they are in
               //
               Position = new Vector2(Position.X, GROUND_POS_Y - Sprite.CurrentMoveAnimation.FrameHeight);               
           }
           cleanUp();
        }
        public virtual void Backstep()
        {  
        }
        public virtual void Dash()
        {
            
        }

        public virtual void cleanUp()
        {
            prevKeyboardState = Keyboard.GetState();
        }

        public virtual void performGroundSpecialMove(KeyboardState ks, String moveName)
        {
            
            if (Sprite.CurrentAnimation == "backstep")
            {
                Backstep();
            }
            if (Sprite.CurrentAnimation == "dash")
            {
                Dash();
                // If dash and they jump, do a dash jump
                //
                if (ks.IsKeyDown(controlSetting.Controls["up"]))
                {
                    DashJump();
                }
            }
        }

        public virtual void AirBackdash()
        {
            CurrentVelocity = new Vector2(0, 0);
            if (Sprite.isLastFrameOfAnimation())
            {
                if (Direction == Direction.Left)
                {
                    CurrentVelocity = new Vector2(JumpHorizontalSpeed, 200);
                }
                else
                {
                    CurrentVelocity = new Vector2(-JumpHorizontalSpeed, 200); ;
                }
            }
        }

        public virtual void AirDash()
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
            Jump();
        }

        public virtual void Jump()
        {
            CurrentVelocity += InitialJumpVelocity;
            Sprite.CurrentAnimation = "jumpup";
        }

        public virtual void AirJump()
        {
            //Lazy atm
            float temp = CurrentVelocity.X;
            CurrentVelocity = new Vector2(temp,InitialJumpVelocity.Y);
            Sprite.CurrentAnimation = "jumpup";
        }

        public virtual void AirJump(Direction directionJumped)
        {
            if (directionJumped == Direction.Right)
            {
                CurrentVelocity = new Vector2(JumpHorizontalSpeed, 0);
            }
            else if (directionJumped == Direction.Left)
            {
                CurrentVelocity = new Vector2(-JumpHorizontalSpeed, 0);
            }
            AirJump();
        }

        public virtual void DashJump()
        {
            if (Direction == Direction.Right)
            {
                CurrentVelocity = new Vector2(jumpHorizontalSpeed * 2, 0);
            }
            else if (Direction == Direction.Left)
            {
                CurrentVelocity = new Vector2(-jumpHorizontalSpeed * 2, 0);
            }
            Jump();
        }

        public virtual void Jumping()
        {
            if (currentVelocity.Y > 0)
            {
                Sprite.CurrentAnimation = "jumpdown";
            }
            else if (currentVelocity.Y > -230)
            {
                Sprite.CurrentAnimation = "jumptop";
            }
        }

        public virtual void AirborneMovement(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            CurrentVelocity += gravityModifierV * time;
            if (Sprite.CurrentMoveAnimation.CharacterState == CharacterState.HIT || Sprite.CurrentMoveAnimation.CharacterState == CharacterState.KNOCKDOWN
                || Sprite.CurrentMoveAnimation.CharacterState == CharacterState.AIRBORNEHIT)
            {
                // Is there special animation logic for getting hit in the air?
                //
                Console.WriteLine("GETTING HIT IN THE AIR");
                untechTime--;
            }
            else
            {
                Jumping();
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

        public void hitByEnemy(KeyboardState keyState, HitInfo hitInfo)
        {
            //Check if blocked or not
            //int hitStun, int blockStun, Hitzone hitzone, float? xVel, float? yVel
            if(isAttackBlocked(keyState, hitInfo.Hitzone))
            {
                Sprite.CurrentAnimation = "block";
                HitAnimation block = (HitAnimation)Sprite.CurrentMoveAnimation;
                block.HitStunCounter = hitInfo.Blockstun;

            }
            else
            {
                // So many different ways to get hit
                //
                Sprite.CurrentAnimation = "hit";
                Sprite.CurrentMoveAnimation.CurrentFrame = 0;
                CurrentHealth -= hitInfo.Damage;

                // Not sure if this a bad idea memory wise
                //
                HitAnimation hit = (HitAnimation)Sprite.CurrentMoveAnimation;
                hit.HitStunCounter = hitInfo.Hitstun;
                hit.reset();
                if (IsAirborne)
                {
                    untechTime = hitInfo.AirUntechTime;
                    Sprite.CurrentAnimation = "falldown";
                    if (hitInfo.AirXVelocity != null && hitInfo.AirYVelocity != null)
                    {
                        if (directionFacing == Direction.Right)
                        {
                            CurrentVelocity = new Vector2((float)-hitInfo.AirXVelocity, (float)hitInfo.AirYVelocity);
                        }
                        else
                        {
                            CurrentVelocity = new Vector2((float)hitInfo.AirXVelocity, (float)hitInfo.AirYVelocity);
                        }
                    }
                }
                else
                {
                    GivePlayerMomentum(5, 4, false);
                    if (hitInfo.IsHardKnockDown)
                    {
                        // Make em HARD DOWN
                        //
                        Sprite.CurrentAnimation = "knockdown";

                    }
                }
                
            }
           
        }

        private Boolean isAttackBlocked(KeyboardState keyState, Hitzone hitzone)
        {
            // Check to see if the right direction is held
            //
            if ((directionFacing == Direction.Right && keyState.IsKeyDown(controlSetting.Controls["left"])) ||
                (directionFacing == Direction.Left && keyState.IsKeyDown(controlSetting.Controls["right"])))
            {
                // Check to see if the right zone is held.
                // Right now just check to see if they have down held or not
                //
                if ((hitzone == Hitzone.LOW && keyState.IsKeyDown(controlSetting.Controls["down"])) ||
                    (hitzone == Hitzone.HIGH && keyState.IsKeyUp(controlSetting.Controls["down"])) ||
                    hitzone == Hitzone.MID)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public void hitEnemy()
        {
            HasHitOpponent = true;
            giveSpecialMeter(10);
        }

        public void giveSpecialMeter(int amount)
        {
            CurrentSpecial += amount;
            if (CurrentSpecial > MaxSpecial)
            {
                CurrentSpecial = MaxSpecial;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (bVisible)
            {
                sprite.Draw(spriteBatch, 0, 0, Direction);     
            }
        }

        // Boring accessors without special implementations. Keeping them down here just cus of how many there are...
        //
        public SpriteAnimationManager Sprite
        {
            get { return sprite; }
        }

        public SpecialInputManager SpecialInputManager
        {
            get { return specialInputManager; }
        }
        public int CurrentHealth
        {
            get { return health; }
            set { health = value; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public Texture2D HealthBar
        {
            get { return healthBar; }
            set { healthBar = value; }
        }

        public int CurrentSpecial
        {
            get { return special; }
            set { special = value; }
        }

        public int MaxSpecial
        {
            get { return maxSpecial; }
            set { maxSpecial = value; }
        }

        public Texture2D SpecialBar
        {
            get { return specialBar; }
            set { specialBar = value; }
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

        public int CenterX
        {
            get { return sprite.CenterX; }
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

        public int MomentumCounter
        {
            get { return momentumCounter; }
            set { this.momentumCounter = value; }
        }

        public int MomentumXMovement
        {
            get { return momentumXMovement; }
            set { this.momentumXMovement = value; }
        }

    }
    
}
