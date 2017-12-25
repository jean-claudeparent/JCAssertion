using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using JCAssertionCore;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class ConnectionOracleUT1
    {
        /// <summary>
        /// Test de l'asserion ConnectionOracle sans
        /// action ce qui ne connecte pas
        /// immédiatement `la BD.
        /// </summary>
        [TestMethod]
        public void OuvrirSansActionConnectionOracle()
        {
            // Définir connection
            XmlDocument monCas = new XmlDocument();
            monCas.InnerXml = "<Assertion>" +
               "<Type>ConnectionOracle</Type>" +
               "<User>JCA</User>" +
               "<Password>JCA</Password>" +
               "<Serveur>instance.test</Serveur>" +
               "</Assertion>";

            JCACore monCore = new JCACore();
            monCore.ExecuteCas(monCas);

            // assertions

            Assert.IsTrue(monCore.Message.Contains(
                "User : JCA"),
                "Le message devrait contenir User : JCA");
            Assert.IsTrue(monCore.Message.Contains(
                "Password : JCA"),
                "Le message devrait contenir Password : JCA");
            Assert.IsTrue(monCore.Message.Contains(
                "Serveur/instance : instance.test"),
                "Le message devrait contenir Serveur/instance : instance.test");

            Assert.IsTrue(monCore.Message.Contains(
                "Assertion ConnectionOracle"),
                "Le message devrait contenir Assertion ConnectionOracle");




            Assert.IsFalse(
                monCore.ODPSQLConnectionOuverte(),
                "Pour une assertion ConnectionOracle sans action la bd n'est pas vraiment ouverte");

            
        }

        /// <summary>
        /// Test de l'assertion ConnectionOracle avec
        /// action ce qui connecte 
        /// immédiatement `la BD.
        /// </summary>
        [TestMethod]
        public void OuvrirAvecActionConnectionOracle()
        {
            JCACore monCore = new JCACore();
            JCAXMLHelper monxmlH = new JCAXMLHelper();
            
            // Définir connection
            XmlDocument monCas = new XmlDocument();
            monCas.InnerXml = monxmlH.xmlConnectionOracle (
                "JCA",
                "JCA",
                "localhost",
                "Ouvrir");
            

            monCore.ExecuteCas(monCas);

            // assertions

            Assert.IsTrue(monCore.Message.Contains(
                "User : JCA"),
                "Le message devrait contenir User : JCA");
            Assert.IsTrue(monCore.Message.Contains(
                "Password : JCA"),
                "Le message devrait contenir Password : JCA");
            Assert.IsTrue(monCore.Message.Contains(
                "Serveur/instance : localhost"),
                "Le message devrait contenir Serveur/instance : localhost");

            Assert.IsTrue(monCore.Message.Contains(
                "Assertion ConnectionOracle"),
                "Le message devrait contenir Assertion ConnectionOracle");




            Assert.IsTrue(
                monCore.ODPSQLConnectionOuverte(),
                "Le connection devrai être ouverte");
            // Fermer la connection

            monCas.InnerXml = monxmlH.xmlConnectionOracle(
                "JCA2",
                "JCA2",
                "localhost2",
                "Fermer");


            monCore.ExecuteCas(monCas);

            Assert.IsFalse(
                monCore.ODPSQLConnectionOuverte(),
                "La connection devrait êtrefermée");


        }



    }
}
