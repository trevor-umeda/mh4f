
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MH4F
{
    public class Move : SpriteAnimation, ICloneable
    {
        private Hitbox[] hitboxInfo;

        private Hitbox[] hurtboxInfo;

        private CharacterState characterState;

        bool canCancelMove = false;

        private HitInfo hitInfo;

        public Hitbox CurrentHitboxInfo
        {
            get { return hitboxInfo[CurrentFrame]; }
        }

        public Hitbox CurrentHurtboxInfo
        {
            get { return hurtboxInfo[CurrentFrame]; }
        }

        public bool CanCancelMove
        {
            get { return canCancelMove; }
            set { canCancelMove = value; }
        }
        public HitInfo HitInfo
        {
            get { return hitInfo; }
            set { hitInfo = value; } 
        }

        public CharacterState CharacterState
        {
            get { return characterState; }
            set { characterState = value; }
        }

        public void AddHitboxInfo(int index, Hitbox hitbox)
        {
            hitboxInfo[index] = hitbox;
        }

        public void AddHurtboxInfo(int index, Hitbox hitbox)
        {
            hurtboxInfo[index] = hitbox;
        }
      

        public Move(Rectangle FirstFrame, int Frames)
        {
            RectInitialFrame = FirstFrame;
            FrameCount = Frames;
            hitboxInfo = new Hitbox[Frames];
            hurtboxInfo = new Hitbox[Frames];
        }

        public Move(int X, int Y, int Width, int Height, int Frames)
        {
            RectInitialFrame = new Rectangle(X, Y, Width, Height);
            FrameCount = Frames;
            hitboxInfo = new Hitbox[Frames];
            hurtboxInfo = new Hitbox[Frames];
        }

        public Move(int X, int Y, int Width, int Height, int Frames, float frameLength)
        {
            RectInitialFrame = new Rectangle(X, Y, Width, Height);
            FrameCount = Frames;
            hitboxInfo = new Hitbox[Frames];
            hurtboxInfo = new Hitbox[Frames];
            FrameLength = frameLength;
        }

        public Move(Texture2D texture, int X, int Y, int Width, int Height, int Frames, float frameLength, CharacterState CharacterState)
        {
            Texture = texture;
            RectInitialFrame = new Rectangle(X, Y, Width, Height);
            FrameCount = Frames;
            hitboxInfo = new Hitbox[Frames];
            hurtboxInfo = new Hitbox[Frames];
            FrameLength = frameLength;
            characterState = CharacterState;
        }

        public Move(Texture2D texture, int X, int Y, int Width, int Height, int Frames, float frameLength, CharacterState CharacterState, bool IsAnAttack)
        {
            Texture = texture;
            RectInitialFrame = new Rectangle(X, Y, Width, Height);
            FrameCount = Frames;
            hitboxInfo = new Hitbox[Frames];
            hurtboxInfo = new Hitbox[Frames];
            FrameLength = frameLength;
            characterState = CharacterState;
            IsAttack = IsAnAttack;
        }

        public Move(Texture2D texture, int X, int Y, int Width, int Height, int Frames, float frameLength, CharacterState CharacterState, bool IsAnAttack, String strNextAnimation)
        {
            Texture = texture;
            RectInitialFrame = new Rectangle(X, Y, Width, Height);
            FrameCount = Frames;
            hitboxInfo = new Hitbox[Frames];
            hurtboxInfo = new Hitbox[Frames];
            FrameLength = frameLength;
            characterState = CharacterState;
            IsAttack = IsAnAttack;
            NextAnimation = strNextAnimation;
        }

        public Move(Texture2D texture, int X, int Y, int Width, int Height, int Frames, float frameLength, bool IsAnAttack)
        {
            Texture = texture;
            RectInitialFrame = new Rectangle(X, Y, Width, Height);
            FrameCount = Frames;
            hitboxInfo = new Hitbox[Frames];
            hurtboxInfo = new Hitbox[Frames];
            FrameLength = frameLength;
            IsAttack = IsAnAttack;
        }

        public Move(Texture2D texture, int X, int Y,
            int Width, int Height, int Frames,
            float frameLength, CharacterState CharacterState, string strNextAnimation)
        {
            Texture = texture;
            RectInitialFrame = new Rectangle(X, Y, Width, Height);
            FrameCount = Frames;
            hitboxInfo = new Hitbox[Frames];
            hurtboxInfo = new Hitbox[Frames];
            FrameLength = frameLength;
            characterState = CharacterState;
            NextAnimation = strNextAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            FrameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (FrameTimer > FrameLength)
            {
                FrameTimer = 0.0f;
                CurrentFrame = (CurrentFrame + 1) % FrameCount;

                if (CurrentFrame == 0)
                {
                    PlayCount = (int)MathHelper.Min(PlayCount + 1, int.MaxValue);
                    IsDone = true;
                }

            }
        }

        object ICloneable.Clone()
        {
            return new HitAnimation(Texture, this.RectInitialFrame.X, this.RectInitialFrame.Y,
                                      this.RectInitialFrame.Width, this.RectInitialFrame.Height,
                                      FrameCount, this.FrameLength, this.characterState, NextAnimation);
        }
    }
    
}
