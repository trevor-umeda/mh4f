using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MH4F
{
    public class LongSwordPlayer : Player
    {
        bool DisplayShadow { get;  set;}
        int SwordLevel { get; set; }
        int SwordGauge { get; set; }

        int MaxSwordGauge = 100;
        int rekkaLevel;

        // TODO Make it so they can have multiple projectiles...
        //
        Projectile Projectile { get; set; }

        Dictionary<String, int> SwordGaugeGains { get; set; }
        Dictionary<String, int> MoveCosts { get; set; }

        Texture2D SwordGaugeBar { get; set; }
        int swordGaugeBarMargin;

        public LongSwordPlayer(int playerNumber, int xPosition, int yHeight, ComboManager comboManager, ThrowManager throwManager) 
            : base ( playerNumber, xPosition, yHeight, comboManager, throwManager)
        {
            CurrentHealth = 1000;
            MaxHealth = 1000;
            DisplayShadow = false;
            
            // D mechanic related moves
            //
            SwordLevel = 1;
            SwordGauge = 0;

            SwordGaugeGains = new Dictionary<String, int>();
            MoveCosts = new Dictionary<String, int>();

            SwordGaugeGains.Add("aattack", 20);
            MoveCosts.Add("battack", 10);
            MoveCosts.Add("backfireball", 25);
            Sprite.BoundingBoxHeight = 288;
            Sprite.BoundingBoxWidth = 90;

            rekkaLevel = 1;

            // TODO make these have to be set for every character.
            //
            ThrowRange = 200;
            BackAirDashVel = 8;
            AirDashVel = 13;
            BackStepVel = 8;
            DashVel = 8;
            BackWalkVel = 3;
            WalkVel = 4;

//            projectile = new ProjectileAnimation(texture, X, Y, Width, Height, Frames, columns, frameLength, characterState, timeLength, direction);

        }

        public override void cleanUp()
        {
            base.cleanUp();
            if (!Sprite.CurrentMoveAnimation.IsAttack)
            {
                rekkaLevel = 1;
                DisplayShadow = false;
            }
        }

        public override void checkValidityAndChangeMove(string moveName)
        {
            // If we have a designated cost for our move. Make sure we can perform it. 
            //
            int moveCostValue;
            
            // If its a d move do stuff
            //
            if (MoveCosts.TryGetValue(moveName, out moveCostValue))
            {
                if (SwordGauge - moveCostValue >= 0)
                {
                    //String move = convertRekka(moveName);
                    if (moveName == "backfireball")
                    {
                        if (!ProjectileManager.containsPlayerProjectile(PlayerNumber))
                        {
                            changeMove(moveName);
                        }
                        else
                        {
                            changeMove(determineBackupMove(moveName));
                        }

                    }
                    else
                    {
                        changeMove(moveName);
                    }
                   
                    SwordGauge = SwordGauge - moveCostValue;
                }
                else
                {
                    changeMove(determineBackupMove(moveName));
                    Console.WriteLine("Couldn't perform move cus not enough gauge");
                }
            }
           
            else
            {
                changeMove(moveName);
            }
        }

        public override void changeMove(string moveName)
        {
            if (moveName == "supera")
            {
                PerformSuperFreeze();
            }
            base.changeMove(moveName);
        }

        private String convertRekka(String moveName)
        {
            if (moveName == "rekka")
            {
                if (rekkaLevel == 1)
                {
                    return "rekkaA";
                }
                else if (rekkaLevel == 2)
                {
                    return "rekkaB";
                }
                else if (rekkaLevel == 3)
                {
                    return "rekkaC";
                }
            }
            return moveName;
        }

        public override void performGroundSpecialMove(KeyboardState ks, String moveName)
        {
            if (moveName == "fireball")
            {
                Fireball();
            }
            if (moveName == "supera")
            {
                if (Sprite.isLastFrameOfAnimation())
                {
                    SuperManager.endSuper();
                    Console.WriteLine("SPECIAL IS NOW OVER YAY");
                }
            }
            else if (moveName == "aattack")
            {

            }
            else if (moveName == "cattack")
            {
                // This seems kinda clumsy to perform every c attack. Maybe we should keep a reference the the projectile instead of cloning it.
                //
                List<Projectile> projectiles = ProjectileManager.getPlayerProjectiles(PlayerNumber);
                for (int i = projectiles.Count - 1; i >= 0; i--)
                {
                    if (Sprite.Hitbox.Intersects(projectiles[i].Hitbox))
                    {
                        projectiles[i].CurrentAnimation = "morph";
                    }         
                }
            }
            else if (moveName == "rekka")
            {
                if (Direction == Direction.Left)
                {
                    Sprite.MoveBy(-5, 0);
                }
                else
                {
                    Sprite.MoveBy(5, 0);
                }
            }
            else if (moveName == "rekkaB")
            {
                if (Direction == Direction.Left)
                {
                    Sprite.MoveBy(-5, 0);
                }
                else
                {
                    Sprite.MoveBy(5, 0);
                }
            }
            else if (moveName == "rekkaC")
            {
                if (Sprite.CurrentMoveAnimation.CurrentFrame > 4 && Sprite.CurrentMoveAnimation.CurrentFrame < 8)
                {
                    IsPhysical = false;
                    if (Direction == Direction.Left)
                    {
                        Sprite.MoveBy(-20, 0);
                    }
                    else
                    {
                        Sprite.MoveBy(20, 0);
                    }
                    GivePlayerMomentum(7, 3, true);    
                }
                else
                {
                    IsPhysical = true;
                }
            }
            else if (moveName == "backfireball")
            {
                BackFireball();
            }
            else
            {
                base.performGroundSpecialMove(ks, moveName);
            }
        }

        public void Fireball()
        {
            DisplayShadow = true;
            Dash();          
        }

        public void BackFireball()
        {
            DisplayShadow = true;
            if (Sprite.CurrentMoveAnimation.CurrentFrame == Sprite.CurrentMoveAnimation.ProjectileCreationFrame)
            {
                if(IsCastingProjectile == false)
                {
                    Console.WriteLine("Creating a fireball!");

                    Projectile clonedProjectile = Projectile;
                    clonedProjectile.CurrentAnimation = "staticprojectile"; 
                    clonedProjectile.Direction = Direction;
                    clonedProjectile.Y = Sprite.Y + 50;
                    clonedProjectile.X = Sprite.X + Sprite.Width/2 + 70;
                    clonedProjectile.PlayerNumber = PlayerNumber;
                    ProjectileManager.createProjectile(clonedProjectile);

                    Console.WriteLine("DOING BACK FIREBALL");
                    IsCastingProjectile = true;
                }              
            }
            else
            {
                IsCastingProjectile = false;
            }
        }

        public override void Backstep()
        {
            base.Backstep();
        }

        public override void Dash()
        {
            base.Dash();

            GivePlayerMomentum(7, 3, true);      
        }
        public override void BackWalk()
        {
            base.BackWalk();
            
        }

        public override void ForwardWalk()
        {
            base.ForwardWalk();
            
        }

        public override void hitEnemy()
        {
            int swordGaugeGainValue;
            if (SwordGaugeGains.TryGetValue(Sprite.CurrentAnimation, out swordGaugeGainValue))
            {
                SwordGauge += swordGaugeGainValue;
                if (SwordGauge > 100)
                {
                    SwordGauge = 100;
                }
            }
            base.hitEnemy();
        }

        public override void AddProjectile(String name, ProjectileAnimation projectileAnimation)
        {
            base.AddProjectile(name, projectileAnimation);
            if (Projectile == null)
            {
                Projectile = new Projectile();
            }
            Projectile.AddProjectile(name, projectileAnimation);
            Projectile.DummyTexture = Sprite.DummyTexture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, 0, 0, Direction);
            if (DisplayShadow)
            {
                Sprite.shadowDraw(spriteBatch, -9, 0, Direction);
            }           
        }

        public override void DrawGauges(SpriteBatch spriteBatch)
        {
            base.DrawGauges(spriteBatch);
            //Draw the special filled
            //
            spriteBatch.Draw(SwordGaugeBar, new Rectangle(swordGaugeBarMargin,
                        630, (int)(SwordGaugeBar.Width * ((double)SwordGauge / MaxSwordGauge)), 25), new Rectangle(0, 45, SwordGaugeBar.Width, 44), Color.Green);

            //Draw the box around the Special bar
            spriteBatch.Draw(SwordGaugeBar, new Rectangle(swordGaugeBarMargin,
                    630, SwordGaugeBar.Width, 25), new Rectangle(0, 0, SwordGaugeBar.Width, 44), Color.White);
        }

        public override void setUpGauges(ContentManager content, int healthBarMargin)
        {
            base.setUpGauges(content, healthBarMargin);
            SwordGaugeBar = content.Load<Texture2D>("HealthBar2");
            swordGaugeBarMargin = healthBarMargin;
        }

        public override void resetRound()
        {
            base.resetRound();
            SwordGauge = 0;
        }

    }
}
