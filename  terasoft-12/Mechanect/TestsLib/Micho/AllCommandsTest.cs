using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mechanect.Exp1;

namespace Tests
{
    [TestClass]
    public class AllCommandsTest
    {
        public AllCommandsTest()
        {
        }
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        static float timeInSeconds;
        static User1 user1;
        static User1 user2;
        static List<int> timeOfCommands;
        static List<string> currentCommands;
        static float tolerance;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {

            timeInSeconds = 0F;
            user1 = new User1();
            user2 = new User1();
            timeOfCommands = new List<int> { 5, 4, 6, 3, 8, 1 };
            currentCommands = new List<string> { "increasingAcceleration", "decreasingAcceleration", 
                "constantVelocity", "constantDisplacement", "constantAcceleration", "abcd" };
            tolerance = 0F;
        }
        [TestInitialize()]
        public void MyTestInitialize()
        {
            user1.Disqualified = false;
            user1.Velocitylist = new List<float[]>();
            user2.Disqualified = false;
            user2.Velocitylist = new List<float[]>();
        }

        [TestMethod()]
        public void CheckTheCommandTest()
        {
            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(false, user1.Disqualified);
            Assert.AreEqual(false, user2.Disqualified);

            timeInSeconds = 6.0f;
            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(false, user1.Disqualified);
            Assert.AreEqual(false, user2.Disqualified);

            timeInSeconds = 6.0f;
            user1.ActiveCommand = 1;
            user2.ActiveCommand = 1;
            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(false, user1.Disqualified);
            Assert.AreEqual(false, user2.Disqualified);

            user1.ActiveCommand = 0;
            user2.ActiveCommand = 0;

            timeInSeconds = 5.0125369f;
            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(false, user1.Disqualified);
            Assert.AreEqual(false, user2.Disqualified);

            timeInSeconds = 5.005329f;
            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(true, user1.Disqualified);
            Assert.AreEqual(true, user2.Disqualified);
        }

        [TestMethod()]
        public void CommandSatisfiedTest()
        {
            bool result = Tools1.CommandSatisfied("abcd", user1.Positions, 2.5f, user1.Velocitylist);
            Assert.AreEqual(true, result);
            result = Tools1.CommandSatisfied("abcd", user2.Positions, 2.5f, user2.Velocitylist);
            Assert.AreEqual(true, result);

            timeInSeconds = 27.005329f;
            user1.ActiveCommand = 3;
            user2.ActiveCommand = 3;
            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(false, user1.Disqualified);
            Assert.AreEqual(false, user2.Disqualified);

            timeInSeconds = 27.012853f;
            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(false, user1.Disqualified);
            Assert.AreEqual(false, user2.Disqualified);
        }

        [TestMethod()]
        public void ConstantAccelerationTest()
        {
            timeInSeconds = 26.008235f;
            user1.ActiveCommand = 4;
            user2.ActiveCommand = 4;
            tolerance = 2.55f;
            user1.Velocitylist.Add(new float[] { 0.2f, 19.02f });
            user1.Velocitylist.Add(new float[] { 0.5f, 19.05f });
            user1.Velocitylist.Add(new float[] { 0.8f, 19.08f });
            user1.Velocitylist.Add(new float[] { 1.38f, 19.15f });

            user2.Velocitylist.Add(new float[] { 0.2f, 19.02f });
            user2.Velocitylist.Add(new float[] { 0.5f, 19.05f });
            user2.Velocitylist.Add(new float[] { 0.8f, 19.08f });
            user2.Velocitylist.Add(new float[] { 1.38f, 19.55f });

            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(false, user1.Disqualified);
            Assert.AreEqual(true, user2.Disqualified);
        }

        [TestMethod()]
        public void ConstantDisplacementTest()
        {
            timeInSeconds = 18.001124f;
            user1.ActiveCommand = 3;
            user2.ActiveCommand = 3;
            user1.Velocitylist.Add(new float[] { 1.0125f, 15.02485f });
            user1.Velocitylist.Add(new float[] { 1.1635f, 15.0529f });
            user1.Velocitylist.Add(new float[] { 1.04f, 15.2215f });
            user1.Velocitylist.Add(new float[] { 1.38f, 15.5f });
            user1.Velocitylist.Add(new float[] { 0.9f, 15.6125f });
            user1.Velocitylist.Add(new float[] { 0.9f, 15.8f });
            user1.Velocitylist.Add(new float[] { 1.28f, 16.002f });

            user2.Velocitylist.Add(new float[] { 1.0125f, 15.02485f });
            user2.Velocitylist.Add(new float[] { 1.1635f, 15.0529f });
            user2.Velocitylist.Add(new float[] { 1.04f, 15.2215f });
            user2.Velocitylist.Add(new float[] { 1.38f, 15.5f });
            user2.Velocitylist.Add(new float[] { 0.9f, 15.6125f });
            user2.Velocitylist.Add(new float[] { 0.9f, 15.8f });
            user2.Velocitylist.Add(new float[] { 1.28f, 15.88f });
            user2.Velocitylist.Add(new float[] { 1.28f, 16.002f });
            user2.Velocitylist.Add(new float[] { 0.5f, 16.0113f });

            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(false, user1.Disqualified);
            Assert.AreEqual(true, user2.Disqualified);
        }

