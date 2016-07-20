using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCASQLODPCore;
using JCAssertionCore;





namespace JCASQLODPCoreTest
{
    [TestClass]
    public class CreerConnectionStringTest
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
            /// <testsummary>
            /// CrerConnectionString exception si user à null. Le password est fourni.
            /// </testsummary>
            JCASQLODPClient monSQLCliemt = new JCASQLODPClient();
            monSQLCliemt.Password = "PasswordOK"; 
            try
                {
                    String maCs = "";
                    maCs = monSQLCliemt.CreerConnectionString();
                } catch (JCAssertionException excep)
                {
                    Assert.IsTrue(excep.Message.Contains("Pour une connection à la base de données le user est obligatoire"),
                        "Maucvais message d'exception :" + excep.Message);  
                }

            /// <testsummary>
            /// CrerConnectionString exception si user à "". Le password est fourni.
            /// </testsummary>
            monSQLCliemt = new JCASQLODPClient();
            monSQLCliemt.Password = "PasswordOK";
            monSQLCliemt.User = ""; 
            try
            {
                String maCs = "";
                maCs = monSQLCliemt.CreerConnectionString();
            }
            catch (JCAssertionException excep)
            {
                Assert.IsTrue(excep.Message.Contains("Pour une connection à la base de données le user est obligatoire"),
                    "Maucvais message d'exception :" + excep.Message);
            }

            /// <testsummary>
            /// CrerConnectionString exception si password à null. Le user est fourni.
            /// </testsummary>
            monSQLCliemt = new JCASQLODPClient();
            monSQLCliemt.User = "UserOK";
            try
            {
                String maCs = "";
                maCs = monSQLCliemt.CreerConnectionString();
            }
            catch (JCAssertionException excep)
            {
                Assert.IsTrue(excep.Message.Contains("Pour une connection à la base de données le mot de passe est obligatoire"),
                    "Maucvais message d'exception :" + excep.Message);
            }

            /// <testsummary>
            /// CrerConnectionString exception si password à "". Le user est fourni.
            /// </testsummary>
            monSQLCliemt = new JCASQLODPClient();
            monSQLCliemt.User = "UserOK";
            monSQLCliemt.Password = "";
 
            try
            {
                String maCs = "";
                maCs = monSQLCliemt.CreerConnectionString();
            }
            catch (JCAssertionException excep)
            {
                Assert.IsTrue(excep.Message.Contains("Pour une connection à la base de données le mot de passe est obligatoire"),
                    "Maucvais message d'exception :" + excep.Message);
            }




            
        }

    }
}
