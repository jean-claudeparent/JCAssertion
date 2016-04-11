using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml;

namespace JCAssertionCoreTest
{
    [TestClass]
    public class ContenuFichierUnitTest1
    {
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() + 
                 "Ressources\\";
        

        [TestMethod]
        public void ContenuFichierMultiple()
        {
            // Variables
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();
            String FichierEssai = Chemin + "ContenuFichierNultiple.txt";


            // Création du fichier de données

            System.IO.File.WriteAllText(FichierEssai, "spool {{SpoolFile}}" +
                Environment.NewLine + "select '{{Logon}}' from dual;" +
                Environment.NewLine);
            
            // Création du xml de cas
            
            monCas.InnerXml = "<Assertion><Type>ContenuFichier</Type>" +
                "<Fichier>{{FichierEssai}}</Fichier>" +
                "<Contient>select</Contient>" +
                "<Contient>from</Contient>" +
                "<Contient>from</Contient>" +
                "<NeContientPas>savana</NeContientPas>" +
                "<NeContientPas>siamois</NeContientPas>" +
                "<NeContientPas>savana</NeContientPas>" +
                "</Assertion>";
            
            
            // définit les variables
            monCore.Variables.MAJVariable("FichierEssai", FichierEssai);
            


            // exécution de l'essai
            Assert.IsTrue(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("savana"),
                "Attendu:savana Réel :" + monCore.Message);
            Assert.IsTrue(monCore.Message.Contains("siamois"),
                "Attendu:siamois Réel :" + monCore.Message);
            Assert.IsTrue(monCore.Message.Contains("from"),
                "Attendu:from Réel :" + monCore.Message);
            Assert.IsTrue(monCore.Message.Contains("select"),
                "Attendu:select Réel :" + monCore.Message);
            
            Assert.AreEqual("", monCore.MessageEchec);



            
        }
    }
}
