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
            // rendu ici
            monJCACore.Variables.MAJVariable("Test2","Ceci estklavaleursubstituée de test 2");
            monJCACore.Variables.MAJVariable("Test1", "Ceci estklavaleursubstituée de test 1");
            monJCACore.Variables.MAJVariable("Test", "Ceci estklavaleursubstituée");
            
            Assert.AreEqual("Aucune variable",JCAVariable.SubstituerVariables("Aucune variable",monJCACore.Variables.GetDictionnaireVariable()));
            Assert.AreEqual("Début:Ceci estklavaleursubstituée:Fin", JCAVariable.SubstituerVariables("Début:{{Test}}:Fin", monJCACore.Variables.GetDictionnaireVariable()));
            Assert.AreEqual("Ceci estklavaleursubstituée de test 2Début:Ceci estklavaleursubstituée:FinCeci estklavaleursubstituéeCeci estklavaleursubstituée de test 1", JCAVariable.SubstituerVariables("{{Test2}}Début:{{Test}}:Fin{{Test}}{{Test1}}", monJCACore.Variables.GetDictionnaireVariable()));

            try  {
                Assert.AreEqual("Exception attendue", JCAVariable.SubstituerVariables("Début:{{TestVarexistepas}}:Fin", monJCACore.Variables.Variables  ));
            } catch (JCAssertionException excep) {
                Assert.IsTrue(excep.Message.Contains("pas eu de valeur fournie"), "Le message d'exception devraitcontenir 'pas eu de valeur fournie'. Mais est " + excep.Message);
            }

        }
    }
}
