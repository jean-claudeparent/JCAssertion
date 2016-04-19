using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCAUtilitaireUnitTest1
    {
        [TestMethod]
        public void EventLogErreur()
        {
            Assert.IsFalse(JCAUtilitaires.EVSourceExiste("Sourceindexistante"),
                "Sourceindexistante ne devrait pas exister commesource de event log"); 
            Assert.IsTrue (JCAUtilitaires.EVSourceExiste(),
                "JCAssertion devrait exister comme source de event log"); 
            JCAssertionCore.JCAUtilitaires.EventLogErreur("JCAssertionCoreUnitTest",
                "Message dans le vent log");
            
        }
    }
}