        [TestMethod()]
        public void ConstantVelocityTest()
        {
            timeInSeconds = 15.0005f;
            user1.ActiveCommand = 2;
            user2.ActiveCommand = 2;
            tolerance = 0.55f;
            user1.Velocitylist.Add(new float[] { 0.845236f, 10.02f });
            user1.Velocitylist.Add(new float[] { 0.8f, 10.05f });
            user1.Velocitylist.Add(new float[] { 0.85318f, 10.08f });
            user1.Velocitylist.Add(new float[] { 0.9005f, 10.15f });

            user2.Velocitylist.Add(new float[] { 0.76851f, 10.02f });
            user2.Velocitylist.Add(new float[] { 0.77250f, 10.05f });
            user2.Velocitylist.Add(new float[] { 0.8f, 10.08f });
            user2.Velocitylist.Add(new float[] { 1.38f, 10.55f });

            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(false, user1.Disqualified);
            Assert.AreEqual(true, user2.Disqualified);
        }

        [TestMethod()]
        public void DecreasingAccelerationTest()
        {
            timeInSeconds = 9.0095f;
            user1.ActiveCommand = 1;
            user2.ActiveCommand = 1;
            tolerance = 2.55f;
            user1.Velocitylist.Add(new float[] { 0.2f, 6.02f });
            user1.Velocitylist.Add(new float[] { 0.5f, 6.05f });
            user1.Velocitylist.Add(new float[] { 0.7f, 6.08f });
            user1.Velocitylist.Add(new float[] { 1.38f, 6.5f });

            user2.Velocitylist.Add(new float[] { 0.2f, 6.02f });
            user2.Velocitylist.Add(new float[] { 0.5f, 6.05f });
            user2.Velocitylist.Add(new float[] { 0.7f, 6.08f });
            user2.Velocitylist.Add(new float[] { 1.38f, 6.15f });

            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(false, user1.Disqualified);
            Assert.AreEqual(true, user2.Disqualified);
        }

        [TestMethod()]
        public void GetWinnerTest()
        {
            List<float> distances = new List<float>(){2.001f, 2.05f, 1.5f, 3.9f};
            user1.Positions = distances;
            user2.Positions = new List<float>();
            Tools1.SetWinner(user1, user2, 9.0f);
            Assert.AreEqual(true, user1.Winner);
            Assert.AreEqual(false, user2.Winner);
        }

        [TestMethod()]
        public void GetAccelerationTest()
        {
            user1.Velocitylist.Add(new float[] { 1.0125f, 15.02485f });
            user1.Velocitylist.Add(new float[] { 1.1635f, 15.0529f });
            user1.Velocitylist.Add(new float[] { 1.04f, 15.2215f });
            user1.Velocitylist.Add(new float[] { 1.38f, 15.5f });
            user1.Velocitylist.Add(new float[] { 0.9f, 15.6125f });
            user1.Velocitylist.Add(new float[] { 0.9f, 15.8f });
            user1.Velocitylist.Add(new float[] { 1.28f, 16.002f });

            List<float> test1 = Tools1.GetAcceleration(user1.Velocitylist);
            List<float> test2 = Tools1.GetAcceleration(new List<float[]>());

            Assert.AreEqual(0, test2.Count);
            Assert.AreEqual(6, test1.Count);
        }

        [TestMethod()]
        public void IncreasingAccelerationTest()
        {
            timeInSeconds = 5.0005f;
            user1.ActiveCommand = 0;
            user2.ActiveCommand = 0;
            tolerance = 2.55f;
            user1.Velocitylist.Add(new float[] { 0.2f, 1.02f });
            user1.Velocitylist.Add(new float[] { 0.5f, 1.04f });
            user1.Velocitylist.Add(new float[] { 0.8f, 1.06f });
            user1.Velocitylist.Add(new float[] { 1.38f, 1.1f });

            user2.Velocitylist.Add(new float[] { 0.2f, 1.02f });
            user2.Velocitylist.Add(new float[] { 0.5f, 1.05f });
            user2.Velocitylist.Add(new float[] { 0.8f, 1.08f });
            user2.Velocitylist.Add(new float[] { 1.38f, 1.6f });

            Tools1.CheckTheCommand(timeInSeconds, user1, user2, timeOfCommands, currentCommands, tolerance);
            Assert.AreEqual(false, user1.Disqualified);
            Assert.AreEqual(true, user2.Disqualified);
        }
    }
}
