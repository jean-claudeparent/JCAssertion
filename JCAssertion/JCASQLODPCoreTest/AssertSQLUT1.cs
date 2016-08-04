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
            // SQL retourne un résultat de type incompatible
            try {
              Assert.IsTrue(monSQLClient.AssertSQL(
                "select 'string' from dual",2));
                } catch (JCAssertionException excep)
                {
                    Assert.IsTrue(excep.Message.Contains (
                        "La connande SQL :select 'string' from dual: ne retourne pas un résultat de type numérique"),
                        "Mauvais message d'exception " +
                        excep.Message  ); 
                }

            // erreur oracle
            try
            {
                Assert.IsTrue(monSQLClient.AssertSQL(
                  "select 'string'  dual", 2));
            }
            catch (Exception excep)
            {
                Assert.IsTrue(excep.Message.Contains(
                    "ORA-"),
                    "Mauvais message d'exception " +
                    excep.Message);
            }
        }
    }
}
