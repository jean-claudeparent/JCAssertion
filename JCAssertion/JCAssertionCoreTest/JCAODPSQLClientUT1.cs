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
            try 
            {
                monCore.ExecuteCas(monCas);
                Assert.Fail ("Une exception aurait du se produire"); 
            } catch (JCAssertionException excep)
            {
                Assert.IsTrue(excep.Message.Contains("ccc"),
                    "Mauvais libellé d'exception : " + excep.Message); 
            }

            // password non fourni
            monCas.InnerXml = "<Assertion><Type>ConnectionOracle</Type>" +
                "<User>{{monPassword}}</User>" +
                "<Serveur>{{monServeur}}</Serveur>" +
                "</Assertion>";
            try
            {
                monCore.ExecuteCas(monCas);
                Assert.Fail("Une exception aurait du se produire");
            }
            catch (JCAssertionException excep)
            {
                Assert.IsTrue(excep.Message.Contains("ccc"),
                    "Mauvais libellé d'exception : " + excep.Message);
            }

            // pas de variables fournies
            monCas.InnerXml = "<Assertion><Type>ConnectionOracle</Type>" +
                "<User>{{monPassword}}</User>" +
                "<Password>{{monPassword}}</Password>" +
                "<Serveur>{{monServeur}}</Serveur>" +
                "</Assertion>";
            try
            {
                monCore.ExecuteCas(monCas);
                Assert.Fail("Une exception aurait du se produire");
            }
            catch (JCAssertionException excep)
            {
                Assert.IsTrue(excep.Message.Contains("ccc"),
                    "Mauvais libellé d'exception : " + excep.Message);
            }

            // xml invalide
            monCas.InnerXml = "<Assertion><Type>ConnectionOracle</Type>" +
                "<User>{{monPassword}}" +
                "<Password>{{monPassword}}</Password>" +
                "<Serveur>{{monServeur}}</Serveur>" +
                "</Assertion>";
            try
            {
                monCore.ExecuteCas(monCas);
                Assert.Fail("Une exception aurait du se produire");
            }
            catch (JCAssertionException excep)
            {
                Assert.IsTrue(excep.Message.Contains("ccc"),
                    "Mauvais libellé d'exception : " + excep.Message);
            }

            
        }

    }
}
