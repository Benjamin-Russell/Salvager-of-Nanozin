using System;

namespace NanozinProject
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Nanozin game = new Nanozin())
            {
                game.Run();
            }
        }
    }
#endif
}