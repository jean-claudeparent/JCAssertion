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
                "Assertion : 3 (Réel) pg= 1 (Attendu)") &&
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
                "Assertion : 3 (Réel) = 111 (Attendu)") &&
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



            
        }

        [TestMethod]
        public void AssertXPathContientxml()
        {
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();

            // Préparer les variables
            monCore.Variables.MAJVariable(
                "monFichier", Chemin + "XML2.");
            monCore.Variables.MAJVariable(
               "monOperateur", "=");
            monCore.Variables.MAJVariable(
               "monXPath", "/ListeDAssertion/Assertion/");
            monCore.Variables.MAJVariable(
               "CONTIENTMAJUS", "CAS");
            monCore.Variables.MAJVariable(
              "CONTIENT", "cas");

            monCore.Variables.MAJVariable(
               "moMessageEchech",
               "Ceci est le mssage d'échec provemamt d'une variable ");



            // cas 1 Vrai AVEC CONTIENT


            monCas.InnerXml = "<Assertion>" +
                   "<Type>AssertXPath</Type>" +
                   "<Fichier>{{monFichier}}xml</Fichier>" +
                   "<Operateur>{{monOperateur}}</Operateur>" +
                   "<ResultatAttendu>1</ResultatAttendu>" +
                   "<Expression>{{monXPath}}Description</Expression>" +
                   "<Contient>{{CONTIENT}} 2</Contient>" +
                   "<MessageEchec>{{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";

            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "L'assertion aurait dûe être vraie. " +
                monCore.Message + Environment.NewLine +
                monCore.MessageEchec);

            Assert.IsTrue(monCore.Message.Contains(
                "Chercher les noeuds qui contiennent : cas 2") &&
            monCore.Message.Contains("Expression XPath : /ListeDAssertion/Assertion/Description") &&
            monCore.Message.Contains("Assertion AssertXPath") &&
            monCore.Message.Contains("Fichier XML à traiter :") &&
            monCore.Message.Contains("Assertion : 1 (Réel) = 1 (Attendu)") &&
           (!monCore.Message.Contains("Comparer en ne tenant pas compte des majuscules et minuscules")),
                "Cas 1 Mauvais contenu de Message : " + Environment.NewLine +
                monCore.Message);


            // cas 2 qui  fait une assertion fausse 
            // à cause de la case
            


            monCas.InnerXml = "<Assertion>" +
                   "<Type>AssertXPath</Type>" +
                   "<Fichier>{{monFichier}}xml</Fichier>" +
                   "<Operateur>=</Operateur>" +
                   "<ResultatAttendu>1</ResultatAttendu>" +
                   "<Expression>{{monXPath}}Description</Expression>" +
                   "<Contient>CAS</Contient>" +
                   "<MessageEchec>{{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";

            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "L'assertion aurait dûe être fausse. " +
                monCore.Message + Environment.NewLine +
                monCore.MessageEchec);

            Assert.IsTrue(monCore.Message.Contains(
                "Assertion : 0 (Réel) = 1 (Attendu)") &&
            monCore.Message.Contains(
            "Chercher les noeuds qui contiennent : CAS") &&
            monCore.Message.Contains("Assertion AssertXPath") &&
            monCore.Message.Contains("Fichier XML à traiter :") &&
            monCore.Message.Contains("XML2.xml") &&
           (!monCore.Message.Contains("ceci ne vient")),
                "Cas 2 Mauvais contenu de Message : " + Environment.NewLine +
                monCore.Message);

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Ceci est le mssage d'échec provemamt d'une variable  ceci ne vient pas de la variable."),
                "Cas 2 mauvais message d'échec : " +
                Environment.NewLine + monCore.MessageEchec);



           // cas 3 qui marche quand on ne tient pas
           //  compte de la case
            monCas.InnerXml = "<Assertion>" +
                   "<Type>AssertXPath</Type>" +
                   "<Fichier>{{monFichier}}xml</Fichier>" +
                   "<Operateur>=</Operateur>" +
                   "<ResultatAttendu>1</ResultatAttendu>" +
                   "<Expression>{{monXPath}}Description</Expression>" +
                   "<ContientMajus>CAS 2</ContientMajus>" +
                   "<MessageEchec>{{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";

            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "L'assertion aurait dûe être vraie. " +
                monCore.Message + Environment.NewLine +
                monCore.MessageEchec);

            Assert.IsTrue(monCore.Message.Contains(
                "Comparer en ne tenant pas compte des majuscules et minuscules"),
                "Mauvais contenu de message " + monCore.Message );
           
            



        }

        

        [TestMethod]
        public void AssertXPathExcepparxml()
        {
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();


            // pas de balise Fichier
            monCas.InnerXml = "<Assertion>" +
                   "<Type>AssertXPath</Type>" +
                   "<Operateur>{{monOperateur}}</Operateur>" +
                   "<ResultatAttendu>1</ResultatAttendu>" +
                   "<Expression>{{monXPath}}Description</Expression>" +
                   "<Contient>{{CONTIENT}} 2</Contient>" +
                   "<MessageEchec>{{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";
            try {
                Assert.IsTrue(monCore.ExecuteCas(monCas),
                    "L'assertion aurait dûe être vraie. " +
                    monCore.Message + Environment.NewLine +
                    monCore.MessageEchec);
                Assert.Fail("Une exception aurait dûe se produire");
            } catch (Exception excep)
                {
                    Assert.IsTrue(
                        excep.Message.Contains(
                        "Le XML ne contient pas la balise Fichier"),
                        "Mauvais contenu de message " +
                        excep.Message  );  
                }


            // pas de balise Expression
            monCas.InnerXml = "<Assertion>" +
                   "<Type>AssertXPath</Type>" +
                   "<Fichier>{{monFichier}}xml</Fichier>" +
                   "<Operateur>{{monOperateur}}</Operateur>" +
                   "<ResultatAttendu>1</ResultatAttendu>" +
                   "<Contient>{{CONTIENT}} 2</Contient>" +
                   "<MessageEchec>{{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";

            try
            {
                Assert.IsTrue(monCore.ExecuteCas(monCas),
                    "L'assertion aurait dûe être vraie. " +
                    monCore.Message + Environment.NewLine +
                    monCore.MessageEchec);
                Assert.Fail("Une exception aurait dûe se produire");
            }
            catch (Exception excep)
            {
                Assert.IsTrue(
                    excep.Message.Contains(
                    "Le XML ne contient pas la balise Expression"),
                    "Mauvais contenu de message " +
                    excep.Message);
            }

            // erreur de conversion en numerique de resultatattendu
            monCas.InnerXml = "<Assertion>" +
                   "<Type>AssertXPath</Type>" +
                   "<Fichier>test.xml</Fichier>" +
                   "<Operateur>{{monOperateur}}</Operateur>" +
                   "<ResultatAttendu>Comment çava?</ResultatAttendu>" +
                   "<Contient>CONTIENT0</Contient>" +
                   "<Expression>//Description</Expression>" +
                   "<MessageEchec>ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";

            try
            {
                Assert.IsTrue(monCore.ExecuteCas(monCas),
                    "L'assertion aurait dûe être vraie. " +
                    monCore.Message + Environment.NewLine +
                    monCore.MessageEchec);
                Assert.Fail("Une exception aurait dûe se produire");
            }
            catch (Exception excep)
            {
                Assert.IsTrue(
                    excep.Message.Contains(
                    "Impossible de convertir le résultat attendu 'Comment çava?' en un nombre"),
                    "Mauvais contenu de message " +
                    excep.Message);
            }


            // erreur de lecture de fichier

            monCas.InnerXml = "<Assertion>" +
                   "<Type>AssertXPath</Type>" +
                   "<Fichier>dl:::test.xml</Fichier>" +
                   "<Operateur>=</Operateur>" +
                   "<Contient>CONTIENT0</Contient>" +
                   "<Expression>//Description</Expression>" +
                   "<MessageEchec>ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";

            try
            {
                Assert.IsTrue(monCore.ExecuteCas(monCas),
                    "L'assertion aurait dûe être vraie. " +
                    monCore.Message + Environment.NewLine +
                    monCore.MessageEchec);
                Assert.Fail("Une exception aurait dûe se produire");
            }
            catch (Exception excep)
            {
                Assert.IsTrue(
                    excep.Message.Contains(
                    "a"),
                    "Mauvais contenu de message " +
                    excep.Message);
            }
        }
    }
}
