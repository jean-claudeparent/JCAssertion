using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;



namespace JCAssertionCoreTest
{
    [TestClass]
    public class CoreUnitTest
    {
        [TestMethod]
        public void JCACoreLoadOK()
        {
            // test sans fichier de valeurs

            JCACore monJCACore = new JCACore();
            monJCACore.Load(JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            
            Assert.AreEqual(2, monJCACore.NombreCas , "Le fichier chargé devrait contenir 2 cas de test, réel = " + monJCACore.NombreCas.ToString());
            
            Assert.IsTrue(monJCACore.FichierDeCas.Contains("Ressources\\FichierDeCasOK.xml"));
            Assert.IsNull(monJCACore.FichierValeur);
            Assert.IsNotNull(monJCACore.FichierJournal);
            Assert.AreEqual(1,monJCACore.NoCasCourant);
            Assert.IsTrue(monJCACore.Message.Contains ("réussi"),"Message erroné " + monJCACore.Message );
            
            // test pour les méthode de la liste desvariables

            Assert.AreEqual(0, monJCACore.NombreVariables() );
            monJCACore.MAJVariable("Fichier","Aucun");
            monJCACore.MAJVariable("Fichier", JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            Assert.AreEqual(1, monJCACore.NombreVariables());
            Assert.IsTrue(monJCACore.GetValeurVariable("Fichier").Contains("Ressources\\FichierDeCasOK.xml"));
            Assert.IsNull(monJCACore.GetValeurVariable("CleAbsente"));


            // Test plus étendu avec fichier de valeur
            //Assert.Fail("Oas encore implémenté.");

            
        }

        [TestMethod]
        public void JCACoreVariables()
        {
            
            JCACore monJCACore = new JCACore();
            
            // test pour les méthode de la liste desvariables

            Assert.AreEqual(0, monJCACore.NombreVariables());
            monJCACore.MAJVariable("Fichier", "Aucun");
            monJCACore.MAJVariable("Fichier", JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            Assert.AreEqual(1, monJCACore.NombreVariables());
            Assert.IsTrue(monJCACore.GetValeurVariable("Fichier").Contains("Ressources\\FichierDeCasOK.xml"));
            Assert.IsNull(monJCACore.GetValeurVariable("CleAbsente"));
        }

        [TestMethod]
        public void JCACoreExecuterCasOK()
        {
            // test sans fichier de valeurs

            JCACore monJCACore = new JCACore();
            monJCACore.Load(JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");

            Assert.AreEqual(2, monJCACore.NombreCas, "Le fichier chargé devrait contenir 2 cas de test, réel = " + monJCACore.NombreCas.ToString());

            Assert.IsNotNull(monJCACore.FichierJournal);
            
            // Initialiser les méthode de la liste desvariables

            monJCACore.MAJVariable("Fichier", JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            

            // Test plus étendu avec fichier de valeur
            Assert.IsFalse(monJCACore.ExecuteCas(0), "Cas hors limite plus petit que 1");
            Assert.IsTrue (monJCACore.ExecuteCas (1) ,"Test de ichier qui existe = true");
            Assert.IsFalse(monJCACore.ExecuteCas(2), "Test de ichier qui existe = false");
            Assert.IsFalse(monJCACore.ExecuteCas(133564),"Cas hors limite dépasse le nombre maximum");


        }

    }
}
