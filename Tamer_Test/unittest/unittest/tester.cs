using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace unittest
{
    [TestFixture]

    public class tester
    {

        [Test]

        public void test()
        {

            Program m = new Program();
            Assert.AreEqual(0, m.fibo(0));
            Assert.AreEqual(1, m.fibo(1));
            Assert.AreEqual(1, m.fibo(2));
            Assert.AreEqual(2, m.fibo(3));
            Assert.AreEqual(3, m.fibo(4));
         

        }
    }
}
