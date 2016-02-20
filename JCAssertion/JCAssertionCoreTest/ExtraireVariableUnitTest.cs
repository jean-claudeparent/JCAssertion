using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class ExtraireVariableUnitTest
    {
        [TestMethod]
        public void ExtraireVariable()
        {
            Assert.AreEqual("Test", JCAVariable.ExtraireVariable("{{Test}}"));
            Assert.AreEqual("", JCAVariable.ExtraireVariable("{{Test"));
            Assert.AreEqual("", JCAVariable.ExtraireVariable("{{Test"));
            Assert.AreEqual("", JCAVariable.ExtraireVariable("Test"));



        }
    }
}
