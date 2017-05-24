using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAMC;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCAMiniCoreUT1
    {

        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";

        [TestMethod]
        public void CompteFichiers()
        {
            // Test de lire tous les fichiers
            Assert.AreEqual(5,
                JCAMiniCore.CompteFichiers(Chemin + "CompteFichiers")); 
            // Test pour un répertoire inexistant
            Assert.AreEqual(0,
                JCAMiniCore.CompteFichiers(Chemin + "CompteFichiersrepertoireinexistant"));

            // Test de lire une extension
            // avec un cas en majuscules et un en minuscules
            Assert.AreEqual(2,
                JCAMiniCore.CompteFichiers(Chemin + "CompteFichiers","*.xml"));

            // un cas pour 0 fichiers
            Assert.AreEqual(0,
                JCAMiniCore.CompteFichiers(Chemin + "CompteFichiers", 
                "*.xpaslaml"));

            

            // Un répertoire invalide
            Assert.AreEqual(0,
                JCAMiniCore.CompteFichiers(Chemin + "CompteFichi::::ers",
                "*.xpaslaml"));

            
        }
    }
}
