using Mechanect.Exp2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;
using System;
using System.Windows;



namespace TestProjec
{


    /// <summary>
    ///This is a test class for Environment2Test and is intended
    ///to contain all Environment2Test Unit Tests
    ///</summary>

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
        ///<remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 20 </para>
        /// <para>DATE MODIFIED: May, 22 </para>
        /// </remarks>
        [Test]
        public void PositionMapperTest()
        {

            Environment2 target = new Environment2(Vector2.Zero, new Rect(5, 5, 1, 1), new Rect(10, 3, 2, 2)); 
            target.Draw(new Rectangle(10, 10, 500, 300),
                new Microsoft.Xna.Framework.Graphics.SpriteBatch(new GraphicsDevice(GraphicsAdapter.DefaultAdapter,
                new GraphicsProfile(), new PresentationParameters())));
            Vector2[] unMapped = { new Vector2(-1, -1), new Vector2(0, 0), new Vector2(1, 1), new Vector2(-1, 1), 
                                     new Vector2(10, 10) };
            Vector2[] expected = { new Vector2(174.2f,193.8f), new Vector2(145, 223), new Vector2(174.2f,193.8f),
                                     new Vector2(174.2f,193.8f), new Vector2(311,57) };
            
            Vector2 actual;
            for (int i = 0; i < unMapped.Length; i++)
            {

                actual = target.PositionMapper(unMapped[i]);
                Assert.AreEqual(expected[i], actual);
            }
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
