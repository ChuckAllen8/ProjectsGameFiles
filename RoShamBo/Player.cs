using System;
using System.Collections.Generic;
using System.Text;

namespace RoShamBo
{
    class Player
    {
        protected string winningPhrase;
        protected string choice;

        public virtual void MakeChoice() { }

        public void Winner()
        {
            Console.WriteLine(winningPhrase);
        }

        public string Choice 
        {
            get { return choice; }
        }
    }
}
