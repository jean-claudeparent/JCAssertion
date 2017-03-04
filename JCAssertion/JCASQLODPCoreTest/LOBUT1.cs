using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCASQLODPCore;
using System.IO;
using System.Text;
 


namespace JCASQLODPCoreTest
{
    [TestClass]
    public class SQLODPLOBUT1
    {

        JCASQLODPClient monSQLClient = new JCASQLODPClient();
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                 "Ressources\\";
        String FichierCLOB;
        String FichierBLOB;

          
        // Définiitions des commandes SQL d'assertion
        String CompteCLOBAvant = "select count(*) from JCATest " +
                "where IDTEST like 'LOBUT1%' and " +
                " TYPEBLOB is not null and " +
                "TYPECLOB like '%Valeur CLOB à remplacer%'";
        String CompteCLOBNull = "select count(*) from JCATest " +
                "where IDTEST like 'LOBUT1%' and " +
                " TYPECLOB is null";
        String CompteBLOBNull = "select count(*) from JCATest " +
                "where IDTEST like 'LOBUT1%' and " +
                " TYPEBLOB is null";
        String CompteCLOBApres = "select count(*) from JCATest " +
                "where IDTEST like 'LOBUT1%' and " +
                "TYPECLOB like '%</ListeDAssertion>%'";



            
        /// <summary>
        /// Efface les fichiers de sortie s'ils existent
        /// </summary>
        /// <param name="Fichier"></param>
        private void Menage(String Fichier) 
            {
                String monFichier = Chemin + Fichier;
                if (System.IO.File.Exists(monFichier))
                    System.IO.File.Delete(monFichier);   
            }

        private Boolean  Existe(String Fichier)
        {
            String monFichier = Chemin + Fichier;
            return System.IO.File.Exists(monFichier);

        }

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

            // Cas 4 pour avoir des lob qui restent a null

            monSQLClient.SQLExecute(
                "INSERT INTO JCATest(IDTEST,NOM,INFO) " +
                "VALUES('LOBUT1_4'," +
                "'cas 4 des unit test lob','Info lob cas 2')");


            



            }

            
        

        [TestInitialize]
        public void InitTest()
        {
            monSQLClient.User = "JCA";
            monSQLClient.Password = "JCA";
            monSQLClient.OuvrirConnection();
            monSQLClient.ActiverResume = true;
            FichierCLOB = Chemin + "CLOB.xml";
            FichierBLOB = Chemin + "BLOB.jpg";
        
        }

        [TestCleanup]
        public void CleanTest()
        {
            monSQLClient.FermerConnection();

        }
        
