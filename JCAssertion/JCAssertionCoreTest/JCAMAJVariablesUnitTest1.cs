using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml;
using System.IO;

namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCAMAJVariablesUnitTest1
    {
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";


        [TestMethod]
        public void MAJVariablesOKTest()
        {
            String NomFichier = Chemin +
                "MAJVariablesOKTest.xml";
            
            // Préparer les variables de test
            JCAVariable mesVariables = new JCAVariable();
            mesVariables.MAJVariable("s","Suffixe");
            mesVariables.MAJVariable("p", "Préfixe");
            mesVariables.MAJVariable("Autre", "Autres");
            mesVariables.MAJVariable("R", "Résultat");
            
            Assert.IsTrue(mesVariables.GetValeurVariable("JCA.FichierDeVariables")
                == null,
                "La variable JCA.FichierDeVariables ne devrait pas exister."); 

            // Défiir le noued xml qui contient le test
            JCACore monCore = new JCACore();
            monCore.Variables = mesVariables; 
                

            XmlDocument monCas = new XmlDocument ();
            
            monCas.InnerXml = "<Assertion><Type>MAJVariables</Type>" +
                "<Cle>{{R}}</Cle>" +
                "<Valeur>{{p}}te{{Autre}}st{{s}}</Valeur>" +
                "</Assertion>";

            // Un cas qui marche sans fichier
           

            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "L'éxécution du cas a échoué : " +
                monCore.Message + Environment.NewLine +
                monCore.MessageEchec );
            Assert.IsTrue(monCore.Variables.GetValeurVariable("Résultat").Contains("PréfixeteAutresstSuffixe"),
                "Variable Résultat avec le mauvais contenu : " +
                 monCore.Variables.GetValeurVariable("Résultat"));
 

            // Un cas qui marche avec fichier
            monCore.Variables = mesVariables;
            monCore.Variables.MAJVariable(
               JCAVariable.Constantes.JCA_FichierDeVariables, NomFichier );
            if (System.IO.File.Exists(NomFichier))
                System.IO.File.Delete(NomFichier );
            Assert.IsFalse(System.IO.File.Exists(NomFichier),
                "Le fichier ne devrait pas exister");
            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "L'éxécution du cas a échoué : " +
                monCore.Message + Environment.NewLine +
                monCore.MessageEchec);
            Assert.IsTrue(monCore.Variables.GetValeurVariable("Résultat").Contains("PréfixeteAutresstSuffixe"),
                "Variable Résultat avec le mauvais contenu : " +
                 monCore.Variables.GetValeurVariable("Résultat"));
            Assert.IsTrue(System.IO.File.Exists(NomFichier),
                "Le fichierdevrait exister : " + NomFichier );


            String ResultatFichier = System.IO.File.ReadAllText(NomFichier );
            Assert.IsTrue(ResultatFichier.Contains("PréfixeteAutresstSuffixe"),
                "Variable Résultat dans le fichier " +
                NomFichier  +" avec le mauvais contenu : " +
                 ResultatFichier);
            

            
        }
        [TestMethod]
        public void MAJVariablesPasOKTest()
        {
            // Il manque la balise de clé
            // Il manque la balise de valeur
            // Le nom de fichier de variable cause une exception

            Assert.Fail("Pas encore implémenté");
        }

    }
}
