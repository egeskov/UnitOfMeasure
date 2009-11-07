using HEX.UnitOfMeasure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestProject
{


    [TestClass()]
    public class LengthTest
    {


        private TestContext testContextInstance;

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


        [TestMethod()]
        public void op_AdditionTest()
        {
            Length lhs = new Length(1.25, LengthUnit.Meter);
            Length rhs = new Length(10, LengthUnit.CentiMeter);
            Length expected = new Length(1.35, LengthUnit.Meter);
            Length actual = (lhs + rhs);
            Assert.AreEqual(expected, actual, "Error testing " + lhs + " + " + rhs + "!");
        }
    }
}
