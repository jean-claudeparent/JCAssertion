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

            // delete acec plus d'une ligne affectée
            // S'assurer que la deuxi`me rangée n'existe pas en l'effacant
            monSQLClient.SQLExecute("delete from JCATest where IDTest='" +
                maCleDeTest + "2'");  
            // * Ajouter une ligne à effacer
            monSQLUpdate = "update JCATest " +
                "set IDTEST = '" +
                maCleDeTest  + "2' where " +
                "IDTEST = '" +
                maCleDeTest + "'";
            Assert.AreEqual(1,
                monSQLClient.SQLExecute(monSQLUpdate),
                "Une rangée aurait du être modifiée.");
            Assert.AreEqual(1,
                monSQLClient.SQLExecute(monSQLInsert),
                "Une rangée aurait du être ajoutée.");


            // * définir delete de 2 rangées
            monSQLDelete = "delete from JCATest where " +
                "IDTEST in ('" +
                maCleDeTest + "','" +
                maCleDeTest + "2')";

            Resultat = monSQLClient.SQLExecute(monSQLDelete);
            Assert.AreEqual(2, Resultat,
                "Deux rangées aurait dû être effacées");
            Assert.IsTrue(monSQLClient.AssertSQL(SQLPrecondition, 0),
                   "La rangée qui ne devrait pas exister est toujours sur la base de données.");
               

            // produire une exception oracle
            // en insérant deux rangées avec la même clé unique

            monSQLClient.SQLExecute(monSQLInsert ); 
            try 
            {
                Resultat = monSQLClient.SQLExecute(monSQLInsert);
                Assert.Fail("Une exception d'insert en double aurait du se produire.");
            } catch (Exception excep)
                {
                    Assert.IsTrue(excep.Message.Contains("ORA-"),
                        "L'exception aurait du contenir ora- mais est " +
                        excep.Message );  
                }

            
        }
    }
}
