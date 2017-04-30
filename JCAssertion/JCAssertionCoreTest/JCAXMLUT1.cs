using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCAXMLUT1
    {
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";

        [TestMethod]
        public void JCAXMLAssertXPathOK()
        {
            JCAXML monJCAXML = new JCAssertionCore.JCAXML();
            String monFichierTest = Chemin + "XML1.xml";
            Assert.IsTrue(System.IO.File.Exists(monFichierTest)); 
            // Cas 1 il existe au moins un book

            Int64 ResultatReel = 0;

            Assert.IsTrue (monJCAXML.AssertXPath (monFichierTest,
                "//book",
                "pg", 0, ref  ResultatReel),
                "Aucun livre (book) trouvé " +
                ResultatReel.ToString()  );

            Assert.Fail("Pas encore implémenté");
        }
    }
}
