using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver
{
    /// <summary>
    /// PuzzleSolver Class
    /// </summary>
    public static class PuzzleSolver
    {
        public static string[] DICTIONARY = { "OX", "CAT", "TOY", "AT", "DOG", "CATAPULT", "T" };

        public static bool IsWord(string testWord)
        {
            if (DICTIONARY.Contains(testWord))
                return true;
            return false;
        }
    }
}
