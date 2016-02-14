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
            // test sans fichier de valeurs

            JCACore monJCACore = new JCACore();
            monJCACore.Load(JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            
            Assert.AreEqual(2, monJCACore.NombreCas , "Le fichier chargé devrait contenir 2 cas de test, réel = " + monJCACore.NombreCas.ToString());
            
            Assert.IsTrue(monJCACore.FichierDeCas.Contains("Ressources\\FichierDeCasOK.xml"));
            Assert.IsNull(monJCACore.FichierValeur);
            Assert.IsNotNull(monJCACore.FichierJournal);
            Assert.AreEqual(1,monJCACore.NoCasCourant);
            Assert.IsTrue(monJCACore.Message.Contains ("réussi"), "Message erroné " + monJCACore.Message );
            Assert.AreEqual(0, monJCACore.NombreVariables() );
            monJCACore.MAJVariable("Fichier","Aucun");
            monJCACore.MAJVariable("Fichier", JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            Assert.AreEqual(1, monJCACore.NombreVariables());


            // Test plus étendu avec fichier de valeur
            //Assert.Fail("Oas encore implémenté.");

            
        }
    }
}
