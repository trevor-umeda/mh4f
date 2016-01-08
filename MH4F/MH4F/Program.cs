using System;

namespace MH4F
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (HitboxEditor game = new HitboxEditor())
            {
                game.Run();
            }
        }
    }
#endif
}

