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
            monCore.Variables.MAJVariable(
                "dual","select count(*) from dual");
            monCore.Variables.MAJVariable(
                "un","1");
            monCore.Variables.MAJVariable(
                "echec", "Ceci est le message d'échec de test.");
            

        }

        [TestCleanup]
        public void CleanTest()
        {
            // FermerConnection

        }
    


        [TestMethod]
        public void ODPSQLAssertOK()
        {
            // Cas 1 Assert un nombre et retourne true
            monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<SQL>{{dual}}</SQL>" +
               "<AttenduNombre>{{un}}</AttenduNombre>" +
               "</Assertion>";
            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "Échec du cas 1 de de ODPSQLAssertOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec );
            Assert.IsTrue(monCore.Message.Contains(
                "select count(*) from dual"), "Le message devrait contenir select count(*) from dual");
            Assert.IsTrue(monCore.Message.Contains(
                "Opérateur : ="),
                "Le message devrait contenir Opérateur : =");
            Assert.IsTrue(monCore.Message.Contains(
                "Valeur attendue : 1"),
                "Le message devrait contenir Valeur attendue : 1");


            // Cas 2 Assert un nombre et retourne false
            monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<SQL>{{dual}}</SQL>" +
                "<Operateur>pg</Operateur>" +
               "<AttenduNombre>{{un}}</AttenduNombre>" +
               "<MessageEchec>{{echec}}</MessageEchec>" +
               "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Échec du cas 2 de de ODPSQLAssertOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec);

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Ceci est le message d'échec de test."),
                "Le message d'échec aurait du contenir :" 
                + " Ceci est le message d'échec de test. Contient : " +
                monCore.MessageEchec  );  

            Assert.Fail ("Pas encore implémenté " + 
                monCore.Message ); 
        }
    }
}
