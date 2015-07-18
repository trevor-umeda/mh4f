using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
namespace MH4F
{
    class SoundManager
    {
        // Dictionary holding all of the FrameAnimation objects
        // associated with this sprite.
        Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        public SoundManager()
        {
        }

        public void AddSound(SoundEffect sound, String name)
        {
            sounds.Add(name, sound);
        }

        public void PlaySound(String name)
        {
           SoundEffect soundEffect = null;
            if(sounds.TryGetValue(name, out soundEffect)) 
            {
                soundEffect.Play();
            }       
        }
    }
}
