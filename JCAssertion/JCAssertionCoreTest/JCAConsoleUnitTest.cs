using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCAConsoleUnitTest
    {
        [TestMethod]
        public void ArgumentTest()
        {
            String[] mesArgs = new String[0];
            JCAssertionCore.JCAConsole maConsole = new JCAssertionCore.JCAConsole();
            JCAssertionCore.JCAVariable mesVariables;


            mesVariables = maConsole.Arguments(mesArgs);
            Assert.AreEqual(0, mesVariables.Variables.Count,"Les barianles d'argument devraient êtres vides." );
            
            mesArgs = new String[10];
            mesArgs[0] = "/Fichier:c:app.exe";
            mesArgs[1] = "/on";
            mesArgs[2] = "/v:Cle=valeur";
            mesArgs[3] = "/fic2:c:app avec espace.exe /p";
            mesVariables = maConsole.Arguments(mesArgs);
            Assert.AreEqual("ON", mesVariables.GetValeurVariable("ON"));
            Assert.AreEqual("c:app.exe", mesVariables.GetValeurVariable("FICHIER"));
            Assert.AreEqual("Cle=valeur", mesVariables.GetValeurVariable("V"));
            Assert.AreEqual("c:app avec espace.exe /p", mesVariables.GetValeurVariable("FIC2"));
            Assert.IsNull(mesVariables.GetValeurVariable("FIxxxC2"));
 



        }
    }
}
