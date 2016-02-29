﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.IO;

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

        [TestMethod]
        public void EcrireEtLire()
        {
            JCAVariable mesVariablesAvant = new JCAVariable();
            JCAVariable mesVariablesApres = new JCAVariable();
            String NomFichier = JCACore.RepertoireAssembly() + "Ressources\\EcrireEtLire.xml";
            
            // tester l'état de l'environnement de test
            if (File.Exists(NomFichier)) File.Delete(NomFichier);
            Assert.IsFalse (File.Exists (NomFichier),"Le fichier de sérialisation devrait ne pas exister");

            // remplir les avariables avant ecriture
            mesVariablesAvant.MAJVariable("Test<3>", "Valeur\" de la variable Test3");
            mesVariablesAvant.MAJVariable("Test\"1\"", "Valeur de <la> variable Test1");
            mesVariablesAvant.MAJVariable("AATest2", "Valeur de la variable Test2");
            mesVariablesAvant.EcrireFichier(NomFichier);
            String Contenu = System.IO.File.ReadAllText(NomFichier);
            Assert.IsTrue(Contenu.Contains("Test&lt;3&gt;"));
            Assert.IsTrue(Contenu.Contains("Valeur&quot; de la variable Test3"));
            Assert.IsTrue(Contenu.Contains("Test&quot;1&quot;"));
            Assert.IsTrue(Contenu.Contains("Valeur de &lt;la&gt; variable Test1"));
            Assert.IsTrue(Contenu.Contains("AATest2"));
            Assert.IsTrue(Contenu.Contains("Valeur de la variable Test2"));

            
            // Créer du contenu qui sera remplacé dans la variable apr
            mesVariablesApres.MAJVariable("errone","Cette valeur devrait disparaitre");

            Assert.AreNotEqual(mesVariablesAvant, mesVariablesApres,"Avant de commencer le test lesvariablesdevraient être différentes");

            
            mesVariablesApres.LireFichier(NomFichier );
            Assert.AreEqual(mesVariablesAvant.Variables , mesVariablesApres.Variables , "Aprè le test les deux objets de variable devraient être pareils");




            
        }
    }
}
