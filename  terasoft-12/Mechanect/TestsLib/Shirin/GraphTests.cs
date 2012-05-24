using System;
using NUnit.Framework;
using Mechanect.Exp1;
using System.Collections.Generic;

namespace TestLib.Shirin
{
    [TestFixture]
    public class GraphTests
    {
        PerformanceGraph g;
        List<float> velocities1=new List<float>();
        List<float> velocities2 = new List<float>();
        List<double> times1=new List<double>();
        List<double> times2 = new List<double>();
        List<string> Commands = new List<string>();
        List<double> CommandsTime = new List<double>();
        List<double> newTimings1=new List<double>();
        List<double> newTimings2 = new List<double>();

        [SetUp]
        public void Init()
        {
            g = new PerformanceGraph();
            Commands.Add("increasingAcceleration");
            Commands.Add("constantAcceleration");
            Commands.Add("decreasingAcceleration");
            Commands.Add("constantVelocity");
            CommandsTime.Add(10);
            CommandsTime.Add(10);
            CommandsTime.Add(10);
            CommandsTime.Add(10);
            velocities1.Add(0);
            velocities1.Add((float)0.8);
            velocities1.Add((float)0.4);
            velocities1.Add((float)0.6);
            velocities1.Add((float)0.8);
            velocities1.Add((float)0.1);
            velocities1.Add((float)0.6);
            velocities1.Add((float)0.2);
            velocities1.Add((float)0.7);
            times1.Add(0);
            times1.Add(1.4);
            times1.Add(3.4);
            times1.Add(8);
            times1.Add(10);
            times1.Add(11);
            times1.Add(12);
            times1.Add(15);
            times1.Add(17.6); velocities2.Add(0);
            velocities2.Add((float)1.0);
            velocities2.Add((float)1.6);
            velocities2.Add((float)1.3);
            velocities2.Add((float)1.9);
            velocities2.Add((float)1.6);
            velocities2.Add((float)1.2);
            velocities2.Add((float)1.9);
            velocities2.Add((float)1.0);
            velocities2.Add((float)1.9);
            velocities2.Add((float)0.9);
            velocities2.Add((float)0.1);
            velocities2.Add((float)1.9);
            velocities2.Add((float)1.5);
            velocities2.Add((float)1.0);
            velocities2.Add((float)0.1);
            velocities2.Add((float)2.2);
            velocities2.Add((float)2.2);
            times2.Add(0);
            times2.Add(2.0);
            times2.Add(2.4);
            times2.Add(5);
            times2.Add(7);
            times2.Add(10);
            times2.Add(12);
            times2.Add(13.5);
            times2.Add(18);
            times2.Add(18.5);
            times2.Add(19.9);
            times2.Add(25);
            times2.Add(26);
            times2.Add(26.8);
            times2.Add(30);
            times2.Add(33);
            times2.Add(35);
            times2.Add(40);
            newTimings1 = GraphEngine.GetTimings(times1);
            newTimings2 = GraphEngine.GetTimings(times2);
            GraphEngine.DrawGraphs(g, velocities1, newTimings1, velocities2, newTimings2, Commands, CommandsTime, -1, -1, 0, 0, 840);
        }

        [Test]
        public void TestAcceleration()
        {
            Assert.AreEqual(0.5,g.getP1Acc()[6]);
            Assert.AreEqual(1.8, g.getP2Acc()[12]);
        }

        [Test]
        public void TestDisplacement()
        {
            Assert.AreEqual(9.4, g.getP1Disp()[8]);
            Assert.AreEqual(2, g.getP2Disp()[1]);
        }

        [Test]
        public void TestTotalTime()
        {
            Assert.AreEqual(40, g.getTotalTime());
        }

        [Test]
        public void TestTimeSlices()
        {
            Assert.AreEqual(4, g.getTimeSpaces().Count);
        }

        [Test]
        public void TestOptimalSize()
        {
            Assert.AreEqual(32, g.getOptD().Count);
            Assert.AreEqual(32, g.getOptV().Count);
            Assert.AreEqual(32, g.getOptA().Count);
        }

        [Test]
        public void TestOptimumDisplacement()
        {
            Assert.AreEqual(840, g.getOptD()[g.getOptD().Count-1]);
            Assert.AreEqual(10, g.getOptD()[4]);
        }

        [Test]
        public void TestOptimumVelocity()
        {
            Assert.AreEqual(0, g.getOptV()[g.getOptV().Count - 1]);
            Assert.AreEqual(3, g.getOptV()[3]);
        }

        [Test]
        public void TestOptmumAcceleration()
        {
            Assert.AreEqual(0, g.getOptA()[g.getOptA().Count - 1]);
            Assert.AreEqual(5, g.getOptA()[10]);
        }

        [Test]
        public void TestXAxis()
        {
            Assert.AreEqual(30, g.GetXAxis()[3]);
        }

        [Test]
        public void TestDisplacementAxis()
        {
            Assert.AreEqual(630, g.YAxisDis()[3]);
        }

        [Test]
        public void TestVelocityAxis()
        {
            Assert.AreEqual("21.80", g.YAxisVel()[1].ToString("N2"));
        }

        [Test]
        public void TestAccelerationAxis()
        {
            Assert.AreEqual(2.5, g.YAxisAcc()[2]);
        }

        [Test]
        public void TestMaximumVelocity()
        {
            Assert.AreEqual("87.20", g.getMaxVelocity().ToString("N2"));
        }

        [Test]
        public void TestMaximumAcceleration()
        {
            Assert.AreEqual(5, g.getMaxAcceleration());
        }
    }
}
