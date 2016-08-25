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
        public void ClientOK()
        {
            // Préparer les variables de test
            JCAVariable mesVariables = new JCAVariable();
            mesVariables.MAJVariable("monUser","JCA");
            mesVariables.MAJVariable("monPassword", "JCA");
            mesVariables.MAJVariable("monServeur", "localhost");

            JCACore monCore = new JCACore();
            monCore.Variables = mesVariables;

            // un cas sans action mais avec les 3 param`tres
            XmlDocument monCas = new XmlDocument();

            monCas.InnerXml = "<Assertion><Type>ConnectionOracle</Type>" +
                "<User>{{monUser}}</User>" +
                "<Password>{{monPassword}}</Password>" +
                "<Serveur>{{monServeur}}</Serveur>" +
                "</Assertion>";

            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "L'éxécution du cas a échoué : " +
                monCore.Message + Environment.NewLine +
                monCore.MessageEchec);
            // code les test d'action
            Assert.Fail("Pas ebcire implémenté"); 
            



  
            
        }

        [TestMethod]
        public void ClientExcep()
        {
            // user pas spécifié
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();

            monCas.InnerXml = "<Assertion><Type>ConnectionOracle</Type>" +
                "<Password>{{monPassword}}</Password>" +
                "<Serveur>{{monServeur}}</Serveur>" +
                "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Cas User non fourni aurait du retourner False");
            Assert.IsTrue(monCore.Message.Contains(
                "Le XML ne contient pas la balise User"),
                "Cas User non fourni: Le message me contient pas le texte attendu " +
                monCore.Message  );

             
            // password non fourni
            monCas.InnerXml = "<Assertion><Type>ConnectionOracle</Type>" +
                "<User>{{monPassword}}</User>" +
                "<Serveur>{{monServeur}}</Serveur>" +
                "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Le cas Password non fourni aurait du retourner false ");
            Assert.IsTrue(monCore.Message.Contains(
                "Le XML ne contient pas la balise Password"),
                "Cas Password non fourni : Le message me contient pas le texte attendu " +
                monCore.Message);
            

            // pas de variables fournies
            monCas.InnerXml = "<Assertion><Type>ConnectionOracle</Type>" +
                "<User>{{monPassword}}</User>" +
                "<Password>{{monPassword}}</Password>" +
                "<Serveur>{{monServeur}}</Serveur>" +
                "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Cas variables non fournmies aurait du retourner false");
            Assert.IsTrue(monCore.MessageEchec.Contains(
                "La variable monPassword n'a pas eu de valeur fournie"),
                "Cas variables non fournieLe message me contient pas le texte attendu " +
                monCore.MessageEchec);
            
            // Action Ouvrir qui provoqie une exception oracle
            monCas.InnerXml = "<Assertion><Type>ConnectionOracle</Type>" +
                "<User>JCA</User>" +
                "<Password>JCA</Password>" +
                "<Action>Ouvrir</Action>" +
                "<Serveur>ServeurInexistant</Serveur>" +
                "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Le cas action Ouvrir invalide  aurair du retourner false");
            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Le XML ne contient pas la balise Password"),
                "Cas action ouvrir invalide  : Le message me contient pas le texte attendu " +
                monCore.MessageEchec);
            
             

            
        }

    }
}
