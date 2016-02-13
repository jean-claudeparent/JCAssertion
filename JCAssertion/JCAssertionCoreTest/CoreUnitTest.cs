using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;



namespace JCAssertionCoreTest
{
    [TestClass]
    public class CoreUnitTest
    {
        [TestMethod]
        public void CoreLoadOK()
        {
            Core monJCACore = new Core();
            Assert.AreEqual(7, monJCACore ,"Le fichier chargé devrait contenir 7 cas de test, réel = " + monJCACore.NombreCas.ToString()  );



            
        }
    }
}
