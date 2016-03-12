using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml;



namespace JCAssertionCoreTest
{
    [TestClass]
    public class ExecuteCasTest
    {
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";

        [TestMethod]
        public void FichierExiste()
        {
            // testc abec variablenon fournie
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument () ;

            monCas.InnerXml = "<Assertion><Type>FichierExiste</Type><Fichier>{{Fichier}}</Fichier></Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas ));
            Assert.IsTrue (monCore.Message.Contains("La variable Fichiern'a pas eu de valeur fournie"), "Attendu:La variable Fichiern'a pas eu de valeur fournie");

            // variable fournie mais fichier existe pas
            monCore.Variables.MAJVariable("Fichier",Chemin + "DivideByZeroException:existepas.pasla");
            Assert.IsFalse(monCore.ExecuteCas (monCas ) );
            Assert.IsTrue(monCore.Message.Contains("Le fichier n'existe pas"), "Attendu:Le fichier n'existe pas");    

            // Valeurs fournies,fichier existe
            monCore.Variables.MAJVariable("Fichier", Chemin +
                "FichierDeCasOK.xml");
            Assert.IsTrue(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("Le fichier existe"),
                "Attendu:Le fichier existe");    

        }

        [TestMethod]
        public void SubstituerVariablesFichier()
        {
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();

            monCas.InnerXml = "<Assertion><Type>SubstituerVariablesFichier</Type>" +
                "<FichierModele>{{Chemin}}SQLOK.txt</FichierModele>" +
                "<FichierSortie>{{Chemin}}SQLOK.txt.sql</FichierSortie>" +
                "<FichierVariable>{{Chemin}}SQLOK.txt.sql</FichierVariable>" +
                "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("La variable Cheminn'a pas eu de valeur fournie"), 
                "Attendu:La variable Chemin n'a pas eu de valeur fournie. Réel :" + monCore.Message  );

        }
        
    }
}
