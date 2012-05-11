using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Shirin
{
    [TestFixture]
    class Test
    {
        Fib fib;

        [TestFixtureSetUp]
        public void Init()
        {
            fib = new Fib();
        }

        [TestFixtureTearDown]
        public void Dispose()
        {

        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(0, fib.GetFib(0));
        }
        [Test]
        public void Test2()
        {
            Assert.AreEqual(1, fib.GetFib(1));
        }
        [Test]
        public void Test3()
        {
            Assert.AreEqual(3, fib.GetFib(4));
        }
        [Test]
        public void Test4()
        {
            Assert.AreEqual(34, fib.GetFib(9));
        }
    }
}
