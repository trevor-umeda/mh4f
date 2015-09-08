﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MH4F
{
    public class ProjectileManager
    {
        List<ProjectileAnimation> projectiles;

        public List<ProjectileAnimation> Projectiles
        {
            get
            {
                return projectiles;
            }
        }
        public ProjectileManager()
        {
            projectiles = new List<ProjectileAnimation>();
        }

        // TODO this method signature is kinda long...
        //
        public void createProjectile(ProjectileAnimation projectileAnimation)
        {            
            projectiles.Add(projectileAnimation);
        }

        public void checkHitOnPlayers(Player player1, Player player2, ComboManager comboManager, RoundManager roundManager, KeyboardState Keyboard)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                ProjectileAnimation projectile = projectiles[i];
                if (projectile.PlayerNumber == 1 && projectile.Hitbox.Intersects(player2.Sprite.Hurtbox) )
                {
                    comboManager.player1LandedHit(player2.CharacterState);
                    player2.hitByEnemy(Keyboard, projectile.HitInfo);
                    player1.hitEnemy();
                    projectile.hitEnemy();
                    projectile.NumOfHits--;
                    if (projectile.NumOfHits <= 0)
                    {
                        projectiles.RemoveAt(i);
                    }
                    System.Diagnostics.Debug.WriteLine("We have projectile collision at " + projectile.CurrentFrame);
                    if (player2.CurrentHealth <= 0)
                    {
                        roundManager.roundEnd(1);
                    }
                }
                else if (projectile.PlayerNumber == 2 && projectile.Hitbox.Intersects(player1.Sprite.Hurtbox) && !player2.HasHitOpponent)
                {

                    comboManager.player2LandedHit(player1.CharacterState);
                    player1.hitByEnemy(Keyboard, projectile.HitInfo);
                    player2.hitEnemy();
                    projectile.hitEnemy();
                    if (projectile.NumOfHits <= 0)
                    {
                        projectiles.RemoveAt(i);
                    }
                    if (player1.CurrentHealth <= 0)
                    {
                        roundManager.roundEnd(2);
                    }
                } 
            }
          
        }

        public void updateProjectileList(GameTime gameTime)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update(gameTime);

                if (projectiles[i].Finished)
                {
                    projectiles.RemoveAt(i);
                }             
            }
        }

        public void drawAllProjectiles(SpriteBatch spriteBatch)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Draw(spriteBatch);

                if (projectiles[i].Finished)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }
    }
}
