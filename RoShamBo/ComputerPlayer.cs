using System;
using System.Collections.Generic;
using System.Text;

namespace RoShamBo
{
    class ComputerPlayer : Player
    {
        private Random numberMaker;
        
        public ComputerPlayer()
        {
            base.winningPhrase = "I, Mr. Computer, am The Machine!";
            numberMaker = new Random();
        }

        public override void MakeChoice()
        {
            int decision = numberMaker.Next(0, 3);
            switch (decision)
            {
                case 0:
                    base.choice = "R";
                    break;
                case 1:
                    base.choice = "P";
                    break;
                case 2:
                    base.choice = "S";
                    break;
                default:
                    base.choice = "Failure";
                    break;
            }
        }
    }
}
