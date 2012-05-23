using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Mechanect.Common;

namespace Mechanect.Exp1
{

    public class User1:User
    {

        public Skeleton skeleton;

        
        
        
        public Boolean Winner;

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
        private List<float> kneepos;
        public List<float> Kneepos
        { 
            get
            { 
                return kneepos;
            }
            set
            {
                kneepos = value;
            }
        }
        private List<float> kneeposr;
        public List<float> Kneeposr
        {
            get
            {
                return kneeposr;
            }
            set
            {
                kneeposr = value;
            }
        }
        private List<float[]> velocitylist;
        public List<float[]> Velocitylist
        {
            get
            {
                return velocitylist;
            }
            set
            {
                velocitylist = value;
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
        private int disqualificationTime = -1;
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

        public User1() 
        {
            this.Winner = false;
            this.disqualified = false;
            this.positions = new List<float>();
            this.kneepos = new List<float>();
            this.kneeposr = new List<float>();
            this.velocitylist = new List<float[]>();
            
        }


        
        

    }
}
