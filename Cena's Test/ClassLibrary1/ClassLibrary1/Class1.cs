using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ClassLibrary1
{

    [TestFixture]
    class Class1
    {

        NoobAdder adder;
        [TestFixtureSetUp]
        public void Init()
        {

            adder = new NoobAdder();
            adder.noobAdder();
           
        }

        [TestFixtureTearDown]
        public void Dispose()
        {

        }
        [Test]
        public void False()
        {
           

            Assert.AreEqual(5, adder.number);
        }
        [Test]
        public void True()
        {
          

            Assert.AreEqual(10, adder.number);
        }


        public static void main()
        {
        }

    }
}
