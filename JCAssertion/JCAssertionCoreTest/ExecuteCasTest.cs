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
                "<FichierModele>{{Chemin}}SQLexec.txt</FichierModele>" +
                "<FichierSortie>{{Chemin}}SQLexec.txt.sql</FichierSortie>" +
                "<FichierVariables>{{Chemin}}SQLexec.var.xml</FichierVariables>" +
                "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("La variable Cheminn'a pas eu de valeur fournie"), 
                "Attendu:La variable Chemin n'a pas eu de valeur fournie. Réel :" + monCore.Message  );

            // cas qui marche
            String Modele = Chemin + "SQLexec.txt";
            System.IO.File.WriteAllText(Modele, "spool {{SpoolFile}}" +
                Environment.NewLine + "select '{{Logon}}' from dual;" +
                Environment.NewLine  );

            // Créer le fichier de variables SQLexec.var.xml
            JCAVariable sqlvar = new JCAVariable();
            sqlvar.MAJVariable("Logon","jean-claude");
            sqlvar.MAJVariable("SpoolFile","z:log.txt");
            sqlvar.EcrireFichier(Chemin + "SQLexec.var.xml");


            monCore.Variables.MAJVariable("Chemin", Chemin );
            Assert.IsTrue(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("La substitution des variables dans le fichier a réussie"), "Attendu:La substitution des variables dans le fichier a réussie");
            //todo verif fichier sql
            String Contenu = System.IO.File.ReadAllText(Chemin + "SQLexec.txt.sql");
            Assert.IsTrue(Contenu.Contains("spool z:log.txt"), "Attendu:spool z:log.txt");
            Assert.IsTrue(Contenu.Contains("select 'jean-claude' from dual;"), "Attendu:select 'jean-claude' from dual;");


            // TODO unitester les autres validations



        }

        [TestMethod]
        public void ContenuFichier()
        {
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();

            monCas.InnerXml = "<Assertion><Type>ContenuFichier</Type>" +
                "<Fichier>{{Chemin}}SQLexec.txt</Fichier>" +
                "<Contient>{{s}}pool</Contient>" +
                "<NeContientPas>{{s}}SQLexec.var.xml</NeContientPas>" +
                "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("La variable Cheminn'a pas eu de valeur fournie"),
                "Attendu:La variable Chemin n'a pas eu de valeur fournie. Réel :" + monCore.Message);

            // Cas qyu  marche
            monCore.Variables.MAJVariable("Chemin",Chemin );
            monCore.Variables.MAJVariable("s", "s");
            Assert.IsTrue(monCore.ExecuteCas(monCas), monCore.Message );
            Assert.IsTrue(monCore.Message.Contains("L'assertion est vraie"), monCore.Message); 
            
            //TODO unit tester les autres validations

            
        }
        
    }
}
