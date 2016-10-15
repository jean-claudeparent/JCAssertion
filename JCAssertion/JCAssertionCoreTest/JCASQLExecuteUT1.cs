using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCASQLExecuteUT1
    {
        JCACore monCore = new JCACore();
        XmlDocument moXML = new XmlDocument();

            
        [TestInitialize]
        public void InitTest()
        {
            // remplir les variables
            monCore.Variables.MAJVariable("NomTable","JCATest");
            monCore.Variables.MAJVariable("Egal", "­=");
            monCore.Variables.MAJVariable("Colonne", "INFO");
            monCore.Variables.MAJVariable("cleCas", "UT1SEXML1");
             
        }
        /// <summary>
        /// SQLExecutexml Test ;e SQ:Execute avec l'interface XML
        /// </summary>
        [TestMethod]
        public void SQLExecutexml()
        {
            // Initialiser la connection
            moXML = JCAssertionCoreTest.SQLHelper.XMLConnection 
                ("JCA","JCA","");
            Assert.IsTrue (monCore.ExecuteCas(moXML),
                "L'éxécution de ConnectionOracle n'a pas marché " +
                monCore.Message  );
  
            // Cas sans balise SQL
            String[] vide = new String[0];
            moXML = JCAssertionCoreTest.SQLHelper.XMLSQLExecute(vide,"");
            Assert.IsFalse(monCore.ExecuteCas(moXML),
                "L'exécution d'un SQLExecute sans balise SQL aurait du donner faux");
            Assert.IsTrue(monCore.Message.Contains(
                "Le XML ne contient pas la balise SQL"),
                "Message innatendu : " + monCore.Message);
  
            // Cas avec 5 commandes sdql dont une change 0,1 ou plus de 1 tangées
            String[] cinqSQL = new String[5];
            cinqSQL[0] = "";
            cinqSQL[1] = "";
            cinqSQL[2] = "";
            cinqSQL[3] = "";
            cinqSQL[4] = "";
            
 

            Assert.Fail("Pas encore implémentée");
        }
    }
}
