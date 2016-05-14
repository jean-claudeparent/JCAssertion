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

            monCas.InnerXml = "<Assertion>"+
                "<Type>FichierExiste</Type>"+
                "<Fichier>{{Fichier}}</Fichier>"+
                "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas ));
            Assert.IsTrue (monCore.Message.Contains("La variable Fichier n'a pas eu de valeur fournie"), "Attendu:La variable Fichiern'a pas eu de valeur fournie");
            // Le MessageEchec est généré par une exception
            Assert.IsTrue(monCore.MessageEchec.Contains("La variable Fichier n'a pas eu de valeur fournie"), 
                monCore.MessageEchec  ); 
            
            // variable fournie mais fichier existe pas.
            // MessageEchec générique

            monCore.Variables.MAJVariable("Fichier",Chemin + "DivideByZeroException:existepas.pasla");
            Assert.IsFalse(monCore.ExecuteCas (monCas ) );
            Assert.IsTrue(monCore.Message.Contains("Le fichier n'existe pas"), "Attendu:Le fichier n'existe pas");
            Assert.IsTrue(monCore.MessageEchec.Contains(" n'existe pas et il devrait exister"),
                monCore.MessageEchec);

            // variable fournie mais fichier existe pas.
            // MessageEchec spécifique

            monCas.InnerXml = "<Assertion>"+
                "<Type>FichierExiste</Type>" +
                "<Fichier>{{Fichier}}</Fichier>" +
                "<MessageEchec>Message d'échec spécifique</MessageEchec></Assertion>";
            

            monCore.Variables.MAJVariable("Fichier", Chemin + "DivideByZeroException:existepas.pasla");
            Assert.IsFalse(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("Le fichier n'existe pas"), "Attendu:Le fichier n'existe pas");
            Assert.IsTrue(monCore.MessageEchec.Contains("Message d'échec spécifique ("),
                monCore.MessageEchec); 
            

            // Valeurs fournies,fichier existe
            monCore.Variables.MAJVariable("Fichier", Chemin +
                "FichierDeCasOK.xml");
            Assert.IsTrue(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("Le fichier existe"),
                "Attendu:Le fichier existe");
            Assert.AreEqual("",monCore.MessageEchec);

            // Valeurs fournies,fichier existe mais est un répetoire

            monCore.Variables.MAJVariable("Fichier", Chemin);
            Assert.IsTrue(monCore.ExecuteCas(monCas),"Execute n.a pas retourné tru pour un répertoire");
            Assert.IsTrue(monCore.Message.Contains("Le fichier existe"),
                "Attendu:Le fichier existe");
            Assert.AreEqual("", monCore.MessageEchec,
                "Le message d'échec devrait être vide");
            

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
            Assert.IsTrue(monCore.Message.Contains("La variable Chemin n'a pas eu de valeur fournie"), 
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

            System.IO.File.WriteAllText(Chemin + "ContenuFichier.txt", "spool {{SpoolFile}}" +
                Environment.NewLine + "select '{{Logon}}' from dual;" +
                Environment.NewLine);
            monCas.InnerXml = "<Assertion><Type>ContenuFichier</Type>" +
                "<Fichier>{{Chemin}}ContenuFichier.txt</Fichier>" +
                "<Contient>{{s}}pool</Contient>" +
                "<NeContientPas>{{s}}SQLexec.var.xml</NeContientPas>" +
                "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("La variable Chemin n'a pas eu de valeur fournie"),
                "Attendu:La variable Chemin n'a pas eu de valeur fournie. Réel :" + monCore.Message);

            // Cas qyu  marche
            monCore.Variables.MAJVariable("Chemin",Chemin );
            monCore.Variables.MAJVariable("s", "s");
            Assert.IsTrue(monCore.ExecuteCas(monCas), monCore.Message );
            Assert.IsTrue(monCore.Message.Contains("L'assertion est vraie"), 
                "Attendu:L.assertion estbraie. Réel:" + monCore.Message); 
            
            //TODO unit tester les autres validations

            
        }

        [TestMethod]
        public void ExecuteProgramme()
        {
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();
            String NomProgramme = "ExecutePrg.bat";

            System.IO.File.WriteAllText(Chemin + NomProgramme, "echo off" +
                Environment.NewLine + "Echo Le programme roule %1" + Environment.NewLine +
                "exit %1" + Environment.NewLine);

                
            monCas.InnerXml = "<Assertion><Type>ExecuteProgramme</Type>" +
                "<Programme>{{Chemin}}" + NomProgramme + "</Programme>" +
                "<Arguments>{{CodeDeRetour}}</Arguments>" +
                "</Assertion>";
            // Echec produit par une exception

            Assert.IsFalse(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("La variable Chemin n'a pas eu de valeur fournie"),
                "Attendu:La variable Chemin n'a pas eu de valeur fournie. Réel :" + monCore.Message);
            Assert.IsTrue(monCore.MessageEchec.Contains("La variable Chemin n'a pas eu de valeur fournie"),
                monCore.MessageEchec); 
            


            // um cas qui marche et retourne true
            monCore.Variables.MAJVariable("Chemin", Chemin);
            monCore.Variables.MAJVariable("CodeDeRetour", "0");
            Assert.IsTrue(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("Résultat de l'exécution de "),
                "Attendu:Résultat de l'exécution de  Réel :" + monCore.Message);
            Assert.AreEqual("",monCore.MessageEchec); 
            
            

            // un cas qui marche et retourne false
            monCore.Variables.MAJVariable("CodeDeRetour", "45");
            Assert.IsFalse (monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("Le programme roule 45"),
                "Attendu:Le programme roule 45. Réel :" + monCore.Message);
            Assert.IsTrue(monCore.MessageEchec.Contains(" a terminé avec le code de retour 45"),
                monCore.MessageEchec); 
            
            // test retournant faux avec message d'échec spécifique

            monCas.InnerXml = "<Assertion><Type>ExecuteProgramme</Type>" +
                "<Programme>{{Chemin}}" + NomProgramme + "</Programme>" +
                "<Arguments>{{CodeDeRetour}}</Arguments>" +
                "<MessageEchec>Message spécifique 32</MessageEchec>" +
                "</Assertion>";
            
            
            monCore.Variables.MAJVariable("CodeDeRetour", "45");
            Assert.IsFalse(monCore.ExecuteCas(monCas));
            Assert.IsTrue(monCore.Message.Contains("Le programme roule 45"),
                "Attendu:Le programme roule 45. Réel :" + monCore.Message);
            Assert.IsTrue(monCore.MessageEchec.Contains("Message spécifique 32 (Code de retour : 45 )"),
                monCore.MessageEchec); 
            

        }
        
    }
}
