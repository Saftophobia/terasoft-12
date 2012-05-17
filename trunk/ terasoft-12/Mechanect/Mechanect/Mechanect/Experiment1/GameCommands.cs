using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanect.Classes
{
    class GameCommands
    {
        private List<String> gCommands;
        public List<String> GCommands {
           
            get
            {
                return gCommands;
            }
            set
            {
                gCommands = value;
            }
           
        }

        private String name;
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        private int time;
        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }




        internal string getName()
        {
            throw new NotImplementedException();
        }
    }
}
