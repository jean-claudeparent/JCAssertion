using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCASQLODPCore;
using JCAssertionCore;


namespace JCASQLODPCoreTest
{
    [TestClass]
    public class OuvrirConnectionTest
    {
        [TestMethod]
        public void ConnectionOK()
        {
            /// <testsummary>
            /// Ouvrir une connection valide.
            /// </testsummary>
            JCASQLODPClient monSQLCliemt = new JCASQLODPClient();
            monSQLCliemt.User = "JCA";
            monSQLCliemt.Password = "JCA";
            monSQLCliemt.OuvrirConnection();
            monSQLCliemt.FermerConnection();
  

           
        }

        [TestMethod]
        public void ConnectionPasOK()
        {
            /// <testsummary>
            /// Ouvrir une connection valide.
            /// </testsummary>
            JCASQLODPClient monSQLCliemt = new JCASQLODPClient();
            monSQLCliemt.User = "JCA";
            monSQLCliemt.Password = "JCA";
            monSQLCliemt.Serveur = "serveurnexistepas"; 
            try {
                    monSQLCliemt.OuvrirConnection();
                } catch (Exception excep)
                {
                    Assert.IsTrue(excep.Message.Contains("ORA-"),
                        "Mauvais message d'exception : " + excep.Message  ); 
                }
  

             
        }
    }
}
