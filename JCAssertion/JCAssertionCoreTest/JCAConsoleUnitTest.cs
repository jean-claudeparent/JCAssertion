using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCAConsoleUnitTest
    {
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";

        [TestMethod]
        public void ArgumentTest()
        {
            String[] mesArgs = new String[0];
            JCAssertionCore.JCAConsole maConsole = new JCAssertionCore.JCAConsole();
            JCAssertionCore.JCAVariable mesVariables;


            mesVariables = maConsole.Arguments(mesArgs);
            Assert.AreEqual(0, mesVariables.Variables.Count,"Les varianles d'argument devraient êtres vides." );
            
            mesArgs = new String[10];
            mesArgs[0] = "/Fichier:c:app.exe";
            mesArgs[1] = "/on";
            mesArgs[2] = "/v:Cle=valeur";
            mesArgs[3] = "/fic2:c:app avec espace.exe /p";
            mesArgs[4] = "Test55";
            mesArgs[5] = "Test43:Valeur43";
           
            mesVariables = maConsole.Arguments(mesArgs);
            Assert.AreEqual("ON", mesVariables.GetValeurVariable("ON"));
            Assert.AreEqual("c:app.exe", mesVariables.GetValeurVariable("FICHIER"));
            Assert.AreEqual("Cle=valeur", mesVariables.GetValeurVariable("V"));
            Assert.AreEqual("c:app avec espace.exe /p", mesVariables.GetValeurVariable("FIC2"));
            Assert.AreEqual("Valeur43", mesVariables.GetValeurVariable("TEST43"));
            Assert.AreEqual("Test55", mesVariables.GetValeurVariable("Test55"));
            
            Assert.IsNull(mesVariables.GetValeurVariable("FIxxxC2"));
 



        }

        [TestMethod]
        public void ExtraireParamTest()
        {
            JCAssertionCore.JCAConsole maConsole = new JCAssertionCore.JCAConsole();
            String maCle;
            String maValeur;

            Assert.IsTrue(maConsole.ExtraireParam ("/duplex", out maCle , out maValeur ));
            Assert.AreEqual("DUPLEX", maCle );
            Assert.AreEqual("DUPLEX", maValeur);

            Assert.IsTrue(maConsole.ExtraireParam("/duplex:", out maCle, out maValeur));
            Assert.AreEqual("DUPLEX", maCle);
            Assert.AreEqual("", maValeur);

            Assert.IsFalse (maConsole.ExtraireParam("", out maCle, out maValeur));
            Assert.AreEqual("", maCle);
            Assert.AreEqual("", maValeur);

            Assert.IsTrue(maConsole.ExtraireParam("/duplex:c:/test:a", out maCle, out maValeur));
            Assert.AreEqual("DUPLEX", maCle);
            Assert.AreEqual("c:/test:a", maValeur);

            Assert.IsTrue(maConsole.ExtraireParam("TEST55", out maCle, out maValeur));
            Assert.AreEqual("TEST55", maCle);
            Assert.AreEqual("TEST55", maValeur);



        }

         [TestMethod]
        public void ExecuteProgrammeTest()
        {
             JCAssertionCore.JCAConsole maConsole = 
                 new JCAssertionCore.JCAConsole ();
             String NomProgramme = Chemin + "Exit1.cmd";

             // Créer 1 fichier exécutable
            System.IO.File.WriteAllText(NomProgramme  ,
                "echo off" + Environment.NewLine  +
                 "echo Ligne 1 de 3" + Environment.NewLine  +
                "echo Ligne 2 de 3" + Environment.NewLine  +
                "echo Ligne 3 de 3" + Environment.NewLine  +
                "exit %1" + Environment.NewLine );
            String Sortie = "";

            Assert.AreEqual(99, maConsole.ExecuteProgramme  (NomProgramme ,"99" ,
                ref Sortie),
             "L'exécution aurait du retourner 99" );
            Assert.IsTrue(Sortie.Contains("Ligne 3 de 3"),
                "Attendu:Ligne 3 de 3");
            Assert.AreEqual(0, maConsole.ExecuteProgramme(NomProgramme, " 0", ref Sortie),
             "L'exécution aurait du marcher");
            Assert.IsTrue(Sortie.Contains("Ligne 1 de 3"),
                "Attendu:Ligne 1 de 3");
            
         }
        
    }
}
