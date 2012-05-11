using NUnit.Framework;

namespace NunitTesting
{
 
    [TestFixture]
    public class FactorialTest
    {
        Factorial f;
        //marks this method as the setup method for this test class. Meaning, before any tests are run this method will be automatically called, so you should put here all the things that might be common among all your tests.
        [TestFixtureSetUp]
        public void Init()
        {
            f = new Factorial();
        }
        //marks this method as the test method for this test class. this is the test method that will be run to make sure the code works as expected.
        [Test]
        public void TestFactorial()
        {
            Assert.AreEqual(1, f.GenerateFactorial(0));
            Assert.AreEqual(1, f.GenerateFactorial(1));
            Assert.AreEqual(24, f.GenerateFactorial(4));
            Assert.AreEqual(-1, f.GenerateFactorial(-10));
        }
        //marks this method as the TearDown method for this class. Meaning, after any tests are run this method will be automatically called, here's where you should put all the unloads and such stuff.
        [TestFixtureTearDown]
        public void Dispose()
        {
        }

    }
}
