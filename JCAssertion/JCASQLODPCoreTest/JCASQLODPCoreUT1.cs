using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCASQLODPCore;




namespace JCASQLODPCoreTest
{
    [TestClass]
    public class JCASQLODPCoreUT1
    {
        [TestMethod]
        public void CreerConnectionStringOK()
        {
            /// <testsummary>
            /// Créer la connection string de ODP avec le serveur à null.
            /// </testsummary>
            JCASQLODPClient monSQLCliemt = new JCASQLODPClient();
            monSQLCliemt.User = "JCAUser";
            monSQLCliemt.Password = "JCAPassword";
            String maCS = monSQLCliemt.CreerConnectionString();
            Assert.AreEqual(
                "Data Source=localhost;User=JCAUser;Password=JCAPassword",
                maCS);

            /// <testsummary>
            /// Créer la connection string de ODP avec le serveur à "".
            /// </testsummary>
            monSQLCliemt.User = "JCAUser";
            monSQLCliemt.Password = "JCAPassword";
            monSQLCliemt.Serveur = ""; 
            maCS = monSQLCliemt.CreerConnectionString();
            Assert.AreEqual(
                "Data Source=localhost;User=JCAUser;Password=JCAPassword",
                maCS);

            /// <testsummary>
            /// Créer la connection string de ODP avec un nom de  serveur.
            /// </testsummary>
            monSQLCliemt.Serveur = "MonServeur";
            maCS = monSQLCliemt.CreerConnectionString();
            Assert.AreEqual(
                "Data Source=MonServeur;User=JCAUser;Password=JCAPassword",
                maCS);

        }

        [TestMethod]
        public void CreerConnectionStringExcep()
        {
            Assert.Fail("Pas encore implémenté");
        }

    }
}
