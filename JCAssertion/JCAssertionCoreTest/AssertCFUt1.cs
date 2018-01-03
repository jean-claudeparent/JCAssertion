using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;



namespace JCAssertionCoreTest
{
    [TestClass]
    public class AssertCFUt1
    {
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                 "Ressources\\";

        [TestMethod]
        public void AssertCF1()
        {
            JCAPontXML  monPont = new JCAPontXML ();
            Int64 ResultatReel = 0;

            Assert.IsTrue(
                monPont.AssertCompteFichiers(
                Chemin + "CompteFichiers",
                "*.xml",
                "pg",
                1,
                ref ResultatReel ),
                "Résultat réel : " +
                ResultatReel.ToString()  );

            Assert.IsFalse(
                monPont.AssertCompteFichiers(
                Chemin + "CompteFichiers",
                "*.xml",
                "pp",
                1,
                ref ResultatReel),
                "Résultat réel : " +
                ResultatReel.ToString());

            
            
        }
    }
}
