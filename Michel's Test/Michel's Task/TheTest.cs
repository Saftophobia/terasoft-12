using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Michel_s_Task
{
    [TestFixture]
    public class TheTest
    {
        Fibonacci fibo;

        [TestFixtureSetUp]
        public void Init()
        {
            fibo = new Fibonacci();
        }

        [TestFixtureTearDown]
        public void Dispose()
        {

        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(-1, fibo.GetFibonnacci(-5));
            Assert.AreEqual(-1, fibo.GetFibonnacci(-2));
            Assert.AreEqual(-1, fibo.GetFibonnacci(-1));
            Assert.AreEqual(1, fibo.GetFibonnacci(0));
            Assert.AreEqual(1, fibo.GetFibonnacci(1));
            Assert.AreEqual(2, fibo.GetFibonnacci(2));
            Assert.AreEqual(8, fibo.GetFibonnacci(5));
        }
    }
}
