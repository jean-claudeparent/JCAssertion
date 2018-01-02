using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml;

namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCASQLAssertUT1
    {
        XmlDocument monCas;
        JCACore monCore;
        JCAXMLHelper monXMLH = new JCAXMLHelper();


        [TestInitialize]
        public void InitTest()
        {
            // Définir connection
            // JCAXMLHelper monXMLH = new JCAXMLHelper();
            monCas = new XmlDocument();
            monCas.InnerXml = monXMLH.xmlConnectionOracle( 
                "JCA","JCA"); 
 
            monCore = new JCACore();
            monCore.ExecuteCas(monCas);
            monCore.Variables.MAJVariable(
                "dual","select count(*) from dual");
            monCore.Variables.MAJVariable(
                "un","1");
            monCore.Variables.MAJVariable(
                "echec", "Ceci est le message d'échec de test.");
            monCore.Variables.MAJVariable(
                "Test", "Valeur de test");
            
            

        }

        [TestCleanup]
        public void CleanTest()
        {
            // tester dispose
            monCore.Dispose();
            // Vérifier que plusieurs dispose
            // me causemt pas d'exception
            monCore.Dispose();
            monCore.Dispose();


        }

        /// <summary>
        /// Assert un nombre et retourne false.
        /// Utilise les variables définies automatiquement.
        /// </summary>
        [TestMethod]
        public void SQLAssertNombreFalse()
        {

            monCas.InnerXml = monXMLH.xmlAssertSQL(
               "{{dual}}",
               "pg",
               "{{un}}",
               null,
               "{{echec}}" +Environment.NewLine +
               "Valeur attendue : {{JCA.ValeurAttendue}}" + Environment.NewLine +
               "Valeur réelle : {{JCA.ValeurReelle}}" + Environment.NewLine +
               "Expresssion : {{JCA.Expression}}");
            
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Échec du cas 2 de de ODPSQLAssertOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec);

            Assert.IsTrue(
                monCore.Message.Contains(
                "Valeur réelle : 1"),
                "erreurr attendu Valeur réelle : 1 mais " +
                monCore.Message);

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Ceci est le message d'échec de test."),
                "Le message d'échec aurait du contenir :"
                + " Ceci est le message d'échec de test. Contient : " +
                monCore.MessageEchec);

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Expresssion : (Valeur Réelle):1 pg 1 :(Valeur attendue)") &&
                monCore.MessageEchec.Contains(
                    "Valeur attendue : 1") &&
                    monCore.MessageEchec.Contains(
                        "Valeur réelle : 1"),
                        "Mauvais message d'échec : " +
                        monCore.MessageEchec ); 

             
        }

        /// <summary>
        /// Fait un SQLAssert avec
        /// un résultat de type texte
        /// et retourne faux.
        /// Les 3 variables auto générées
        /// sont utilisées dans le messahe 
        /// d.erreur (MessageEchec).
        /// </summary>
        [TestMethod]
        public void SQLAssertTexteFalse()
        {

            monCas.InnerXml = monXMLH.xmlAssertSQL(
               "select '{{Test}}'" +
               Environment.NewLine + "from dual",
               null, null,
               "Test pas pareil",
               "{{echec}}" + Environment.NewLine +
               "Valeur attendue : {{JCA.ValeurAttendue}}" + Environment.NewLine +
               "Valeur réelle : {{JCA.ValeurReelle}}" + Environment.NewLine +
               "Expresssion : {{JCA.Expression}}");
            
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Échec du cas 4 de de ODPSQLAssertOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec);

            Assert.IsTrue(monCore.Message.Contains(
                "select 'Valeur de test'"),
                "Cas 4 Le message devrait contenir select 'Valeur de test'" +
                " mais contient : " +
                monCore.Message);
            Assert.IsTrue(monCore.Message.Contains(
                "Valeur attendue : Test pas pareil"),
                "Cas 4 Le message devrait contenir Valeur attendue : Test pas pareil" +
                " mais contient : " +
                monCore.Message);

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Ceci est le message d'échec de test"),
                "Cas 4 Le message d'échec devrait contenir :Ceci est le message d'échec de test: mais contient :" +
                monCore.MessageEchec);


            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Expresssion : (Valeur Réelle):\"Valeur de test\" = \"Test pas pareil\" :(Valeur attendue)") &&
                monCore.MessageEchec.Contains(
                    "Valeur attendue : Test pas pareil") &&
                    monCore.MessageEchec.Contains(
                        "Valeur réelle : Valeur de test"),
                        "Mauvais message d'échec : " +
                        monCore.MessageEchec);

            Assert.IsTrue (
                monCore.Message.Contains(
                    "Valeur réelle : Valeur de test"),   
                monCore.Message  ); 

        }




        [TestMethod]
        public void AssertSQLTexteTrue()
        {

            // Cas : Assert un texte et retourne true
            monCas.InnerXml = monXMLH.xmlAssertSQL(
                "select '{{Test}}'" +
               Environment.NewLine + "from dual",
                null,
                null,
               "{{Test}}",
               null);
            
            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "Échec du cas 3 de de ODPSQLAssertOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec);
            
            Assert.IsTrue(monCore.Message.Contains(
                "select 'Valeur de test'"),
                "Cas 3 Le message devrait contenir select 'Valeur de test'" +
                " mais contient : " +
                monCore.Message  );
            Assert.IsTrue(monCore.Message.Contains(
                "Valeur attendue : Valeur de test"),
                "Cas 3 Le message devrait contenir Valeur attendue : Valeur de test" +
                " mais contient : " +
                monCore.Message );

            Assert.IsTrue(monCore.MessageEchec == "",
                "Cas 3 Le message d'échec devrait être vide mais contient :" +
                monCore.MessageEchec);

            


            
           
        }

        [TestMethod]
        public void SQLAssertNombreTrue()
        {
            // Cas : Assert un nombre et retourne true
            monCas.InnerXml = monXMLH.xmlAssertSQL(
                "{{dual}}",
                null,
                "{{un}}",
                null);
            // Les variables générées automatiquement ne devraient pas exister
            Assert.IsFalse(monCore.Variables.CleExiste(
                "JCA.Expression"));
            Assert.IsFalse(monCore.Variables.CleExiste(
                "JCA.ValeurAttendue"));
            Assert.IsFalse(monCore.Variables.CleExiste(
                            "JCA.ValeurReelle"));


            // exécution de l'assertion
            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "Échec du cas 1 de de ODPSQLAssertOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec);
            // Assertions
            // Les variables générées automatiquement ne devraient pas exister
            Assert.IsTrue(monCore.Variables.CleExiste(
                "JCA.Expression"));
            Assert.IsTrue(monCore.Variables.CleExiste(
                "JCA.ValeurAttendue"));
            Assert.IsTrue(monCore.Variables.CleExiste(
                            "JCA.ValeurReelle"));


            Assert.IsTrue(monCore.Message.Contains(
                "select count(*) from dual"), "Le message devrait contenir select count(*) from dual");
            Assert.IsTrue(monCore.Message.Contains(
                "Opérateur : ="),
                "Le message devrait contenir Opérateur : =");
            Assert.IsTrue(monCore.Message.Contains(
                "Valeur attendue : 1"),
                "Le message devrait contenir Valeur attendue : 1");



            
            Assert.IsTrue(monCore.MessageEchec == "",
                "Cas 3 Le message d'échec devrait être vide mais contient :" +
                monCore.MessageEchec);
            
        }


        [TestMethod]
        public void ODPSQLAssertPasOK()
        {
            // Cas 1 Aucun résultat attendu spécifié
            monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<SQL>{{dual}}</SQL>" +
                "<Operateur>pg</Operateur>" +
               "<MessageEchec>{{echec}}</MessageEchec>" +
               "</Assertion>";
            
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Échec du cas 1 de de ODPSQLAssertPasOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec);

            Assert.IsTrue(
                monCore.MessageEchec.Contains(
                "Une des deux balises suivantes doit exister :AttenduNombre ou AttenduTexte"),
                "Cas 1 Le MessageEchec ne contient pas le message atttendu : " +
                monCore.MessageEchec  );

            // Cas 2 deux résultat attendu spécifiés
            monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<SQL>{{dual}}</SQL>" +
               "<AttenduNombre>12</AttenduNombre>" +
               "<AttenduTexte>pg</AttenduTexte>" +
               "<Operateur>pg</Operateur>" +
               "<MessageEchec>{{echec}}</MessageEchec>" +
               "</Assertion>";

            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Échec du cas 2 de de ODPSQLAssertPasOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec);



            Assert.IsTrue(
                monCore.MessageEchec.Contains(
                "Une seule des deux balises suivantes doit exister dans le xml :AttenduNombre ou AttenduTexte"),
                "Cas 2 Le MessageEchec ne contient pas le message atttendu : " +
                monCore.MessageEchec);

            // Cas 3 Pas de balise sql
            monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<AttenduNombre>12</AttenduNombre>" +
               "<Operateur>pg</Operateur>" +
               "<MessageEchec>{{echec}}</MessageEchec>" +
               "</Assertion>";

            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Échec du cas 3 de de ODPSQLAssertPasOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec);



            Assert.IsTrue(
                monCore.MessageEchec.Contains(
                "Le XML ne contient pas la balise SQL"),
                "Cas 3 Le MessageEchec ne contient pas le message atttendu : " +
                monCore.MessageEchec);

            // Cas 4 sql invalide
            monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<AttenduNombre>12</AttenduNombre>" +
               "<Operateur>pg</Operateur>" +
               "<SQL>select pg where true</SQL>" +
               "<MessageEchec>{{echec}}</MessageEchec>" +
               "</Assertion>";

            try 
            { 
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Échec du cas 3 de de ODPSQLAssertPasOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec);
            } catch (Exception excep)
            {
                Assert.IsTrue(excep.Message.Contains("ORA-"),
                    "Cas 4 un erreur oracle aurait du seproduire.");     
            }



            
            
             
        }
        
    }
}
