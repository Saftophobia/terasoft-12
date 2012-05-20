using Mechanect.Exp2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Xna.Framework;


namespace TestMechanect
{
    
    
    /// <summary>
    ///This is a test class for Environment2Test and is intended
    ///to contain all Environment2Test Unit Tests
    ///</summary>
    [TestClass()]
    public class Environment2Test
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for PositionMapper
        ///</summary>
        [TestMethod()]
        public void PositionMapperTest()
        {
            Vector2 position = new Vector2(); // TODO: Initialize to an appropriate value
            Rectangle prey = new Rectangle(); // TODO: Initialize to an appropriate value
            Rectangle aquriaum = new Rectangle(); // TODO: Initialize to an appropriate value
            Environment2 target = new Environment2(position, prey, aquriaum); // TODO: Initialize to an appropriate value
            Vector2 unMappedPosition = new Vector2(); // TODO: Initialize to an appropriate value
            Vector2 expected = new Vector2(); // TODO: Initialize to an appropriate value
            Vector2 actual;
            actual = target.PositionMapper(unMappedPosition);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
