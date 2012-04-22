using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanect.Classes
{
    class User1:User
    {
        /// <summary>
        /// this is the index of the array of currentCommands in the class Game which defines what is the command active
        /// </summary>
        private int activeCommand;
        public int ActiveCommand
        {
            get
            {
                return activeCommand;
            }
            set
            {
                activeCommand = value;
            }
        }
        private List<float> positions;
        public List<float> Positions
        {
            get
            {
                return positions;
            }
            set
            {
                positions = value;
            }
        }
        private bool disqualified;
        public bool Disqualified
        {
            get
            {
                return disqualified;
            }
            set
            {
                disqualified = value;
            }
        }
        private int disqualificationTime;
        public int DisqualificationTime
        {
            get
            {
                return disqualificationTime;
            }
            set
            {
                disqualificationTime = value;
            }
        }
        

    }
}
