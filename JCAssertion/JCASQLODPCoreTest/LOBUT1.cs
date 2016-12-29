using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCASQLODPCore;


namespace JCASQLODPCoreTest
{
    [TestClass]
    public class LOBUT1
    {

        JCASQLODPClient monSQLClient = new JCASQLODPClient();


        /// <summary>
        /// Nettoie et recrée les rangées
        /// de test dans la table JCATest
        /// </summary>
        private void ChargeBD()
            {
            monSQLClient.SQLExecute(
                    "delete from JCATest where IDTEST like 'LOBUT1%'");
            // Insérer les données
            monSQLClient.SQLExecute(
                "INSERT INTO JCATest(IDTEST,NOM,INFO) " +
                "VALUES('LOBUT1_1'," +
                "'cas 1 des unit test lob','Info lob cas 1')");

            monSQLClient.SQLExecute(
                "INSERT INTO JCATest(IDTEST,NOM,INFO) " +
                "VALUES('LOBUT1_2'," +
                "'cas 2 des unit test lob','Info lob cas 2')");

            monSQLClient.SQLExecute(
                "INSERT INTO JCATest(IDTEST,NOM,INFO, " +
                "TYPECLOB,TYPEBLOB) " +
                "VALUES('LOBUT1_3'," +
                "'cas 3 des unit test lob','Info lob cas 3'," 
                + "'Valeur VLOB à remplacer','F0F0')");

            // ,ettre un lob à remplacer



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
