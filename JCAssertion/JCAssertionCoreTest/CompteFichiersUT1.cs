using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml ;

namespace JCAssertionCoreTest
{
    [TestClass]
    public class CompteFichiersUT1
    {
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";


        [TestMethod]
        public void CompteFichiersparxml()
        {
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();

            // Préparer les variables
            monCore.Variables.MAJVariable(
                "monRepertoire", Chemin);
            monCore.Variables.MAJVariable(
               "monOperateur", "=");
            monCore.Variables.MAJVariable(
               "monResultat", "1");

            monCore.Variables.MAJVariable(
               "moMessageEchech",
               "Ceci est le mssage d'échec provemamt d'une variable ");



            // cas 1 Vrai operateeur = 


            monCas.InnerXml = "<Assertion>" +
                   "<Type>CompteFichiers</Type>" +
                   "<Repertoire>{{monRepertoire}}CompteFichiers</Repertoire>" +
                   "<Operateur>pg{{monOperateur}}</Operateur>" +
                   "<ResultatAttendu>{{monResultat}}</ResultatAttendu>" +
                   "<MessageEchec>{{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";

            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "L'assertion aurait dûe être vraie. " +
                Environment.NewLine +
                monCore.Message + Environment.NewLine +
                monCore.MessageEchec);

            Assert.IsTrue(monCore.Message.Contains(
                "Assertion : 5 (Réel) pg= 1 (Attendu)") &&
            monCore.Message.Contains(
            "Assertion CompteFichiers") &&
            monCore.Message.Contains(
            "Répertoire à traiter :") &&
            monCore.Message.Contains(
            "Pattern des fichiers à compter : *.*") &&
            monCore.Message.Contains("\\CompteFichiers") &&
           (!monCore.Message.Contains("ceci ne vient")),
                "Cas 1 Mauvais contenu de Message : " + Environment.NewLine +
                monCore.Message);


            // cas 2 qui  fait une assertion fausse 
            //  pour vérifier le message d'échec



            monCas.InnerXml = "<Assertion>" +
                   "<Type>CompteFichiers</Type>" +
                   "<Repertoire>{{monRepertoire}}CompteFichiers</Repertoire>" +
                   "<Operateur>=</Operateur>" +
                   "<ResultatAttendu>{{monResultat}}1111</ResultatAttendu>" +
                   "<MessageEchec>{{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";

            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "L'assertion aurait dûe être fausse. " +
                monCore.Message + Environment.NewLine +
                monCore.MessageEchec);

            Assert.IsTrue(monCore.Message.Contains(
                "Assertion : 5 (Réel) = 11111 (Attendu)") &&
            !monCore.Message.Contains(
            "ceci ne vient"),
                "Cas 2 Mauvais contenu de Message : " + Environment.NewLine +
                monCore.Message);

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Ceci est le mssage d'échec provemamt d'une variable  ceci ne vient pas de la variable."),
                "Cas 2 mauvais message d'échec : " +
                Environment.NewLine + monCore.MessageEchec);


            


        }


        [TestMethod]
        public void CompteFichiersparxmlexcep()
        {
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();
            // La méthoode ExecuteCas traite
            // lres exception comme des échecs

            // Pas de balise répertoire

            monCas.InnerXml = "<Assertion>" +
                   "<Type>CompteFichiers</Type>" +
                   "<Operateur>pg{{monOperateur}}</Operateur>" +
                   "<ResultatAttendu>{{monResultat}}</ResultatAttendu>" +
                   "<MessageEchec>ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";

            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "L'assertion arait due etre fausse");
            Assert.IsTrue(monCore.Message.Contains(
                "XML ne contient pas la balise Repertoire"),
                "Nauvais message " +
                monCore.Message  );

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "XML ne contient pas la balise Repertoire"),
                "Nauvais message " +
                monCore.MessageEchec);
              
            
            
            // balise répertoire existe mais vide

            monCas.InnerXml = "<Assertion>" +
                   "<Type>CompteFichiers</Type>" +
                   "<Repertoire></Repertoire>" +
                   "<Operateur>pg{{monOperateur}}</Operateur>" +
                   "<ResultatAttendu>{{monResultat}}</ResultatAttendu>" +
                   "<MessageEchec>{moMessageEchech} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "L'assertion arait due etre fausse");
            Assert.IsTrue(monCore.Message.Contains(
                "La balise Repertoire est vide"),
                "Nauvais message " +
                monCore.Message);

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "La balise Repertoire est vide"),
                "Nauvais message " +
                monCore.MessageEchec);
              
            
            // Opêrateur invalide

            monCas.InnerXml = "<Assertion>" +
                   "<Type>CompteFichiers</Type>" +
                   "<Repertoire>12</Repertoire>" +
                   "<Operateur>pgx}}</Operateur>" +
                   "<ResultatAttendu>12</ResultatAttendu>" +
                   "<MessageEchec>{moMessageEchech} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "L'assertion arait due etre fausse");
            Assert.IsTrue(monCore.Message.Contains(
                "Pour cette comparaison l'opérateur 'PGX}}' n'est pas un opérateur valide"),
                "Nauvais message " +
                monCore.Message);

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Pour cette comparaison l'opérateur 'PGX}}' n'est pas un opérateur valide"),
                "Nauvais message " +
                monCore.MessageEchec);
              
            
            // Resultat attendunonconvertissable en entier
            monCas.InnerXml = "<Assertion>" +
                   "<Type>CompteFichiers</Type>" +
                   "<Repertoire>12</Repertoire>" +
                   "<Operateur>=</Operateur>" +
                   "<ResultatAttendu>xux</ResultatAttendu>" +
                   "<MessageEchec>{moMessageEchech} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "L'assertion arait due etre fausse");
            Assert.IsTrue(monCore.Message.Contains(
                "Impossible de convertir le résultat attendu 'xux' en un nombre"),
                "Nauvais message " +
                monCore.Message);

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Impossible de convertir le résultat attendu 'xux' en un nombre"),
                "Nauvais message " +
                monCore.MessageEchec);
              
            

            
            // Pattern invalide "LLL:::"
            // Produit une exception qui n'est pas une JCAssertionException

            monCas.InnerXml = "<Assertion>" +
                   "<Type>CompteFichiers</Type>" +
                   "<Repertoire>" +
                   Chemin +  "</Repertoire>" +
                   "<Operateur>=</Operateur>" +
                   "<ResultatAttendu>1012</ResultatAttendu>" +
                   "<Pattern>LLL:::</Pattern>" +
                   "<MessageEchec>{moMessageEchech} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";
            try {
                Assert.IsFalse(monCore.ExecuteCas(monCas),
                    "L'assertion arait due etre fausse");
                Assert.Fail("Une exception aurait du se produire");
                }
            catch (Exception excep)
                {
                    Assert.IsTrue(excep.Message.Contains("a")   );  
                }
            Assert.IsTrue(monCore.Message.Contains(
                "Pattern des fichiers à compter : LLL:::"),
                "Nauvais message " +
                monCore.Message);

            Assert.IsTrue(monCore.MessageEchec == "",
                "Nauvais message " +
                monCore.MessageEchec);
              
            

             
        }

    }
}
