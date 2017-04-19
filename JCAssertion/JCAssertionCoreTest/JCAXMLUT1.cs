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
             
            Assert.Fail("Pas encore implémenté");
        }
    }
}
