using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTests2
{
    [TestFixture]
    public class FactorialTest
    {
        Factorial f;
        [TestFixtureSetUp]
        public void Init()
        {
            f = new Factorial();
        }

        [Test]
        public void checkFactorial()
        {
            Assert.AreEqual(1, f.factorial(0));
            Assert.AreEqual(120, f.factorial(5));
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
        }

    }
}
