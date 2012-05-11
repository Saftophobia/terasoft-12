using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTestsTutorial
{
    [TestFixture]
    public class Test1
    {
        fibo fibo;
        [TestFixtureSetUp]
        public void Init()
        {
        
            fibo = new fibo();
        }
        
        [TestFixtureTearDown]
        public void Dispose()
        {

        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(120, fibo.getFibonacci(5));
        }
        
        
    }
}
