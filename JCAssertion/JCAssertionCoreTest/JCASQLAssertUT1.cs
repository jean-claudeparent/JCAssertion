using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml;

namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCASQLAssertUT1
    {
        XmlDocument monCas;
        JCACore monCore;

        [TestInitialize]
        public void InitTest()
        {
            // Définir connection
            monCas = new XmlDocument();
            monCas.InnerXml = "<Assertion>" +
               "<Type>ConnectionOracle</Type>" + 
               "<User>JCA</User>" + 
               "<Password>JCA</Password>" +
               "</Assertion>"; 
 
            monCore = new JCACore();
            monCore.ExecuteCas(monCas); 
             
            // monSQLClient.User = "JCA";
            // monSQLClient.Password = "JCA";
            // monSQLClient.OuvrirConnection();
            // monSQLClient.ActiverResume = true;

        }

        [TestCleanup]
        public void CleanTest()
        {
            // FermerConnection

        }
    


        [TestMethod]
        public void ODPSQLAssertOK()
        {
            // Assert un nombre et retourne true
            monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<SQL>{{dual}}</SQL>" +
               "<AttenduNombre>{{un}}</AttenduNombre>" +
               "</Assertion>";
            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "Échec du cas 1 de de ODPSQLAssertOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec ); 
            
            Assert.Fail ("Pas encore implémenté"); 
        }
    }
}
