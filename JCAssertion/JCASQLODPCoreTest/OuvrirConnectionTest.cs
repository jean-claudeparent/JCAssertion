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

            Assert.IsTrue(  
                 monSQLCliemt.SiConnectionOuverte(),
                 "La connection n'est pas ouverte");

            monSQLCliemt.FermerConnection();

            Assert.IsFalse(
                 monSQLCliemt.SiConnectionOuverte(),
                 "La connection est toujours ouverte");
            // test de dispose multiple
            monSQLCliemt.Dispose();
            monSQLCliemt.Dispose();




        }

        [TestMethod]
        public void ConnectionPasOK()
        {
            /// <testsummary>
            /// Ouvrir une connection invalide.
            /// </testsummary>
            JCASQLODPClient monSQLCliemt = new JCASQLODPClient();
            monSQLCliemt.User = "JCA";
            monSQLCliemt.Password = "JCA";
            monSQLCliemt.Serveur = "serveurnexistepas"; 
            try {
                    monSQLCliemt.OuvrirConnection();
                    Assert.Fail("Une exception aurait dû se produire");  
                } catch (Exception excep)
                {
                    Assert.IsTrue(excep.Message.Contains("ORA-"),
                        "Mauvais message d'exception : " + excep.Message  ); 
                }
            monSQLCliemt.Dispose();  
  

             
        }
    }
}
