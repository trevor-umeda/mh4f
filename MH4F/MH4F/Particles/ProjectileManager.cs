using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MH4F
{
    public class ProjectileManager
    {
        List<ProjectileAnimation> projectiles;
       
        public ProjectileManager()
        {
            projectiles = new List<ProjectileAnimation>();
        }

        public void createProjectile(Texture2D texture, int X, int Y, int Width, int Height, int Frames, int columns, float frameLength, CharacterState characterState, int timeLength)
        {
            ProjectileAnimation newProjectile = new ProjectileAnimation(texture, X, Y, Width, Height, Frames, columns, frameLength, characterState, timeLength);
            projectiles.Add(newProjectile);
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
                projectiles[i].Draw(spriteBatch, projectiles[i].Direction);

                if (projectiles[i].Finished)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }
    }
}
