using System;
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
        List<Projectile> player1Projectiles;
        List<Projectile> player2Projectiles;

 
        public ProjectileManager()
        {
            player1Projectiles = new List<Projectile>();
            player2Projectiles = new List<Projectile>();
        }

        public List<Projectile> getPlayerProjectiles(int playerNumber)
        {
            if (playerNumber == 1)
            {
                return player1Projectiles;
            }
            else
            {
                return player2Projectiles;
            }
        }

        public void createProjectile(Projectile projectileAnimation)
        {
            if (projectileAnimation.PlayerNumber == 1)
            {
                player1Projectiles.Add(projectileAnimation);
            }
            else
            {
                player2Projectiles.Add(projectileAnimation);
            }
        }

        public void checkHitOnPlayers(Player player1, Player player2, ComboManager comboManager, RoundManager roundManager, KeyboardState Keyboard)
        {
            for (int i = player1Projectiles.Count - 1; i >= 0; i--)
            {
                Projectile projectile = player1Projectiles[i];
                if (projectile.Hitbox.Intersects(player2.Sprite.Hurtbox) )
                {
                    comboManager.player1LandedHit(player2.CharacterState);
                    player2.hitByEnemy(Keyboard, projectile.CurrentProjectile.HitInfo);
                    player1.hitEnemy();
                    projectile.hitEnemy();
                    if (projectile.NumOfHits <= 0)
                    {
                        player1Projectiles.RemoveAt(i);
                    }
                    System.Diagnostics.Debug.WriteLine("We have projectile collision at " + projectile.CurrentProjectile.CurrentFrame);
                    if (player2.CurrentHealth <= 0)
                    {
                        roundManager.roundEnd(1);
                    }
                }
            }
                
            for( int j = player2Projectiles.Count - 1; j >=0; j--)
            {
                Projectile projectile = player2Projectiles[j];
                if (projectile.Hitbox.Intersects(player1.Sprite.Hurtbox) && !player2.HasHitOpponent)
                {
                    comboManager.player2LandedHit(player1.CharacterState);
                    player1.hitByEnemy(Keyboard, projectile.CurrentProjectile.HitInfo);
                    player2.hitEnemy();
                    projectile.hitEnemy();
                    if (projectile.NumOfHits <= 0)
                    {
                        player2Projectiles.RemoveAt(j);
                    }
                    if (player1.CurrentHealth <= 0)
                    {
                        roundManager.roundEnd(2);
                    }
                }         
            }
            
        }

        public Boolean containsPlayerProjectile(int playerNumber)
        {
            if (playerNumber == 1)
            {
                return player1Projectiles.Count > 0;
            }
            else
            {
                return player2Projectiles.Count > 0;
            }
        }

        public void updateProjectileList(GameTime gameTime)
        {
            for (int i = player1Projectiles.Count - 1; i >= 0; i--)
            {
                player1Projectiles[i].Update(gameTime);

                if (player1Projectiles[i].Finished)
                {
                    player1Projectiles.RemoveAt(i);
                }             
            }

            for (int i = player2Projectiles.Count - 1; i >= 0; i--)
            {
                player2Projectiles[i].Update(gameTime);

                if (player2Projectiles[i].Finished)
                {
                    player2Projectiles.RemoveAt(i);
                }
            }
        }

        public void drawAllProjectiles(SpriteBatch spriteBatch)
        {
            for (int i = player1Projectiles.Count - 1; i >= 0; i--)
            {
                player1Projectiles[i].Draw(spriteBatch);
            }
            for (int i = player2Projectiles.Count - 1; i >= 0; i--)
            {
                player2Projectiles[i].Draw(spriteBatch);
            }
        }
    }
}
