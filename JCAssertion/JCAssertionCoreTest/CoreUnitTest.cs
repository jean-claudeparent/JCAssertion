using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;



namespace JCAssertionCoreTest
{
    [TestClass]
    public class CoreUnitTest
    {
        [TestMethod]
        public void CoreLoadOK()
        {
            // test limité sans fichier de valeurs

            Core monJCACore = new Core();
            monJCACore.Load(Core.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            // Assert.AreEqual("rien", monJCACore.Message );
            
            Assert.AreEqual(2, monJCACore.NombreCas , "Le fichier chargé devrait contenir 2 cas de test, réel = " + monJCACore.NombreCas.ToString());
            
            Assert.IsTrue(monJCACore.FichierDeCas.Contains("Ressources\\FichierDeCasOK.xml"));
            Assert.IsNull(monJCACore.FichierValeur);
            Assert.IsNotNull(monJCACore.FichierJournal);
 
            // Test plus étendu avec fichier de valeur
            Assert.Fail("Oas encore implémenté.");

            
        }
    }
}
