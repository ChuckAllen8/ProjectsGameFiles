using System;
using System.Collections.Generic;
using System.Text;

namespace HangMan
{
    class PhraseMaker
    {
        private Random choiceMaker;
        private List<char[]> phrases;

        public PhraseMaker()
        {
            choiceMaker = new Random();
            phrases = new List<char[]>();
            phrases.Add(new char[] { 'D', 'E', 'V', '.', 'B', 'U', 'I', 'L', 'D', ' ', 'I', 'S', ' ', 'F', 'U', 'N', '!' });
            phrases.Add(new char[] { 'C', '#', ' ', 'I', 'S', ' ', 'C', 'O', 'O', 'L' });
            phrases.Add(new char[] { 'H', 'O', 'W', ' ', 'D', 'O', ' ', 'Y', 'O', 'U', ' ', 'P', 'L', 'A', 'Y', ' ', 'T', 'H', 'I', 'S' });
            /*
            phrases.Add(new char[] { '', '', '', '', '', '', '', '', '', '', '', '', '', '' });
            phrases.Add(new char[] { '', '', '', '', '', '', '', '', '', '', '', '', '', '' });
            phrases.Add(new char[] { '', '', '', '', '', '', '', '', '', '', '', '', '', '' });
            phrases.Add(new char[] { '', '', '', '', '', '', '', '', '', '', '', '', '', '' });
            phrases.Add(new char[] { '', '', '', '', '', '', '', '', '', '', '', '', '', '' });
            phrases.Add(new char[] { '', '', '', '', '', '', '', '', '', '', '', '', '', '' });
            phrases.Add(new char[] { '', '', '', '', '', '', '', '', '', '', '', '', '', '' });
            phrases.Add(new char[] { '', '', '', '', '', '', '', '', '', '', '', '', '', '' });
            */
        }
        public char[] GetPhrase()
        {
            return phrases[choiceMaker.Next(phrases.Count)];
        }
    }
}
