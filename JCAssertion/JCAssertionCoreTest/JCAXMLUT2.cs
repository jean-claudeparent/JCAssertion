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
                   "<Type>AssertXPath</Type>" +
                   "<Fichier>{{monFichier}}xml</Fichier>" +
                   "<Operateur>pg{{monOperateur}}</Operateur>" +
                   "<ResultatAttendu>{{monResultat}}</ResultatAttendu>" +
                   "<Expression>{{monXPath}}ID</Expression>" +
                   "<MessageEchec>{{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>"; 
            
            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "L'assertion aurait dûe être vraie. " +
                monCore.Message + Environment.NewLine  +
                monCore.MessageEchec);

            Assert.IsTrue(monCore.Message.Contains(
                "Assertion : 3 pg= 1") &&
            monCore.Message.Contains("Expression XPath : //ID") &&
            monCore.Message.Contains("Assertion AssertXPath") &&
            monCore.Message.Contains("Fichier XML à traiter :") &&
            monCore.Message.Contains("XML2.xml") &&
           (!monCore.Message.Contains("ceci ne vient")),
                "Cas 1 Mauvais contenu de Message : " + Environment.NewLine +
                monCore.Message);  


            // cas 2 qui  fait une assertion fausse 
            //  pour vérifier le message d'échec

            

            monCas.InnerXml = "<Assertion>" +
                   "<Type>AssertXPath</Type>" +
                   "<Fichier>{{monFichier}}xml</Fichier>" +
                   "<Operateur>=</Operateur>" +
                   "<ResultatAttendu>111</ResultatAttendu>" +
                   "<Expression>{{monXPath}}ID</Expression>" +
                   "<MessageEchec>{{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>"; 
            
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "L'assertion aurait dûe être fausse. " +
                monCore.Message + Environment.NewLine  +
                monCore.MessageEchec);

            Assert.IsTrue(monCore.Message.Contains(
                "Assertion : 3 = 111") &&
            monCore.Message.Contains("Expression XPath : //ID") &&
            monCore.Message.Contains("Assertion AssertXPath") &&
            monCore.Message.Contains("Fichier XML à traiter :") &&
            monCore.Message.Contains("XML2.xml") &&
           (!monCore.Message.Contains("ceci ne vient")),
                "Cas 2 Mauvais contenu de Message : " + Environment.NewLine +
                monCore.Message);  

            Assert.IsTrue (monCore.MessageEchec.Contains(
                "Ceci est le mssahe d'échec provemamt d'une variable  ceci ne vient pas de la variable."),
                "Cas 2 mauvais message d'échec : " +
                Environment.NewLine  + monCore.MessageEchec   ); 



            // cas avec Contient

            // cas avec ContientMaj
            Assert.Fail("Pas encore implémenté");
        }

        [TestMethod]
        public void AssertXPathExcepparxml()
        {
            // todo pas de balise Fichier
            // todo pas de balise Expression
            // erreur de conversion en numerique de resultatattendu

            Assert.Fail("Pas encore implémenté");
        }
    }
}
