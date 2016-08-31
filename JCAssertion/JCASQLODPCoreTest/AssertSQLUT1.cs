using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using JCAssertionCore;
using JCASQLODPCore;

namespace JCASQLODPCoreTest
{
    [TestClass]
    public class AssertSQLUT1
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

        [TestCleanup]
        public void CleanTest()
        {
            monSQLClient.FermerConnection();  

        }
    

        [TestMethod]
        public void AssertSQLOK()
        {
            Double ResultatAttendu = 0;

            // Cas 1  Test d'égalité numérique entier = true
            Assert.IsTrue(monSQLClient.AssertSQL (
                "select count(*)  as R  from dual",1),
                "fail cas 1 de AssertSQLOK()");

            // Cas 2  Test d'égalité numérique réel = true
            ResultatAttendu = 0.25 ;
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select count(*) / 4  as R  from dual",
                ResultatAttendu),
                "fail cas 2 de AssertSQLOK()" +
                Environment.NewLine + monSQLClient.Resume   );

            // Cas 3 Test d'égalité numérique entier = false
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select count(*)  as R  from dual", 12),
                "fail cas 3 de AssertSQLOK()");

            // Cas 4 Test d'égalité numérique réel = false
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select count(*) / 2 as R  from dual", 1.11),
                "fail cas 4 de AssertSQLOK()");
  
            // Cas 5 Tester surcharge string = true
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select 'Mon résultat'  as R  from dual",
                "Mon résultat"),
                "fail cas 5 de AssertSQLOK()");

            // Cas 6 Tester surcharge string = false
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select 'pas ça' as R  from dual", 
                "pas vraiment cela"),
                "fail cas 6 de AssertSQLOK()");

            // Cas 9  Test de non égalité numérique = true
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select count(*) / 2  as R  from dual", 0.50001,"!="),
                "fail cas 9 de AssertSQLOK()");

            // Cas 10  Test de non égalité numérique = false
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select count(*) / 2  as R  from dual", 0.50, "!="),
                "fail cas 10 de AssertSQLOK() " +
                monSQLClient.Resume  );

            // Cas 11  Test de sql plus grand numérique = true
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select count(*) + .5  as R  from dual", 1, "pg"),
                "fail cas 11 de AssertSQLOK() " +
                monSQLClient.Resume);

            // Cas 12  Test de sql plus grand numérique = false
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select count(*) - .5  as R  from dual", 1, "pg"),
                "fail cas 12 de AssertSQLOK() " +
                monSQLClient.Resume);

            // Cas 13  Test de sql plus grand ou égal  numérique = true
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select count(*)   as R  from dual", 1, "pg="),
                "fail cas 13 de AssertSQLOK() " +
                monSQLClient.Resume);

            // Cas 14  Test de sql plus grand ou égal  numérique = false
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select count(*) - .5  as R  from dual", 1, "pg="),
                "fail cas 14 de AssertSQLOK() " +
                monSQLClient.Resume);

            // Cas 15  Test de sql plus petit numérique = true
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select count(*) - .5  as R  from dual", 1, "pp"),
                "fail cas 15 de AssertSQLOK() " +
                monSQLClient.Resume);

            // Cas 16  Test de sql plus petit numérique = false
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select count(*) + .5  as R  from dual", 1, "pp"),
                "fail cas 16 de AssertSQLOK() " +
                monSQLClient.Resume);

            // Cas 17  Test de sql plus petit ou égal ou égal  numérique = true
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select count(*)   as R  from dual", 1, "pp="),
                "fail cas 17 de AssertSQLOK() " +
                monSQLClient.Resume);

            // Cas 18  Test de sql plus petit ou égal  numérique = false
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select count(*) + .5  as R  from dual", 1, "pp="),
                "fail cas 18 de AssertSQLOK() " +
                monSQLClient.Resume);

            // Cas 19  Test sql ne retourne aucunerangée = false
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select * from dual where rownum = 5", 1, "pp="),
                "fail cas 19 de AssertSQLOK() " +
                monSQLClient.Resume);

            // Cas 20 vérifier si le cas de tst est dans la banque oracle = false
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select count(*)  from JCATest where IDTEST = 'Cas1'", 1, "="),
                "fail cas 20 sous cas 1 de AssertSQLOK(). Le cas de test LogonCas1 n'est pas dans la table JCAUser " +
                monSQLClient.Resume);
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select count(*)  from JCATest where IDTEST = 'Cas2'", 1, "="),
                "fail cas 20 sous cas 2 de AssertSQLOK(). Le cas de test LogonCas1 n'est pas dans la table JCAUser " +
                monSQLClient.Resume); Assert.IsTrue(monSQLClient.AssertSQL(
                 "select count(*)  from JCATest where IDTEST = 'Cas3'", 1, "="),
                 "fail cas 20 sous cas 3 de AssertSQLOK(). Le cas de test LogonCas1 n'est pas dans la table JCAUser " +
                 monSQLClient.Resume);

            // Cas 21  Test pour un sql de résultat number entiere = true
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select TYPENUMBERINT from JCATest where IDTEST = 'Cas2'",
                45, "="),
                "fail cas 21 INT de AssertSQLOK() " +
                monSQLClient.Resume);
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select TYPENUMBERINT16 from JCATest where IDTEST = 'Cas2'",
                45, "="),
                "fail cas 21 INT16 de AssertSQLOK() " +
                monSQLClient.Resume);
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select TYPENUMBERINT32 from JCATest where IDTEST = 'Cas2'",
                45, "="),
                "fail cas 21 INT32 de AssertSQLOK() " +
                monSQLClient.Resume);

            Assert.IsTrue(monSQLClient.AssertSQL(
                "select TYPENUMBERINT64 from JCATest where IDTEST = 'Cas2'",
                45, "="),
                "fail cas 21 INT64 de AssertSQLOK() " +
                monSQLClient.Resume);

            // Cas 22 sql retourne colonne null = true
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select TYPENUMBERINT from JCATest where IDTEST = 'Cas1'",
                45, "="),
                "fail cas 22 de AssertSQLOK() " +
                monSQLClient.Resume);

            // Cas 23  Test sql retourne reel = truee
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select TYPENUMBERDEC from JCATest where IDTEST = 'Cas2'",
                15.345, "="),
                "fail cas 23 de AssertSQLOK() " +
                monSQLClient.Resume);
        
        
        
        
        
        }

        [TestMethod]
        public void AssertSQLPasOK()
        {
            // SQL retourne un résultat de type incompatible avec un nombre
            try {
              Assert.IsTrue(monSQLClient.AssertSQL(
                "select 'string' from dual",2));
                } catch (JCASQLODPException excep)
                {
                    Assert.IsTrue(excep.Message.Contains (
                        "La commande SQL retourne une chaîne de caractère et le résultat attendu est un nombre, commande = select 'string' from dual"),
                        "Mauvais message d'exception " +
                        excep.Message  ); 
                }

            // type incompatible abec une string
            try
            {
                Assert.IsTrue(monSQLClient.AssertSQL(
                  "select count(*) from dual", "1"));
            }
            catch (JCASQLODPException  excep)
            {
                Assert.IsTrue(excep.Message.Contains(
                    "La connande SQL :select count(*) from dual: ne retourne pas un résultat de type chaîne de caractère"),
                    "Mauvais message d'exception " +
                    excep.Message);
            }


            // erreur oracle
            try
            {
                Assert.IsTrue(monSQLClient.AssertSQL(
                  "select 'string'  dual", 2));
            }
            catch (Exception excep)
            {
                Assert.IsTrue(excep.Message.Contains(
                    "ORA-"),
                    "Mauvais message d'exception " +
                    excep.Message);
            }
        }
    }
}