        /// <summary>
        /// Méthode de test  oz les fonctions de LOB fonctionnent normalement
        /// </summary>
        [TestMethod]
        public void SQLODPLOBOK()
        {
            // Test : Mattre à jour les champs BLOB (1 appel `ChargerLOB) 
            // et CLOB (1 autre appel) du cas 1,
            // ces colommes sont `null

            // ensuite en un seul apppel mettre `jour les champs clob
            // des cas 2 et 3
            // Ensuite en un seul appel mettre
            // à jour les colonnes  clob des cas 2 et 3
            // 
            // le cas 3 a des valeurs dams le clob et le blob
            ChargeBD();
            
            // Vérifier les pré requis de l'essai
            Assert.IsTrue(File.Exists(FichierCLOB),
                "Le fichier CLOB d'essai est introuvable " +
                FichierCLOB);

            Assert.IsTrue(File.Exists(FichierBLOB),
                "Le fichier BLOB d'essai est introubable " +
                FichierBLOB);
            // Valider qu'il y a 3 rangées avec le CLOB `null
            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteCLOBNull, 3),
            "La base de données oracle ne contient pas les pré requis id=1");

            // Valider qu'il y a 3 rangées avec le BLOB à null
            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteBLOBNull, 3),
            "La base de données oracle ne contient pas les pré requis id=2");

            
            // Valider qu'il existe une rangée avec le CLOB avec la valeur à remplacer
            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteCLOBAvant, 1),
            "La base de données oracle ne contient pas les pré requis id=3");
 
 
            // Lancer le test 1. charger un BLOB
            // dans une seule rangée. Le BLOB esy à null.
            Assert.AreEqual(1,
             monSQLClient.ChargeLOB("select IDTEST,TYPEBLOB AS BLOB from JCATest " +
             " where IDTEST = 'LOBUT1_1'", 
             FichierBLOB),
             "ChargerLOB aurait du retourner 1 comme nombre de rangées affectées");
  
            // vérifier résultat test 1
            // Il est censé ne rester que 2 BLOB `null
            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteBLOBNull, 2),
            "Il ne devrait ne rester que 2 lignes sur la banque ou le BLOB est null cas test 1 après id=1004");

            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteCLOBNull, 3),
            "Il devrait rester 3 rangées avec le clob à null id=1005");

            // Faire le test 2.
            // Changer une rangée, mettre du contenu dans le clob
            // qui est à null

            // Vérification avant test
            // 3 rangées avec clob `null
            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteCLOBNull, 3),
            "erreur CompteCLOBnull id=1006");
            // 1 rangée avec contenu dans clob, qui contient la valeur à remplacer
            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteCLOBAvant, 1),
            "erreur CompteCLOBAvant id=1006");
            // 0 rangées avec la nouvelle valeur dans le clob
            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteCLOBApres, 0),
            "erreur CompteCLOBApres id=1007");


            // Lancer l'esssai cas 2
            Assert.AreEqual(1,
            monSQLClient.ChargeLOB("select IDTEST,TYPECLOB AS CLOB from JCATest " +
             " where IDTEST = 'LOBUT1_1'",
                FichierCLOB),
            "ChargerLOB aurait du retourner 1 comme nombre de rangées affectées");
            // Il devrait y avoir un clob  changé
            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteCLOBApres, 1),
            "erreur CompteCLOBApres une rangée devrait avoir changé id=1008");
 
            // todo 2Il devrait rester un clob à null
            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteCLOBNull, 2),
            "erreur il devrait rester juste un clob à null id=1009");
            
            // Il devrait rester un clob avant modification
            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteCLOBAvant, 1),
            "erreur il devrait rester juste un clob avec la valeur à remplacer id=1010");
            
            // Test 3 changer 2 bloB d'un coup, un
            // est à null l'autre conntient quelque chose
            Assert.AreEqual(2,
             monSQLClient.ChargeLOB("select IDTEST,TYPEBLOB AS BLOB from JCATest " +
             " where IDTEST in ('LOBUT1_2','LOBUT1_3')",
             FichierBLOB),
             "ChargerLOB aurait du retourner 2 comme nombre de rangées affectées");

            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteBLOBNull, 1),
            "erreur il  devrait rester 1 blob à null ie=1011");

            // Test 4 changer 2 cloB d'un coup, un
            // est à null l'autre conntient quelque chose
            Assert.AreEqual(2,
             monSQLClient.ChargeLOB("select IDTEST,TYPECLOB AS CLOB from JCATest " +
             " where IDTEST in ('LOBUT1_2','LOBUT1_3')",
             FichierCLOB),
             "ChargerLOB aurait du retourner 2 comme nombre de rangées affectées");

            Assert.IsTrue(
            monSQLClient.AssertSQL(CompteCLOBApres, 3),
            "erreur il ne devrait rester aucun clob à l.ancienne valeur id=1012");
            

            //Vérifications avec ExporteLOB
            ExporteLOBOK(monSQLClient);
            
        }


        
        [TestMethod]
        public void SQLODPExportLOBPasOK()
        {
            Assert.Fail("Pas encore implémenté"); 
        }

        /// <summary>
        /// Cas de LOB qui gémèrent des exceptions
        /// </summary>
        [TestMethod]
        public void SQLODPChargeLOBPasOK()
        {
            String FichierBLOB = Chemin + "BLOB.jpg";
            
            // Select de LOB incorrect (sans 'alias)
            try {
                 monSQLClient.ChargeLOB("select idtest,typeblob from JCATest", FichierBLOB);
                } catch (JCASQLODPException excep)
                {
                    Assert.IsTrue(excep.Message.Contains(
                        "Il n'y a aucune colonne identifiée par un alias BLOB ou CLOB dans la commande SQL"),
                        "Le message d'exception attendu n'est pas là " +
                        excep.Message  ); 
                }
            // Select de LOB incorrect (sans info de clé)
            try
            {
                monSQLClient.ChargeLOB("select typeblob as blob, info from JCATest", FichierBLOB);
            }
            catch (JCASQLODPException excep)
            {
                Assert.IsTrue(excep.Message.Contains(
                    "La commande doit avoir la forme 'select colonnedecle, colonneblob as blob from table' "),
                    "Le message d'exception attendu n'est pas là " +
                    excep.Message);
            }

            // Select de LOB incorrect (sql invalide)
            try
            {
                monSQLClient.ChargeLOB("select xx xx typeblob as blob deom deom, info from JCATest", FichierBLOB);
            }
            catch (JCASQLODPException excep)
            {
                Assert.IsTrue(excep.Message.Contains(
                    "ORA-00923"),
                    "Le message d'exception attendu n'est pas là " +
                    excep.Message);
            }

            // Select de LOB correct nom de fichier a null
            try
            {
                monSQLClient.ChargeLOB("select idtest, typeblob as blob  from JCATest", 
                    null );
            }
            catch (JCASQLODPException excep)
            {
                Assert.IsTrue(excep.Message.Contains(
                    "Le nom de fichier doit contenir un nom de fichier"),
                    "Le message d'exception attendu n'est pas là " +
                    excep.Message);
            }

            // Select de LOB correct nom de fichier a chaine vide
            try
            {
                monSQLClient.ChargeLOB("select idtest, typeblob as blob  from JCATest",
                    "");
            }
            catch (JCASQLODPException excep)
            {
                Assert.IsTrue(excep.Message.Contains(
                    "Le nom de fichier doit contenir un nom de fichier"),
                    "Le message d'exception attendu n'est pas là " +
                    excep.Message);
            }

            // Select de LOB correct nom de fichier invalide
            try
            {
                monSQLClient.ChargeLOB("select idtest, typeblob as blob  from JCATest",
                    "cd::\\\\\\oascvalide");
            }
            catch (JCASQLODPException excep)
            {
                Assert.IsTrue(excep.Message.Contains(
                    "Le fichier à charger sur la base de données n'existe pas ou est invalide Nom du fichier : cd::\\\\\\oascvalide"),
                    "Le message d'exception attendu n'est pas là " +
                    excep.Message);
            }



            
        }

        public void ExporteLOBOK(JCASQLODPClient monSQLClient)
            {
            // Les trois rangées coneiennent FichoerBLOB dans typeblob
            // et FichierCLOB dams ;a colonne type clob

            // Faire le ménage
                Menage("LOBUT1_1.JPG");
                Menage("LOBUT1_2.JPG");
                Menage("LOBUT1_3.JPG");
                Menage("LOBUT1_1.txt");
                Menage("LOBUT1_2.txt");
                Menage("LOBUT1_3.txt");
            // Extraction d un blob avec alias NOM
            Assert.AreEqual(3,     
                monSQLClient.ExporteLOB(
                    "select IDTEST||'.JPG' AS NOM, TYPEBLOB AS BLOB " +
                    " FROM JCATEST WHERE IDTEST LIKE 'LOBUT1_%'",
                    Chemin ),
                    "Pas le bon nombre de blob exporté");
            // Valider que les fichiers blob ont été créés
            Assert.IsTrue(Existe("LOBUT1_1.JPG") &&
                Existe("LOBUT1_2.JPG") &&
                Existe("LOBUT1_3.JPG"),
                "Les 3 fichiers BLOB ne sont pas créés " +
                monSQLClient.DernierResultat);


            //extraction d'un clob sans alias dans le select
            Assert.AreEqual (3, 
                monSQLClient.ExporteLOB(
                        "select IDTEST||'.txt' , TYPECLOB AS CLOB " +
                        " FROM JCATEST WHERE IDTEST LIKE 'LOBUT1_%'",
                        Chemin,
                        Encoding.UTF8  ),
                        "Nauvais nombre de clob exportés"); 

            // Vérifier que les fichiers CLOB existent
            Assert.IsTrue(
                Existe ("LOBUT1_1.txt") &&
                Existe ("LOBUT1_2.txt") &&
                Existe ("LOBUT1_3.txt"),
                "Les 3 fichiers pour le CLOB ne sont pas créés " +
                monSQLClient.DernierResultat  );

            // Valider le contenu des nouveaux fichiers
            Assert.IsTrue(
               UtilitairesUT.FichierTextePareils(FichierCLOB,
               Chemin + "LOBUT1_1.txt"),
               "LOBUT1_1.txt a été altéré ");

            Assert.IsTrue(
               UtilitairesUT.FichierTextePareils(FichierCLOB,
               Chemin + "LOBUT1_2.txt"),
               "LOBUT1_2.txt a été altéré ");
            Assert.IsTrue(
              UtilitairesUT.FichierTextePareils(FichierCLOB,
              Chemin + "LOBUT1_3.txt"),
              "LOBUT1_3.txt a été altéré ");

            Assert.IsTrue(
              UtilitairesUT.FichierBinairePareils(FichierBLOB,
              Chemin + "LOBUT1_2.JPG"),
              "LOBUT1_2.JPG a été altéré ");

            Assert.IsTrue(
              UtilitairesUT.FichierBinairePareils(FichierBLOB,
              Chemin + "LOBUT1_1.JPG"),
              "LOBUT1_1.JPG a été altéré ");


            Assert.IsTrue(
              UtilitairesUT.FichierBinairePareils(FichierBLOB,
              Chemin + "LOBUT1_3.pdf"),
              "LOBUT3_1.pdf a été altéré ");




 
 
 
                  
            }


    }
}
