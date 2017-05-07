using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.IO;
using JCAssertionCore;


 
namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCAXMLUT2
    {

        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";

        
        [TestMethod]
        public void AssertXPathOKparxml()
        {
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();

            // Préparer les variables
            monCore.Variables.MAJVariable(
                "monFichier",Chemin + "XML2.");
            monCore.Variables.MAJVariable(
               "monOperateur", "=");
            monCore.Variables.MAJVariable(
               "monXPath", "//");
            monCore.Variables.MAJVariable(
               "monResultat", "1");
            
            monCore.Variables.MAJVariable(
               "maChaine", "Execute");
            monCore.Variables.MAJVariable(
               "moMessageEchech", 
               "Ceci est le mssahe d'échec provemamt d'une variable ");

   

            // cas 1 Vrai sans Contient


            monCas.InnerXml = "<Assertion>" +
                   "<Type>AssertionXPath</Type>" +
                   "<Fichier>{{monFichier}}xml</Fichier>" +
                   "<Operateur>pg{monOperateur}}</Operateur>" +
                   "<Resultat>{{monResultat}}</Resultat>" +
                   "<Expression>{monXPath}}ID</Expression>" +
                   "<MessageEchec>{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>"; 
            
            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "L'assertion aurait dûe être vraie. " +
                monCore.Message + Environment.NewLine  +
                monCore.MessageEchec);



            Assert.Fail("Pas encore implémenté");
        }

        [TestMethod]
        public void AssertXPathExcepparxml()
        {
            // todo pas de balise Fichier
            // todo pas de balise Expression

            Assert.Fail("Pas encore implémenté");
        }
    }
}
