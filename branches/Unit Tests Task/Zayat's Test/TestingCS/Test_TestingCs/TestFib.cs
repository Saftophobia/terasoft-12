using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingCS;
namespace Test_TestingCs
{
    /// <summary>
    /// Summary description for TestFib
    /// </summary>
    [TestClass]
    public class TestFib
    {
        int[] input;
        int[] expectedOutput;
        public TestFib()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            input = new int[] { -5, 0, 1,2,3, 10, 40 };
            expectedOutput = new int[] { -1, 0, 1,1,2, 55, 102334155 };
        }
        //
        // Use TestCleanup to run code after each test has run
         [TestCleanup()]
         public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethodFib()
         {
             for (int i = 0; i < input.Length; i++)
             {
                 Assert.AreEqual(expectedOutput[i],Fib.fib(input[i]));
             }
            
        }
        [TestMethod]
        public void TestMethodFibDP()
        {
            for (int i = 0; i < input.Length; i++)
            {
                Assert.AreEqual(expectedOutput[i], Fib.fibDP(input[i]));
            }
            Assert.AreEqual(3736710778780434371, Fib.fibDP(100));
        }
    }
}
