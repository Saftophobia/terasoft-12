using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ClassLibrary1
{
    [TestFixture]
    class Test
    {
        [TestFixtureSetUp]
        public void Init()
        {
        }

        [Test]
        public void TestFac()
        {
            Assert.True(Fac.Factorial(-2) == -1, "negative values should return -1");
            Assert.True(Fac.Factorial(0) == 1, "factorial 0 is 1");
            Assert.True(Fac.Factorial(1) == 1, "fac 1 is 1");
            Assert.True(Fac.Factorial(5) == 120, "factorial 5 is 120");
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
        }
    }
}
