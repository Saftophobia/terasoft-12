using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTestsTutorial
{
    [TestFixture]
    public class TestClass
    {
        Bla b;
        //this method will be automatically called before any tests are run
        [TestFixtureSetUp]
        public void Init()
        {
            //a random number is chosen here between 5,6
            b = new Bla();
        }
        
        //this method will be automatically called after all the tests run
        [TestFixtureTearDown]
        public void Dispose()
        {

        }

        [Test]
        public void Test1()
        {
            //will succeed if the number was 5
            Assert.AreEqual(120,b.Factorial());
        }
        
        [Test]
        public void Test2()
        {
            //will succeed if the number was 6
            Assert.AreEqual(720, b.Factorial());
        }
    }
}
