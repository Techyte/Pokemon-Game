using System;
using System.Text;

namespace PokemonGame.General
{
    // Extension method modified from: https://stackoverflow.com/questions/4133377/splitting-a-string-number-every-nth-character-number
    // 2nd answer
    static class StringExtensions {

        public static string InsertCharEveryNChar(this String s, int n, char characterToInsert) 
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (i % n == 0 && i != 0)
                    sb.Append(characterToInsert);
                sb.Append(s[i]);
            }
            return sb.ToString();
        }

        public static string[] SplitIntoParts(this string s, int n)
        {
            s = s.InsertCharEveryNChar(n, ']');
            return s.Split(']');
        }
    }
}