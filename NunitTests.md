# Unit Tests Tutorial #

I explained how to install Nunit framework and get it to work with VS and how to run the tests previously. Now, i'm going to explain, briefly, how to create the tests.

Now go to branches and open UnitTestsTutorial. The project consists of 2 classes, Bla.cs and TestClass.cs. Now open TestClass.cs
[TestFixtureSetUp](TestFixtureSetUp.md) before method Init() marks this method as the setup method for this test class. Meaning, before any tests are run this method will be automatically called, so you should put here all the things that might be common among all your tests.
[TestFixtureTearDown](TestFixtureTearDown.md) before method Dispose marks this method as the TearDown method for this class. Meaning, after any tests are run this method will be automatically called, so you should put here all the unloads and such stuff.
Assert.AreEqual(e1,e2) compares e1 and e2 by value
Assert.AreSame(o1,o2) compares o1 and o2 by reference
Let's look at a code flow for some normal test class

TestFixtureSetUpMethod();
Test1();
TestFixtureTearDownMethod();

Now read the 2 classes, they should be easy to understand and run the tests(it calculates factorial of 5 or 6 randomly).