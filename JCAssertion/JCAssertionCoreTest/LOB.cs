using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using JCAssertionCore;
using JCAMC;
using System.Text;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class LOB
    {

        XmlDocument monCas;
        JCACore monCore;
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";

        /// <summary>
        /// Nettoyer et renettre la base de données dans son état avant test
        /// pour le unit test 1
        /// </summary>
        public void InitBD()
            {
                monCas.InnerXml = "<Assertion>" +
                   "<Type>SQLExecute</Type>" +
                   "<SQL>delete from JCATest where idtest like 'JCACT.LOB_%'</SQL>" +
                   "<SQL>INSERT INTO JCATest(IDTEST) VALUES ('JCACT.LOB_1')</SQL>" +
                   "<SQL>INSERT INTO JCATest(IDTEST) VALUES ('JCACT.LOB_2')</SQL>" +
                   "<SQL>INSERT INTO JCATest(IDTEST) VALUES ('JCACT.LOB_3')</SQL>" +
                   "</Assertion>";

                if (!monCore.ExecuteCas(monCas))
                    throw new Exception(
                        "Erreur d'initialisation " +
            monCore.Message + " " +
            monCore.MessageEchec );
            
            }
        
        public Boolean AssertSQL(
            String SQL,
            Int32 Valeur)
            {
                // monCore doit être setup par une autre méthode
                monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<SQL>" + SQL + "</SQL>" +
               "<AttenduNombre>" + Valeur.ToString() + "</AttenduNombre>" +
               "</Assertion>";
                return monCore.ExecuteCas(monCas);  

                
            }

        [TestInitialize]
        public void InitTest()
        {
            // Définir connection
            monCas = new XmlDocument();
            monCas.InnerXml = "<Assertion>" +
               "<Type>ConnectionOracle</Type>" +
               "<User>JCA</User>" +
               "<Password>JCA</Password>" +
               "</Assertion>";

            monCore = new JCACore();
            monCore.ExecuteCas(monCas);
            monCore.Variables.MAJVariable(
                "Fichier", Chemin);
            


        }

        
 

        [TestCleanup]
        public void CleanTest()
        {
            
            // La connection s'étant fermée à chaque assertionn
            // ps besoin de ;a fermer
            
        }
    

        [TestMethod]
        public void JCACoreLOBOKUT1()
        {
            InitBD();
            monCore.Variables.MAJVariable("Where",
                " idtest='JCACT.LOB_1'");  
            // Lancer un test où les assertions passent
            monCas.InnerXml = "<Assertion>" +
               "<Type>ChargeLOB</Type>" +
               "<Fichier>{{Fichier}}pdf.pdf</Fichier>" +
               "<SQL>select idtest, typeblob AS BLOB from JCATest"+
               " where {{Where}}</SQL>" +
               "</Assertion>";

            if (!monCore.ExecuteCas(monCas))
                throw new Exception("Échec de l'assertion "+
            monCore.Message + 
            monCore.MessageEchec  );

            
            Assert.IsTrue(monCore.Message.Contains("SQL de spécification des rangées à charger select idtest, typeblob AS BLOB from JCATest where  idtest='JCACT.LOB_1'"),
                "Attendu : SQL de spécification des rangées à charger select idtest, typeblob AS BLOB from JCATest where  idtest='JCACT.LOB_1'");
            Assert.IsTrue(monCore.Message.Contains("Fichier dont le contenu sera chargé :"),
                "Attendu : Fichier dont le contenu sera chargé :");
            Assert.IsTrue(monCore.Message.Contains("1 rangée affectée"),
                "Attendu : 1 rangée affectée");
            Assert.IsTrue(AssertSQL(
                "select count(*) from JCATest where idtest like 'JCACT.LOB_%' and typeblob is not null", 1),
                "Le nombre de blob not null n'est pas bon " + monCore.Message + " " +
                monCore.MessageEchec );
            // Avant clob
            Assert.IsTrue(AssertSQL(
                "select count(*) from JCATest where "+
                " idtest like 'JCACT.LOB_%' and typeclob is not null", 0),
                "Le nombre de clob not null n'est pas bon " + monCore.Message + " " +
                monCore.MessageEchec);
            
            Assert.IsTrue(AssertSQL(
                "select count(*) from JCATest where idtest like 'JCACT.LOB_%' and typeblob is not null", 1),
                "Le nombre de blob not null n'est pas bon " + monCore.Message + " " +
                monCore.MessageEchec );


            // Cas pour le clob
            Assert.IsTrue(AssertSQL(
                "select count(*) from JCATest where"+
                " idtest like 'JCACT.LOB_%' and typeclob is not null", 0),
                "Le nombre de clob not null n'est pas bon " + monCore.Message + " " +
                monCore.MessageEchec);

            monCas.InnerXml = "<Assertion>" +
               "<Type>ChargeLOB</Type>" +
               "<Fichier>{{Fichier}}SQLOK.txt</Fichier>" +
               "<SQL>select idtest, typeclob AS CLOB from JCATest" +
               " where {{Where}}</SQL>" +
               "</Assertion>";

            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "L'assertion aurait due être vraie " +
                monCore.Message );

            Assert.IsTrue(AssertSQL(
                "select count(*) from JCATest where" +
                " idtest like 'JCACT.LOB_%' and typeclob is not null", 1),
                "Le nombre de clob not null n'est pas bon " + monCore.Message + " " +
                monCore.MessageEchec);

            // Retester avec l'exportation de LOB
            RxporteLOBVerif();
            
                
        }

        private void RxporteLOBVerif()
            {
            // test blob
                monCas.InnerXml = "<Assertion>" +
               "<Type>ExporteLOB</Type>" +
               "<Chemin>{{Fichier}}</Chemin>" +
               "<SQL>select idtest||'.pdf' as NOM, typeblob AS BLOB from JCATest" +
               " where {{Where}}</SQL>" +
               "</Assertion>";

                Assert.IsTrue (monCore.ExecuteCas(monCas),
                    "L'assertion est en échec : "+
                    monCore.Message + " " +
                    monCore.MessageEchec );
                Assert.IsTrue(monCore.Message.Contains(
                    "1 rangée exportée.") &&
                    monCore.Message.Contains(
                    "SQL de spécification des rangées à exporter select "),
                    "Mauvais message : " +
                    monCore.Message  );

                Assert.IsTrue(monCore.Message.Contains(
                        "Noms de fichier provenant de la colonne NOM") &&
                        monCore.Message.Contains(
                        "JCACT.LOB_1.pdf"),
                        "Mauvais message : " +
                        monCore.Message);

                Assert.IsTrue(
                    System.IO.File.Exists (Chemin +
                    "JCACT.LOB_1.pdf"), "Fichier pas créé " +
                    monCore.Message    );  

            // test clob utf8 
                monCas.InnerXml = "<Assertion>" +
                   "<Type>ExporteLOB</Type>" +
                   "<Chemin>{{Fichier}}</Chemin>" +
                   "<SQL>select idtest||'.UTF8.txt' as NOM, typEClob AS CLOB from JCATest" +
                   " where IDTEST IN "+
                   "('JCACT.LOB_1','JCACT.LOB_2','JCACT.LOB_3' )</SQL>" +
                   "</Assertion>";

                Assert.IsTrue(monCore.ExecuteCas(monCas),
                    "L'assertion est en échec : " +
                    monCore.Message + " " +
                    monCore.MessageEchec);

                Assert.IsTrue(monCore.Message.Contains(
                    "1 rangée exportée.") &&
                    monCore.Message.Contains(
                    "JCACT.LOB_1.UTF8.txt"),
                    "Mauvais message : " +
                    monCore.Message);
                Assert.IsTrue(JCAMiniCore.TypeEncodage(Chemin + "JCACT.LOB_1.UTF8.txt") ==
                Encoding.UTF8  ,
                "Le fichier devrait être en utf8 : JCACT.LOB_1.UTF8.txt");

                // test clob ascii 
                monCas.InnerXml = "<Assertion>" +
                   "<Type>ExporteLOB</Type>" +
                   "<Chemin>{{Fichier}}</Chemin>" +
                   "<SQL>select idtest||'.ascii.txt' as NOM, typEClob AS CLOB from JCATest" +
                   " where IDTEST IN " +
                   "('JCACT.LOB_1','JCACT.LOB_2','JCACT.LOB_3' )</SQL>" +
                   "<Encodage>Ascii</Encodage>" +
                   "</Assertion>";

                Assert.IsTrue(monCore.ExecuteCas(monCas),
                    "L'assertion est en échec : " +
                    monCore.Message + " " +
                    monCore.MessageEchec);
                Assert.IsTrue(JCAMiniCore.TypeEncodage(Chemin + 
                    "JCACT.LOB_1.ascii.txt") ==
                    Encoding.ASCII ,
                    "Le fichier devrait être en ascii : JCACT.LOB_1.asciitxt");

            // test clob ansi
            // test aucune rangée retournée par le select

            Assert.Fail("Pas encore implémenté exportelob ");
            }

    }
}
