using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTest
{
     [TestFixture]
    class Test
    {
         power p;
         power s;
         [TestFixtureSetUp]
         public void Init()
         {
             p = new power(5);
             s = new power(19);
         }

         [TestFixtureTearDown]
         public void Dispose()
         {

         }

         [Test]
         public void Test1()
         {
             Assert.AreEqual(32, p.twoPower());
         }

         [Test]
         public void Test2()
         {
             Assert.AreEqual(524288, s.twoPower());
         }
    }
}
