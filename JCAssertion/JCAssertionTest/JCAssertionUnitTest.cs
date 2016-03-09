using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertion;


namespace JCAssertionTest
{
    [TestClass]
    public class JCAssertionUnitTest
    {
        [TestMethod]
        public void JCAssertionTest1()
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

            // cas qui marche
            monProgramme.args[0] = "/FV:" +
                 JCAssertionCore.JCACore.RepertoireAssembly() +
                "\\Ressources\\EssaiCompletVar.xml";
            monProgramme.args[1] = "/fa:" + JCAssertionCore.JCACore.RepertoireAssembly() +
                "\\Ressources\\EssaiComplet.xml";
            JCAssertionCore.JCAVariable mesVariables =
                new JCAssertionCore.JCAVariable();
            mesVariables.MAJVariable("Fichier",JCAssertionCore.JCACore.RepertoireAssembly() +
                "\\Ressources\\EssaiCompletVar.xml" );
            mesVariables.EcrireFichier(JCAssertionCore.JCACore.RepertoireAssembly() +

                "\\Ressources\\EssaiCompletVar.xml");
            int Resultat = monProgramme.Execute() ;
            String FichierActivite = JCAssertionCore.JCACore.RepertoireAssembly() +
                "\\Ressources\\EssaiCompletActivite.txt";
            System.IO.File.WriteAllText(FichierActivite, monProgramme.gettxbActivite());

            Assert.AreEqual(0, Resultat  , 
                "Erreur technique" + monProgramme.gettxbActivite());
            
            Assert.Fail("Implanter le reste de l'essai");


        }
    }
}
