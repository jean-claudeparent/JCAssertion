using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCASQLODPCore;
using System.IO;
 


namespace JCASQLODPCoreTest
{
    [TestClass]
    public class LOBUT1
    {

        JCASQLODPClient monSQLClient = new JCASQLODPClient();
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                 "Ressources\\";


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
                + "'Valeur CLOB à remplacer','F0F0')");

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
            String FichierCLOB = Chemin + "CLOB.xml";
            String FichierBLOB = Chemin + "BLOB.jpg";
            // Test : Mattre à jour les champs BLOB (1 appel `ChargerLOB) 
            // et CLOB (1 autre appel) du cas 1,
            // ces colommes sont `null

            // ensuite en un seul apppel mettre `jour les champs clob
            // des cas 2 et 3
            // Ensuite en un seul appel mettre
            // à jour les colonnes  clob des cas 2 et 3
            // 
            // le cas 3 a des valeurs dams le clob et le blob
            
            // Vérifier les pré requis de l'essai
            Assert.IsTrue(File.Exists(FichierCLOB),
                "Le fichier CLOB d'essai est introubable " +
                FichierCLOB);

            Assert.IsTrue(File.Exists(FichierBLOB),
                "Le fichier BLOB d'essai est introubable " +
                FichierBLOB);

            String monSQLCheck1 = "select count(*) from JCATest " +
                "where IDTEST like 'LOBUT1%' and " +
                " TYPECLOB is null";

            Assert.IsTrue(
            monSQLClient.AssertSQL(monSQLCheck1, 2),
            "La base de données oracle ne contient pas les pré requis id=1");

            String monSQLCheck2 = "select count(*) from JCATest " +
                "where IDTEST like 'LOBUT1%' and " +
                " TYPEBLOB is null";

            Assert.IsTrue(
            monSQLClient.AssertSQL(monSQLCheck2, 2),
            "La base de données oracle ne contient pas les pré requis id=2");

            String monSQLCheck3 = "select count(*) from JCATest " +
                "where IDTEST like 'LOBUT1%' and " +
                " TYPEBLOB is not null and " +
                "TYPECLOB like '%Valeur CLOB à remplacer%'";

            Assert.IsTrue(
            monSQLClient.AssertSQL(monSQLCheck3, 1),
            "La base de données oracle ne contient pas les pré requis id=3");
 
 
            // Lancer le test 1. charger un BLOB
            // dans une seule rangée
            monSQLClient.ChargeLOB("select TYPEBLOB from JCATest "+
            " where where IDTEST = 'LOBUT1_1'", 
            FichierBLOB);
  
            // vérifier résultat test 1
            Assert.IsTrue(
            monSQLClient.AssertSQL(monSQLCheck2, 1),
            "Il ne devrait ne rester qu'une ligne sur la banque ou le BLOB est null");

            Assert.IsTrue(
            monSQLClient.AssertSQL(monSQLCheck2, 2),
            "Il devrait rester 2 rangées avec le clob à null");

            // todo test 2


            Assert.Fail("Pas encore implémenté");
        }
    }
}
