using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MH4F
{
    class SpriteAnimation
    {
         // The texture that holds the images for this sprite
        Texture2D t2dTexture;
        public Texture2D dummyTexture ;

        // True if animations are being played
        bool bAnimating = true;

        Rectangle hitbox;

        Rectangle hurtbox;

        // If set to anything other than Color.White, will colorize
        // the sprite with that color.
        Color colorTint = Color.White;

        // Screen Position of the Sprite
        Vector2 v2Position = new Vector2(0, 0);
        Vector2 v2LastPosition = new Vector2(0, 0);

        // Dictionary holding all of the FrameAnimation objects
        // associated with this sprite.
        Dictionary<string, Move> animations = new Dictionary<string, Move>();

        // Which FrameAnimation from the dictionary above is playing
        string currentAnimation = null;

        // The next animation from the dictionary to be played
        string nextAnimation = null;

        // If true, the sprite will automatically rotate to align itself
        // with the angle difference between it's new position and
        // it's previous position.  In this case, the 0 rotation point
        // is to the right (so the sprite should start out facing to
        // the right.
        bool bRotateByPosition = false;

        // Calcualted center of the sprite
        Vector2 v2Center;

        // Calculated width and height of the sprite
        int iWidth;
        int iHeight;

        int currentXVelocity;

        ///
        /// Vector2 representing the position of the sprite's upper left
        /// corner pixel.
        ///
        public Vector2 Position
        {
            get { return v2Position; }
            set
            {
                v2LastPosition = v2Position;
                v2Position = value;
            }
        }

        public Vector2 PositionCenter
        {
            get
            {
                int xPos = X + CurrentMoveAnimation.FrameWidth/2;
                int yPos = Y + CurrentMoveAnimation.FrameHeight/2;
                return new Vector2(xPos, yPos);
            }
        }

        ///
        /// The X position of the sprite's upper left corner pixel.
        ///
        public int X
        {
            get { return (int)v2Position.X; }
            set
            {
                v2LastPosition.X = v2Position.X;
                v2Position.X = value;
            }
        }

        ///
        /// The Y position of the sprite's upper left corner pixel.
        ///
        public int Y
        {
            get { return (int)v2Position.Y; }
            set
            {
                v2LastPosition.Y = v2Position.Y;
                v2Position.Y = value;
            }
        }

        ///
        /// Width (in pixels) of the sprite animation frames
        ///
        public int Width
        {
            get { return iWidth; }
        }

        ///
        /// Height (in pixels) of the sprite animation frames
        ///
        public int Height
        {
            get { return iHeight; }
        }

        ///
        /// If true, the sprite will automatically rotate in the direction
        /// of motion whenever the sprite's Position changes.
        ///
        public bool AutoRotate
        {
            get { return bRotateByPosition; }
            set { bRotateByPosition = value; }
        }
        ///
        /// Screen coordinates of the bounding box surrounding this sprite
        ///
        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, iWidth, iHeight); }
        }

        ///
        /// The texture associated with this sprite.  All FrameAnimations will be
        /// relative to this texture.
        ///
        public Texture2D Texture
        {
            get { return t2dTexture; }
        }

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public Rectangle Hurtbox
        {
            get { return hurtbox; }
        }

        ///
        /// Color value to tint the sprite with when drawing.  Color.White
        /// (the default) indicates no tinting.
        ///
        public Color Tint
        {
            get { return colorTint; }
            set { colorTint = value; }
        }

        ///
        /// True if the sprite is (or should be) playing animation frames.  If this value is set
        /// to false, the sprite will not be drawn (a sprite needs at least 1 single frame animation
        /// in order to be displayed.
        ///
        public bool IsAnimating
        {
            get { return bAnimating; }
            set { bAnimating = value; }
        }

        public int CurrentXVelocity
        {
            get { return currentXVelocity; }
            set { this.currentXVelocity = value; }
        }

        ///
        /// The FrameAnimation object of the currently playing animation
        ///
        public Move CurrentMoveAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(currentAnimation))
                    return animations[currentAnimation];
                else
                    return null;
            }
        }

        ///
        /// The string name of the currently playing animaton.  Setting the animation
        /// resets the CurrentFrame and PlayCount properties to zero.
        ///
        public string CurrentAnimation
        {
            get { return currentAnimation; }
            set
            {
                if (animations.ContainsKey(value))
                {
                    if (( currentAnimation != value && !animations[value].IsAttack) || animations[value].IsAttack || animations[value].CharacterState == CharacterState.HIT )
                    {     
                            currentAnimation = value;
                            animations[currentAnimation].CurrentFrame = 0;
                            animations[currentAnimation].PlayCount = 0;
                            nextAnimation = animations[currentAnimation].NextAnimation;
                            animations[currentAnimation].IsDone = false;
                    }
                }
            }
        }

        public SpriteAnimation(Texture2D Texture)
        {
            t2dTexture = Texture;
        }

        public void AddAnimation(Texture2D texture, string Name, int X, int Y, int Width, int Height, int Frames, float FrameLength, CharacterState characterState)
        {
            animations.Add(Name, new Move(texture, X, Y, Width, Height, Frames, FrameLength, characterState));
            iWidth = Width;
            iHeight = Height;
            v2Center = new Vector2(iWidth / 2, iHeight / 2);
        }

        public void AddAnimation(Texture2D texture, string Name, int X, int Y, int Width, int Height, int Frames, float FrameLength, CharacterState characterState, bool isAnAttack)
        {
            animations.Add(Name, new Move(texture, X, Y, Width, Height, Frames, FrameLength, characterState, isAnAttack));
            iWidth = Width;
            iHeight = Height;
            v2Center = new Vector2(iWidth / 2, iHeight / 2);
        }
        public void AddAnimation(Texture2D texture, string Name, int X, int Y, int Width, int Height, int Frames, float FrameLength, CharacterState characterState, bool isAnAttack, String NextAnmation)
        {
            animations.Add(Name, new Move(texture, X, Y, Width, Height, Frames, FrameLength, characterState, isAnAttack, NextAnmation));
            iWidth = Width;
            iHeight = Height;
            v2Center = new Vector2(iWidth / 2, iHeight / 2);
        }

        public void AddAnimation(Texture2D texture, string Name, int X, int Y, int Width, int Height, int Frames,
           float FrameLength, CharacterState characterState, string NextAnimation)
        {
            animations.Add(Name, new Move(texture, X, Y, Width, Height, Frames, FrameLength, characterState, NextAnimation));
            iWidth = Width;
            iHeight = Height;
            v2Center = new Vector2(iWidth / 2, iHeight / 2);
        }

        public void AddHitbox(String moveName, int index, Hitbox hitbox)
        {
            animations[moveName].AddHitboxInfo(index, hitbox);
        }

        public void AddHurtbox(String moveName, int index, Hitbox hitbox)
        {
            animations[moveName].AddHurtboxInfo(index, hitbox);
        }

        public void SetMoveProperties(String moveName, int hitstun)
        {
            animations[moveName].Hitstun = hitstun;

        }

        public Move GetAnimationByName(string Name)
        {
            if (animations.ContainsKey(Name))
            {
                return animations[Name];
            }
            else
            {
                return null;
            }
        }

        public void MoveBy(int x, int y)
        {
            v2LastPosition = v2Position;
            v2Position.X += x;
            v2Position.Y += y;
            currentXVelocity += x;
        }

        public void Update(GameTime gameTime, Direction direction)
        {
            // Don't do anything if the sprite is not animating
            if (bAnimating)
            {
                // If there is not a currently active animation
                if (CurrentMoveAnimation == null)
                {
                    // Make sure we have an animation associated with this sprite
                    if (animations.Count > 0)
                    {
                        // Set the active animation to the first animation
                        // associated with this sprite
                        string[] sKeys = new string[animations.Count];
                        animations.Keys.CopyTo(sKeys, 0);
                        CurrentAnimation = sKeys[0];
                    }
                    else
                    {
                        return;
                    }
                }

                // Run the Animation's update method
                CurrentMoveAnimation.Update(gameTime);
                Hitbox info = CurrentMoveAnimation.CurrentHitboxInfo;

                if (info != null)
                {
                    if (direction == Direction.Left)
                    {
                        hitbox.Height = info.Height;
                        hitbox.Width = info.Width;
                        hitbox.X = info.XPos - info.Width / 2 + (int)v2Position.X;
                        hitbox.Y = info.YPos - info.Height / 2 + (int)v2Position.Y;
                    }
                    else
                    {
                        hitbox.Height = info.Height;
                        hitbox.Width = info.Width;
                        
                        hitbox.X = (int)v2Position.X + CurrentMoveAnimation.FrameWidth - info.Width/2 - info.XPos;
                        hitbox.Y = info.YPos - info.Height / 2 + (int)v2Position.Y;
                    }                  
                }
                else
                {
                    hitbox = new Rectangle();         
                }

                Hitbox hurtboxInfo = CurrentMoveAnimation.CurrentHurtboxInfo;

                if (hurtboxInfo != null)
                {
                    if (direction == Direction.Left)
                    {
                        hurtbox.Height = hurtboxInfo.Height;
                        hurtbox.Width = hurtboxInfo.Width;
                        hurtbox.X = hurtboxInfo.XPos - hurtboxInfo.Width / 2 + (int)v2Position.X;
                        hurtbox.Y = hurtboxInfo.YPos - hurtboxInfo.Height / 2 + (int)v2Position.Y;
                    }
                    else
                    {
                        hurtbox.Height = hurtboxInfo.Height;
                        hurtbox.Width = hurtboxInfo.Width;

                        hurtbox.X = (int)v2Position.X + CurrentMoveAnimation.FrameWidth - hurtboxInfo.Width / 2 - hurtboxInfo.XPos;
                        hurtbox.Y = hurtboxInfo.YPos - hurtboxInfo.Height / 2 + (int)v2Position.Y;
                    }         
                }
                else
                {
                    hurtbox = new Rectangle();
                }
                // Check to see if there is a "followup" animation named for this animation
                if (!String.IsNullOrEmpty(CurrentMoveAnimation.NextAnimation))
                {
                    // If there is, see if the currently playing animation has
                    // completed a full animation loop
                    if (CurrentMoveAnimation.PlayCount > 0)
                    {
                        // If it has, set up the next animation
                        CurrentAnimation = CurrentMoveAnimation.NextAnimation;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int XOffset, int YOffset, Direction direction)
        {
            if (bAnimating)
            {
                if (direction == Direction.Right)
                {
                    spriteBatch.Draw(CurrentMoveAnimation.Texture, (v2Position + new Vector2(XOffset, YOffset) + v2Center),
                                                   CurrentMoveAnimation.FrameRectangle, colorTint,
                                                   0, v2Center, 1f, SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    spriteBatch.Draw(CurrentMoveAnimation.Texture, (v2Position + new Vector2(XOffset, YOffset) + v2Center),
                                                   CurrentMoveAnimation.FrameRectangle, colorTint,
                                                   0, v2Center, 1f, SpriteEffects.None, 0);
                }

                Color translucentRed = Color.Red * 0.5f;
                spriteBatch.Draw(dummyTexture, hitbox, translucentRed);

                Color translucentBlue = Color.Blue * 0.5f;
                spriteBatch.Draw(dummyTexture, hurtbox, translucentBlue);
            }                
        }
    }
}
