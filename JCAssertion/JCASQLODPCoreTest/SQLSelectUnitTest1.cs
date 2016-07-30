using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using JCASQLODPCore;



namespace JCASQLODPCoreTest
{
    [TestClass]
    public class SQLSelectUnitTest1
    {
        [TestMethod]
        public void SQLSelectOKDeBase()
        {
            /// <testsummary>
            /// SQLSelect cas les plui simples.
            /// </testsummary>
            JCASQLODPClient monSQLCliemt = new JCASQLODPClient();
            monSQLCliemt.User = "JCA";
            monSQLCliemt.Password = "JCA";
            monSQLCliemt.OuvrirConnection(); 
            monSQLCliemt.SQLSelect("select count(*) from dual");
            monSQLCliemt.FermerConnection(); 
        }


        [TestMethod]
        public void SQLSelectPasOK()
        {
            /// <testsummary>
            /// SQLSelect Cas avec exception oracle
            /// </testsummary>
            JCASQLODPClient monSQLCliemt = new JCASQLODPClient();
            monSQLCliemt.User = "JCA";
            monSQLCliemt.Password = "JCA";
            monSQLCliemt.OuvrirConnection();
            try {
                  monSQLCliemt.SQLSelect("select count(*) from dualnexistepas");
                } catch (Exception excep)
                {
                    Assert.IsTrue(excep.Message.Contains ("ORA-"),
                        "Mauvais message " + excep.Message  );
                    monSQLCliemt.FermerConnection(); 
                }
            monSQLCliemt.FermerConnection();
        }

    }
}
