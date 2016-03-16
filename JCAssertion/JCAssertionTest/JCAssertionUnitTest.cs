using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertion;


namespace JCAssertionTest
{
    [TestClass]
    public class JCAssertionUnitTest
    {
            JCAssertionCore.JCAUtilitaireFichier UF = new JCAssertionCore.JCAUtilitaireFichier();
            String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";
        
        

        [TestMethod]
        public void JCAssertionTestValide()
        {
            // Créer et configurer l'instance delaclasse qui fait letravail du form load
            JCAssertion.JCAssertion monProgramme = new JCAssertion.JCAssertion();
            monProgramme.Interactif = false;
            monProgramme.args = new String[3];
            Assert.IsTrue(monProgramme.gettxbActivite().Contains("Démarrage"));
            // Définir les fichiers
            String FichierVar = Chemin + "EssaiCompletVar.xml";
            String FichierAssertion = Chemin + "EssaiComplet.xml";
            String FichierActivite = Chemin + "\\EssaiCompletActivite.txt";
            String FichierJournal = Chemin + "JCAssertion_Journal.txt";

            // Effacer les fichiers de résultats de l'essai précédent
            UF.EffaceSiExiste(FichierActivite);
            UF.EffaceSiExiste(FichierJournal);

            // Simuler les arguments de ligne de commande du programme
            monProgramme.args[0] = "/FV:" + FichierVar;
            monProgramme.args[1] = "/fa:" + FichierAssertion;
            monProgramme.args[2] = "/j:" + FichierJournal;
            
            // Créer le fichier de valeurs de variables
            JCAssertionCore.JCAVariable mesVariables =
                new JCAssertionCore.JCAVariable();
            mesVariables.MAJVariable("Fichier", FichierVar);
            mesVariables.EcrireFichier(FichierVar );
            
            // Faire le test
            int Resultat = monProgramme.Execute();
            System.IO.File.WriteAllText(FichierActivite, monProgramme.gettxbActivite());

            Assert.AreEqual(0, Resultat,
                "Erreur technique" + monProgramme.gettxbActivite());
            String Activite = monProgramme.gettxbActivite();

            Assert.IsTrue(Activite.Contains("Lecture du fichier d'assertion"), "Lecture du fichier d'assertion");
            Assert.IsTrue(Activite.Contains("Nombre de cas à traiter :"), "Nombre de cas à traiter :");
            Assert.IsTrue(Activite.Contains("Lecture du fichier de variables :"), "Lecture du fichier de variables :");
            Assert.IsTrue(Activite.Contains("Exécution du cas 1"), "Exécution du cas 1");
            Assert.IsTrue(Activite.Contains("Assertion vraie"), "Assertion vraie");
            Assert.IsTrue(Activite.Contains("Cas réussis :"), "Cas réussis :");
            Assert.IsTrue(Activite.Contains("Cas en échec :"), "Cas en échec :");
            
            // Fichier de jourmal
            Assert.IsTrue (System.IO.File.Exists (FichierJournal),
                "Le fichier de journal devrait exister"   );
            String ContenuJournal = System.IO.File.ReadAllText(FichierJournal);
            Assert.IsTrue(ContenuJournal.Contains("Le fichier existe."));
        
        
        }
        

        [TestMethod]
        public void JCAssertionTestInvalide()
        {
            JCAssertion.JCAssertion monProgramme = new JCAssertion.JCAssertion ();
            monProgramme.Interactif  = false;
            Assert.IsTrue(monProgramme.gettxbActivite().Contains ("Démarrage")  );
            // cas sans argument
            String[] argsvide = new String[0];
            monProgramme.args = argsvide;
            Assert.AreEqual (99, monProgramme.Execute ());


            // avec argument mais /FA

            monProgramme.args = new String[2] ;
            monProgramme.args[0] = "/FV:test";
            monProgramme.args[1] = "/ab:test";
            Assert.AreEqual(99, monProgramme.Execute());

            

        }
    }
}
