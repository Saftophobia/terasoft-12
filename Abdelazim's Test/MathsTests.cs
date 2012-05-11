using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Tests
{
    public class MathsTests
    {

        //this method will be automatically called before any tests are run
        [TestFixtureSetUp]
        public void Init()
        {
        }

        //this method will be automatically called after all the tests run
        [TestFixtureTearDown]
        public void Dispose()
        {

        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(0, Maths.Fibonacci(0));
        }

        [Test]
        public void Test2()
        {
            Assert.AreEqual(1, Maths.Fibonacci(1));
        }

        [Test]
        public void Test3()
        {
            Assert.AreEqual(55, Maths.Fibonacci(10));
        }

    }
}
