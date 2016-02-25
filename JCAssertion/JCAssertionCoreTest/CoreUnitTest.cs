using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml;




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
            Assert.IsNotNull(monJCACore.FichierJournal,"Le nom du fichier de journal par défaut aurait du être assigné");
            Assert.AreEqual(0,monJCACore.NoCasCourant);
            //Assert.IsTrue(monJCACore.Message.Contains ("réussi"),"Message erroné " + monJCACore.Message );
            
            // test pour les méthode de la liste desvariables

            Assert.AreEqual(0, monJCACore.NombreVariables() ,"Au début le nombre de variables devrait être 0");
            monJCACore.MAJVariable("Fichier","Aucun");
            monJCACore.MAJVariable("Fichier", JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            Assert.AreEqual(1, monJCACore.NombreVariables(),"Il devrait y avoir maintenant 1 variable");
            Assert.IsTrue(monJCACore.GetValeurVariable("Fichier").Contains("Ressources\\FichierDeCasOK.xml"),"La variable devrait contenir le chemin du fichier");
            Assert.IsNull(monJCACore.GetValeurVariable("CleAbsente"),"Les variables absentes devraient retourner null");


            // Test plus étendu avec fichier de valeur
            //Assert.Fail("Oas encore implémenté.");

            
        }

        [TestMethod]
        public void JCACoreVariables()
        {
            
            JCACore monJCACore = new JCACore();
            
            // test pour les méthode de la liste desvariables

            Assert.AreEqual(0, monJCACore.NombreVariables(),"Au début lenombre de variables devrait être 0");
            monJCACore.MAJVariable("Fichier", "Aucun");
            monJCACore.MAJVariable("Fichier", JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            Assert.AreEqual(1, monJCACore.NombreVariables(),"Maintenant le nombre de vraiables devrait être 1");
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

            Assert.IsNotNull(monJCACore.FichierJournal,"Un nom de fichier journalpar défaut aurait du être assigné");
            
            // Initialiser les méthode de la liste desvariables

            monJCACore.MAJVariable("Fichier", JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            

            // Test plus étendu avec fichier de valeur
            


            Assert.IsTrue(monJCACore.ExecuteCas(monJCACore.getListeDeCas().Item(0)),"Test pour un fichier qui existe");
            
            Assert.IsFalse (monJCACore.ExecuteCas(monJCACore.getListeDeCas().Item(1)) ,"Test de ichier qui n'existe pas = donc retourne false");
            

        }

    }
}
