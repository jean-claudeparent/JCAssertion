using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;

namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCAVariableUnitTest1
    {
        [TestMethod]
        public void SubstituerVariable()
        {
            JCACore monJCACore = new JCACore();
            monJCACore.MAJVariable("Test2","Ceci estklavaleursubstituée de test 2");
            monJCACore.MAJVariable("Test1", "Ceci estklavaleursubstituée de test 1");
            monJCACore.MAJVariable("Test", "Ceci estklavaleursubstituée");
            Assert.AreEqual("Aucune variable",JCAVariable.SubstituerVariables("Aucune variable",monJCACore.GetDictionnaireVariable()));
            //Assert.AreEqual("Début:Ceci estklavaleursubstituée:Fin", JCAVariable.SubstituerVariables("", monJCACore.GetDictionnaireVariable));


        }
    }
}
