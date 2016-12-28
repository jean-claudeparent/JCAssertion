using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCASQLODPCore;


namespace JCASQLODPCoreTest
{
    [TestClass]
    public class LOBUT1
    {

        JCASQLODPClient monSQLClient = new JCASQLODPClient();

        private void ChargeBD()
            {
            }

            
        

        [TestInitialize]
        public void InitTest()
        {
            monSQLClient.User = "JCA";
            monSQLClient.Password = "JCA";
            monSQLClient.OuvrirConnection();
            monSQLClient.ActiverResume = true;
            ChargeBD();
            
        }

        [TestCleanup]
        public void CleanTest()
        {
            monSQLClient.FermerConnection();

        }

        [TestMethod]
        public void ChargeLOBOK()
        {
            Assert.Fail("Pas encore implémenté");
        }
    }
}
