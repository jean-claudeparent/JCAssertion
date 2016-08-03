using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using JCASQLODPCore;

namespace JCASQLODPCoreTest
{
    [TestClass]
    public class AssertSQLUT1
    {
        JCASQLODPClient monSQLClient = new JCASQLODPClient();
            


        [TestInitialize]
        public void InitTest()
        {
            monSQLClient.User = "JCA";
            monSQLClient.Password = "JCA";
            monSQLClient.OuvrirConnection(); 
            
        }

        [TestCleanup]
        public void CleanTest()
        {
            monSQLClient.FermerConnection();  

        }
    

        [TestMethod]
        public void AssertSQLOK()
        {
            Assert.IsTrue(monSQLClient.AssertSQL (
                "select count(*)  as R  from dual",1),
                "fail cas 1 de AssertSQLOK()");

            Assert.IsTrue(monSQLClient.AssertSQL(
                "select count(*) / 4  as R  from dual", 1 / 4),
                "fail cas 2 de AssertSQLOK()");
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select count(*)  as R  from dual", 12),
                "fail cas 3 de AssertSQLOK()");
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select count(*) / 2 as R  from dual", 1.11),
                "fail cas 1 de AssertSQLOK()");  
        
        
        
        }

        [TestMethod]
        public void AssertSQLPasOK()
        {
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select count(*)  from dual",2));
        }
    }
}
