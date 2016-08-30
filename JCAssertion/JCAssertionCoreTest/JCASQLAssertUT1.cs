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
            // FermerConnection

        }
    


        [TestMethod]
        public void ODPSQLAssertOK()
        {
            // Cas 1 Assert un nombre et retourne true
            monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<SQL>{{dual}}</SQL>" +
               "<AttenduNombre>{{un}}</AttenduNombre>" +
               "</Assertion>";
            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "Échec du cas 1 de de ODPSQLAssertOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec );
            Assert.IsTrue(monCore.Message.Contains(
                "select count(*) from dual"), "Le message devrait contenir select count(*) from dual");
            Assert.IsTrue(monCore.Message.Contains(
                "Opérateur : ="),
                "Le message devrait contenir Opérateur : =");
            Assert.IsTrue(monCore.Message.Contains(
                "Valeur attendue : 1"),
                "Le message devrait contenir Valeur attendue : 1");


            // Cas 2 Assert un nombre et retourne false
            monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<SQL>{{dual}}</SQL>" +
                "<Operateur>pg</Operateur>" +
               "<AttenduNombre>{{un}}</AttenduNombre>" +
               "<MessageEchec>{{echec}}</MessageEchec>" +
               "</Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "Échec du cas 2 de de ODPSQLAssertOK(). true attendu " +
                monCore.Message +
                " " + monCore.MessageEchec);

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Ceci est le message d'échec de test."),
                "Le message d'échec aurait du contenir :" 
                + " Ceci est le message d'échec de test. Contient : " +
                monCore.MessageEchec  );


            // Cas 3 Assert un texte et retourne true
            monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<SQL>select '{{Test}}'" +
               Environment.NewLine   +"from dual</SQL>" +
               "<AttenduTexte>{{Test}}</AttenduTexte>" +
               "</Assertion>";
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

            // Cas 4 Assert un texte et retourne false
            monCas.InnerXml = "<Assertion>" +
               "<Type>AssertSQL</Type>" +
               "<SQL>select '{{Test}}'" +
               Environment.NewLine + "from dual</SQL>" +
               "<AttenduTexte>Test pas pareil</AttenduTexte>" +
               "<MessageEchec>{{echec}}</MessageEchec>" +
               "</Assertion>";
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

            Assert.IsTrue(monCore.MessageEchec.Contains (
                "Ceci est le message d'échec de test"),
                "Cas 4 Le message d'échec devrait contenir :Ceci est le message d'échec de test: mais contient :" +
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
