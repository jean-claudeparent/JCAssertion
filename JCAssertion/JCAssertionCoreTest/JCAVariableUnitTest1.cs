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
            Assert.AreEqual("Début:Ceci estklavaleursubstituée:Fin", JCAVariable.SubstituerVariables("Début:{{Test}}:Fin", monJCACore.GetDictionnaireVariable()));
            Assert.AreEqual("Ceci estklavaleursubstituée de test 2Début:Ceci estklavaleursubstituée:FinCeci estklavaleursubstituéeCeci estklavaleursubstituée de test 1", JCAVariable.SubstituerVariables("{{Test2}}Début:{{Test}}:Fin{{Test}}{{Test1}}", monJCACore.GetDictionnaireVariable()));

            try  {
                Assert.AreEqual("Exception attendue", JCAVariable.SubstituerVariables("Début:{{TestVarexistepas}}:Fin", monJCACore.GetDictionnaireVariable()));
            } catch (JCAssertionException excep) {
                Assert.IsTrue(excep.Message.Contains("pas fournie"),"Il devrait y avoir un exception si la variable n'est pas fournie." );
            }

        }
    }
}
