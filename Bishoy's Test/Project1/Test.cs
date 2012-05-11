using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Project1
{
    [TestFixture]
    public class Test
    {

        [TestFixtureSetUp]
        public void Init()
        {

        }

        [Test]
        public void TestSum1()
        {
            try
            {
                Functions.Sum(null);
                Assert.Fail("sum should throw an ArgumentException if passed null as a parameter");
            } catch (Exception ex) {
                Assert.IsInstanceOf<ArgumentException>(ex, "sum should throw an ArgumentException if passed null as a parameter");
            }
        }

        [Test]
        public void TestSum2()
        {
            Assert.AreEqual(10, Functions.Sum(new int[] { 1, 2, 3, 4 }), "passed 1, 2, 3, 4 as parameters, expected sum is 10");
        }

        [Test]
        public void TestSum()
        {
            Assert.AreEqual(0, Functions.Sum(new int[] { }), "passed empty array as parameter, expected sum is 0");
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
        }

    }

}
