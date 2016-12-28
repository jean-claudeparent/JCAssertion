using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml;
using System.IO;




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
            
            Assert.AreEqual(3, monJCACore.NombreCas , "Le fichier chargé devrait contenir 2 cas de test, réel = " + monJCACore.NombreCas.ToString());
            
            Assert.IsTrue(monJCACore.FichierDeCas.Contains("Ressources\\FichierDeCasOK.xml"));
            Assert.IsNull(monJCACore.FichierValeur);
            Assert.IsNotNull(monJCACore.FichierJournal,
                "Le nom du fichier de journal par défaut aurait du être assigné");
            // Tester les inscriptions au journal lors du load

            // Vérifier que la version et la date est mise dans le journal
            String monContenuJournal =
                File.ReadAllText(monJCACore.FichierJournal);
            Assert.IsTrue(monContenuJournal.Contains(
                "Essai lancé avec JCAssertion version 1."),
                "La version aurait dû être dans le journal " +
                monContenuJournal);
            Assert.IsTrue(monContenuJournal.Contains(
                "Date et heure de l'essai : 2"),
                "La date et heure  aurait dû être dans le journal " +
                monContenuJournal); 
            

            // aprs test journal
            Assert.AreEqual(0,monJCACore.NoCasCourant);
            //Assert.IsTrue(monJCACore.Message.Contains ("réussi"),"Message erroné " + monJCACore.Message );
            
            // test pour les méthode de la liste desvariables

            Assert.AreEqual(0, monJCACore.Variables.NombreVariables() ,"Au début le nombre de variables devrait être 0");
            monJCACore.Variables.MAJVariable("Fichier", "Aucun");
            monJCACore.Variables.MAJVariable("Fichier", JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            Assert.AreEqual(1, monJCACore.Variables.NombreVariables(),"Il devrait y avoir maintenant 1 variable");
            Assert.IsTrue(monJCACore.Variables.GetValeurVariable("Fichier").Contains("Ressources\\FichierDeCasOK.xml"),"La variable devrait contenir le chemin du fichier");
            Assert.IsNull(monJCACore.Variables.GetValeurVariable("CleAbsente"),"Les variables absentes devraient retourner null");


            // Test plus étendu avec fichier de valeur
            //Assert.Fail("Oas encore implémenté.");

            
        }

        [TestMethod]
        public void JCACoreVariables()
        {
            
            JCACore monJCACore = new JCACore();
            
            // test pour les méthode de la liste desvariables

            
            Assert.AreEqual(0, monJCACore.Variables.NombreVariables(),"Au début lenombre de variables devrait être 0");
            monJCACore.Variables.MAJVariable("Fichier", "Aucun");
            monJCACore.Variables.MAJVariable("Fichier", JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            Assert.AreEqual(1, monJCACore.Variables.NombreVariables(),"Maintenant le nombre de vraiables devrait être 1");
            Assert.IsTrue(monJCACore.Variables.GetValeurVariable("Fichier").Contains("Ressources\\FichierDeCasOK.xml"));
            Assert.IsNull(monJCACore.Variables.GetValeurVariable("CleAbsente"));
        }

        [TestMethod]
        public void JCACoreExecuterCasOK()
        {
            // test sans fichier de valeurs

            JCACore monJCACore = new JCACore();
            monJCACore.Load(JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");

            Assert.AreEqual(3, monJCACore.NombreCas, "Le fichier chargé devrait contenir 2 cas de test, réel = " + monJCACore.NombreCas.ToString());

            Assert.IsNotNull(monJCACore.FichierJournal,"Un nom de fichier journalpar défaut aurait du être assigné");
            
            // Initialiser les méthode de la liste desvariables

            monJCACore.Variables.MAJVariable("Fichier", JCACore.RepertoireAssembly() + "Ressources\\FichierDeCasOK.xml");
            

            // Test plus étendu avec fichier de valeur
            


            Assert.IsTrue(monJCACore.ExecuteCas(monJCACore.getListeDeCas().Item(0)),"Test pour un fichier qui existe");
            
            Assert.IsFalse (monJCACore.ExecuteCas(monJCACore.getListeDeCas().Item(1)) ,"Test de ichier qui n'existe pas = donc retourne false");
            Assert.IsFalse(monJCACore.ExecuteCas(monJCACore.getListeDeCas().Item(2)), "une exception retourne false");
            

        }

    }
}
