using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml;



namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCAODPSQLClientUT1
    {
        [TestMethod]
        public void SQLCClientonnectsansAction()
        {

            JCASQLClient monClient = new JCASQLClient();

            // Au début la connection doit être fermée
            Assert.IsFalse(monClient.ConnectionOuverte(),
                "Avant de commencer la connection devrait être fermée");

            monClient.InitConnection("JCA", "JCA");

            Assert.IsFalse(monClient.ConnectionOuverte(),
                "Après la connection devrait être encore fermée");

            // Test de multi dispose
            monClient.Dispose();
            monClient.Dispose();
            
        }


        [TestMethod]
        public void SQLCClientonnectAvecAction()
        {

            JCASQLClient monClient = new JCASQLClient();

            // Au début la connection doit être fermée
            Assert.IsFalse(monClient.ConnectionOuverte(),
                "Avant de commencer la connection devrait être fermée");

            monClient.InitConnection(
                "JCA", "JCA",
                "localhost",
                JCASQLClient.Action.Ouvrir);

            Assert.IsTrue(monClient.ConnectionOuverte(),
                "Après la connection devrait être ouvertee");
            // Test de fermeture
            monClient.InitConnection(
                "JCA", "JCA",
                "localhost",
                JCASQLClient.Action.Fermer );

            Assert.IsFalse(monClient.ConnectionOuverte(),
                "Maintenant la connection devrait être fermée");


            // Test de multi dispose
            monClient.Dispose();
            monClient.Dispose();

        }



        
        [TestMethod]
        public void ClientExcepUser()
        {
            // user pas spécifié
            JCASQLClient monClient = new JCASQLClient();
            try
            {
                monClient.InitConnection(
                null,
                "JCA",
                "!!!!",
                JCASQLClient.Action.Ouvrir);
                Assert.Fail("Une exception aurait dû se produire"); 
            }
            catch (Exception excep)
            {
                Assert.IsTrue(
                    excep.Message.Contains(
                        "Pour une connection à la base de données le user est obligatoire"),
                    excep.Message); 
            }
            // Test de dispose
            monClient.Dispose();
            monClient.Dispose();




        }

        [TestMethod]
        public void ClientExcepPassword()
        {
            // user pas spécifié
            JCASQLClient monClient = new JCASQLClient();
            try
            {
                monClient.InitConnection(
                "JCA",
                null,
                "!!!!",
                JCASQLClient.Action.Ouvrir);
                Assert.Fail("Une exception aurait dû se produire");
            }
            catch (Exception excep)
            {
                Assert.IsTrue(
                    excep.Message.Contains(
                        "Pour une connection à la base de données le mot de passe est obligatoire"),
                    excep.Message);
            }
            // Test de dispose
            monClient.Dispose();
            monClient.Dispose();




        }

        [TestMethod]
        public void ClientExcepOracle()
        {
            // user pas spécifié
            JCASQLClient monClient = new JCASQLClient();
            try
            {
                monClient.InitConnection(
                "Aucunaccès",
                "Aucunaccès",
                null,
                JCASQLClient.Action.Ouvrir);
                Assert.Fail("Une exception aurait dû se produire");
            }
            catch (Exception excep)
            {
                Assert.IsTrue(
                    excep.Message.Contains(
                        "ORA-"),
                    excep.Message);
            }
            // Test de dispose
            monClient.Dispose();
            monClient.Dispose();




        }




    }
}
