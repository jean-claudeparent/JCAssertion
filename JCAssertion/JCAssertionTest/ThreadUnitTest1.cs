using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Threading;
using JCAssertion;
using System.IO;





namespace JCAssertionTest
{
    [TestClass]
    public class ThreadUnitTest1
    {
        [TestMethod]
        public void ThreadExecuterAssertion()
        {
            // Finalement ce test est peu utile
            // car il ne lève pas les exceptions
            // quand un controle est manipulé dans le
            // mauvais thread

            JCAssertion.JCAssertion maForm;
            maForm = new JCAssertion.JCAssertion();


            JCAssertionCore.JCAUtilitaireFichier UF = new JCAssertionCore.JCAUtilitaireFichier();
            String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";
            

            // Faire un cas qui marcge en pasassant par le thread
            // Définir les fichiers
            String FichierVar = Chemin + "ThreadEssaiCompletVar.xml";
            String FichierAssertion = Chemin + "EssaiComplet.xml";
            String FichierActivite = Chemin + "ThreadEssaiCompletActivite.txt";
            String FichierJournal = Chemin + "ThreadJCAssertion_Journal.txt";

            // Effacer les fichiers de résultats de l'essai précédent
            UF.EffaceSiExiste(FichierActivite);
            UF.EffaceSiExiste(FichierJournal);

            // Simuler les arguments de ligne de commande du programme
            maForm.args = new String[5];  
            maForm.args[0] = "/FV:" + FichierVar;
            maForm.args[1] = "/fa:" + FichierAssertion;
            maForm.args[2] = "/j:" + FichierJournal;
            
            // Créer le fichier de variables
            JCAssertionCore.JCAVariable mesVariables =
                new JCAssertionCore.JCAVariable();
            mesVariables.MAJVariable("Fichier", FichierVar);
            mesVariables.EcrireFichier(FichierVar);
            

            // lancer et attendre la fin du thread
            maForm.LancerThread();
            while (maForm.monThreadIsAlive())
                Thread.Sleep(1);
            Assert.AreEqual(0, maForm.getCodeDeRetour(),
                "Le code de retour devrait petre 0."
                + maForm.getMessage()
                + maForm.getException().Message );
        }
    }
}
