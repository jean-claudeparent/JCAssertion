using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.IO;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCAUtilitaireFichierUnitTest
    {
        [TestMethod]
        public void SubstituerVariableFichierTest()
        {
            JCAUtilitaireFichier monUFichier = new JCAUtilitaireFichier();

            // quelques erreurs
            try {
                monUFichier.SubstituerVariableFichier(null, "","");
                } catch (Exception excep)
            {
                Assert.IsNotNull(excep );
            }

            // cas qui marche
            String monPath = JCACore.RepertoireAssembly() + "\\Ressources\\";
            JCAVariable mesVariables = new JCAVariable();
            mesVariables.MAJVariable("Logon","noursicain");
            mesVariables.MAJVariable("SpoolFile", "z:allo");
            mesVariables.EcrireFichier (monPath + "SQLOK.xml" );

            if (System.IO.File.Exists(monPath + "SQLOK.SQL"))
                System.IO.File.Delete(monPath + "SQLOK.SQL");
            Assert.IsFalse(System.IO.File.Exists(monPath + "SQLOK.SQL"));
            monUFichier.SubstituerVariableFichier(monPath + "SQLOK.txt", monPath + "SQLOK.SQL", monPath + "SQLOK.xml");






        }
    }
}
