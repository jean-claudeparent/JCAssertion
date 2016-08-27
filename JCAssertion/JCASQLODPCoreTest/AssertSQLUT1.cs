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
            Assert.IsTrue(monSQLClient.AssertSQLString(
                "select 'Mon résultat'  as R  from dual",
                "Mon résultat"),
                "fail cas 5 de AssertSQLOK()");

            // Cas 6 Tester surcharge string = false
            Assert.IsFalse(monSQLClient.AssertSQLString(
                "select 'pas ça' as R  from dual", 
                "pas vraiment cela"),
                "fail cas 6 de AssertSQLOK()");

            // Cas 9  Test de non égalité numérique = true
            Assert.IsTrue(monSQLClient.AssertSQL(
                "select count(*) / 2  as R  from dual", 0.50001,"!="),
                "fail cas 9 de AssertSQLOK()");

            // Cas 10  Test de non égalité numérique = false
            monSQLClient.ActiverResume = true; 
            Assert.IsFalse(monSQLClient.AssertSQL(
                "select count(*) / 2  as R  from dual", 0.50, "!="),
                "fail cas 10 de AssertSQLOK() " +
                monSQLClient.Resume  );
        
        
        
        
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
                Assert.IsTrue(monSQLClient.AssertSQLString(
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
