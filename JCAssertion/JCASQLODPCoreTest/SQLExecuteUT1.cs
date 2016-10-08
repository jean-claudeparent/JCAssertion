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
            Assert.IsTrue(monSQLClient.AssertSQL(SQLPrecondition, 1),
                "La rangée n'a pas été créée.");

            // updater
            String monInfo = "+++123+++";
            String SQLPrecondition2 = "select count(*) from JCATest where" +
                " ((IDTEST = '" +
                maCleDeTest + "') AND " +
                "(info = '" + monInfo + "')";

            String monSQLUpdate = "update JCATest " +
                "set info = '" +
                monInfo + "' where "+
                "IDTEST = '" +
                maCleDeTest + "'";
            // * vérifier que la rengée n'exustre oas déjà
            Assert.IsTrue(monSQLClient.AssertSQL(SQLPrecondition2,0), 
                "La rangée avec l'info " +
                monInfo + " ne devrait pas déjà exister.");
            
            // * exécuter update et vérifier résultat
            Resultat = monSQLClient.SQLExecute(monSQLUpdate);
            Assert.AreEqual(1, Resultat ,
                "SQLExecute aurait du retourner 1");
            Assert.IsTrue(monSQLClient.AssertSQL(SQLPrecondition2, 1),
                          "La rangée avec l'info " +
                          monInfo + " devrait exister.");

            // de;ete


            // produire une exception oracle

            Assert.Fail ("Pas encore implémenté.");
        }
    }
}
