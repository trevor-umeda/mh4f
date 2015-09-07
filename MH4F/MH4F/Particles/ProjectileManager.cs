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

        // TODO this method signature is kinda long...
        //
        public void createProjectile(ProjectileAnimation projectileAnimation)
        {            
            projectiles.Add(projectileAnimation);
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
