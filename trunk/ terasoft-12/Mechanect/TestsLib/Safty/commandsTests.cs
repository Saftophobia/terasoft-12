using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Text;
using Mechanect;
using Mechanect.Exp1;

namespace TestsLib.Safty
{
    [TestFixture]
    public class commandsTests
    {
        List<string> gcommands;

        [SetUp]
        public void init()
        {
           gcommands = new List<string>(){ "constantDisplacement", "constantAcceleration", "constantVelocity", "increasingAcceleration", "decreasingAcceleration"};

        }

        [Test]
        public void shufflecommands()
        {

            List<string> gcommands1 = new List<string>();
            gcommands1= this.gcommands;
            Tools1.commandshuffler<string>(gcommands1);

            Assert.AreEqual(gcommands1[1], "constantVelocity");
           


           

            

        }

     

    }
}
