using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCASQLODPCore;


namespace JCASQLODPCoreTest
{
    [TestClass]
    public class SQLExecuteUT1
    {
        JCASQLODPClient monSQLClient = new JCASQLODPClient();



        [TestInitialize]
        public void InitTest()
        {
            monSQLClient.User = "JCA";
            monSQLClient.Password = "JCA";
            monSQLClient.OuvrirConnection();
            monSQLClient.ActiverResume = true;

        }

        [TestMethod]
        public void SQLExecuteOK()
        {
            Assert.Fail ("Pas encore implémenté.");
        }
    }
}
