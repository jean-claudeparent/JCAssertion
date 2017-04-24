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
        public void JCAXMLXMLContientOK()
        {
            JCAXML monJCAXML = new JCAssertionCore.JCAXML();
            String monFichierTest = Chemin + "XML1.xml";
            Assert.IsTrue(System.IO.File.Exists(monFichierTest)); 
            // Chercher un livre avec un titre précis
            // Oberon's Legacy
            
            Assert.IsTrue (monJCAXML.XMLNoeudEgal (monFichierTest,
                "Title",
                "Oberon's Legacy"),
                "Le titre n'a pas été trouvé" +
                Environment.NewLine + monJCAXML.DebugInfo  );

            Assert.Fail("Pas encore implémenté");
        }
    }
}
