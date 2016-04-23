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
            String IDunique = Guid.NewGuid().ToString ();
            JCAUtilitaires U = new JCAUtilitaires();

            U.JournalEveSource = "JCAssertion";
            U.JournalEveNombreMax = 15;
            U.LancerExceptionJE = true;




            Assert.IsFalse(JCAUtilitaires.EVSourceExiste("Sourceindexistante"),
                "Sourceindexistante ne devrait pas exister commesource de event log"); 
            Assert.IsTrue (JCAUtilitaires.EVSourceExiste(),
                "JCAssertion devrait exister comme source de event log");
            // RechercheJournalEve
            Assert.IsFalse(
                U.RechercheJournalEve(IDunique).Contains(IDunique ) );
 
            
             U.EventLogErreur(" ID unique = " + IDunique );
            
            String Journal = U.RechercheJournalEve(IDunique);

            // code d'exploration
            Journal = U.LogExplore();



            Assert.IsTrue(
                Journal.Contains(IDunique),
                "Identifiant unique non trouvé dans " +
                Journal );
 

        }
    }
}
