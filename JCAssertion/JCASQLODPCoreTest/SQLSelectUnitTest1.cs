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
            /// SQLSelect dca les plui simples.
            /// </testsummary>
            JCASQLODPClient monSQLCliemt = new JCASQLODPClient();
            monSQLCliemt.User = "JCA";
            monSQLCliemt.Password = "JCA";
            monSQLCliemt.OuvrirConnection(); 
            monSQLCliemt.SQLSelect("select count(*) from dual");
            monSQLCliemt.FermerConnection(); 
        }
    }
}
