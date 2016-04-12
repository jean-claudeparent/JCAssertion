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

        private void CreerFichierTest(String FichierEssai)
            {
                System.IO.File.WriteAllText(FichierEssai, "spool {{SpoolFile}}" +
                    Environment.NewLine + "select '{{Logon}}' from dual;" +
                    Environment.NewLine);
            
            }

        

        [TestMethod]
        public void ContenuFichierMultipleSucces()
        {
            // Variables
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();
            String FichierEssai = Chemin + "ContenuFichierMultipleSucces.txt";


            // Création du fichier de données
            CreerFichierTest(FichierEssai);
            
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


        [TestMethod]
        public void ContenuFichierMultipleEchecContient()
        {
            // Variables
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();
            String FichierEssai = Chemin + "ContenuFichierMultipleEchecContient.txt";


            // Création du fichier de données
            CreerFichierTest(FichierEssai);

            // Création du xml de cas

            monCas.InnerXml = "<Assertion><Type>ContenuFichier</Type>" +
                "<Fichier>{{FichierEssai}}</Fichier>" +
                "<Contient>introuvable</Contient>" +
                "<Contient>from</Contient>" +
                "<Contient>from</Contient>" +
                "<NeContientPas>savana</NeContientPas>" +
                "<NeContientPas>siamois</NeContientPas>" +
                "<NeContientPas>savana</NeContientPas>" +
                "</Assertion>";


            // définit les variables
            monCore.Variables.MAJVariable("FichierEssai", FichierEssai);



            // exécution de l'essai
            Assert.IsFalse (monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("savana"),
                "Attendu:savana Réel :" + monCore.Message);
            Assert.IsTrue(monCore.Message.Contains("siamois"),
                "Attendu:siamois Réel :" + monCore.Message);
            Assert.IsTrue(monCore.Message.Contains("from"),
                "Attendu:from Réel :" + monCore.Message);
            Assert.IsTrue(monCore.Message.Contains("introuvable"),
                "Attendu:introuvable Réel :" + monCore.Message);

            Assert.IsTrue(monCore.MessageEchec.Contains("Le texte 'introuvable' n'a pas été trouvé et il devrait être dans le fichier"),
                "Attendu:Le texte 'introuvable...  Réel " + monCore.MessageEchec);




        }


        [TestMethod]
        public void ContenuFichierMultipleEchecNCP()
        {
            // Variables
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();
            String FichierEssai = Chemin + 
                "ContenuFichierMultipleEchecNCP.txt";


            // Création du fichier de données
            CreerFichierTest(FichierEssai);

            // Création du xml de cas

            monCas.InnerXml = "<Assertion><Type>ContenuFichier</Type>" +
                "<Fichier>{{FichierEssai}}</Fichier>" +
                "<Contient>lect</Contient>" +
                "<Contient>from</Contient>" +
                "<NeContientPas>savana</NeContientPas>" +
                "<NeContientPas>sel</NeContientPas>" +
                "<MessageEchec>Message d'échech spécifique &gt;</MessageEchec>" +
                "</Assertion>";


            // définit les variables
            monCore.Variables.MAJVariable("FichierEssai", FichierEssai);



            // exécution de l'essai
            Assert.IsFalse (monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("sel"),
                "Attendu:sel Réel :" + monCore.Message);
            Assert.IsTrue(monCore.Message.Contains("lect"),
                "Attendu:lect Réel :" + monCore.Message);
            Assert.IsTrue(monCore.Message.Contains("from"),
                "Attendu:from Réel :" + monCore.Message);

            Assert.IsTrue(monCore.MessageEchec.Contains("Le texte 'sel' a été trouvé et il me devrait pas être dans le fichier"),
                "Attendu:Le texte 'sel'...  Réel " + monCore.MessageEchec);

            Assert.IsTrue(monCore.MessageEchec.Contains("Message d'échech spécifique >"),
                "Attendu: Message d'échech spécifique >  Réel " + monCore.MessageEchec);




        }

    }
}
