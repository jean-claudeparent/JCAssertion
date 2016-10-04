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
        public void SQLExecute()
        {
            // Vérifier que la rangée de test m'existe pas
            // si elle existe l'effacer
            String maCleDeTest = "SQLExecuteOK1";

            String SQLPrecondition = "select count(*) from JCATest where"+
                " IDTEST = '" +
                maCleDeTest + "'";

            String monSQLDelete = "delete from JCATest where "
                + "IDTEST = '" +
                maCleDeTest + "'";

            if (!(monSQLClient.AssertSQL(SQLPrecondition, 0)))
                {
                    monSQLClient.SQLExecute(monSQLDelete);
                    Assert.IsTrue(monSQLClient.AssertSQL(SQLPrecondition, 0),
                        "La rangée qui ne devrait pas exister est toujours sur la base de données.");
                }
            Assert.IsTrue(monSQLClient.AssertSQL(SQLPrecondition, 0),
                "La ligne de table qui ne devrait pas exister existe encore");

            // insérer
            String monSQLInsert = "insert into JCATest" +
                "(IDTEST, NOM, INFO) " +
                "values('SQLExecuteOK1','Test','Test info')";
            Int64 Resultat = 0;
            Resultat = monSQLClient.SQLExecute(monSQLInsert);
            Assert.AreEqual(1,Resultat,
                "Le nombre de rangées insérées retournées par SQLExecute n'est pas 1 mais " +
                Resultat.ToString()  ); 

            // updater

            // de;ete


            // produire une exception oracle

            Assert.Fail ("Pas encore implémenté.");
        }
    }
}
