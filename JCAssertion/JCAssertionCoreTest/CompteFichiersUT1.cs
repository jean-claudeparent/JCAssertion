using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml ;

namespace JCAssertionCoreTest
{
    [TestClass]
    public class CompteFichiersUT1
    {
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";


        [TestMethod]
        public void CompteFichiersparxml()
        {
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument();

            // Préparer les variables
            monCore.Variables.MAJVariable(
                "monRepertoire", Chemin);
            monCore.Variables.MAJVariable(
               "monOperateur", "=");
            monCore.Variables.MAJVariable(
               "monResultat", "1");

            monCore.Variables.MAJVariable(
               "moMessageEchech",
               "Ceci est le mssage d'échec provemamt d'une variable ");



            // cas 1 Vrai operateeur = 


            monCas.InnerXml = "<Assertion>" +
                   "<Type>CompteFichiers</Type>" +
                   "<Repertoire>{{monRepertoire}}CompteFichiers</Repertoire>" +
                   "<Operateur>pg{{monOperateur}}</Operateur>" +
                   "<ResultatAttendu>{{monResultat}}</ResultatAttendu>" +
                   "<MessageEchec>{{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";

            Assert.IsTrue(monCore.ExecuteCas(monCas),
                "L'assertion aurait dûe être vraie. " +
                Environment.NewLine +
                monCore.Message + Environment.NewLine +
                monCore.MessageEchec);

            Assert.IsTrue(monCore.Message.Contains(
                "Assertion : 5 (Réel) pg= 1 (Attendu)") &&
            monCore.Message.Contains(
            "Assertion CompteFichiers") &&
            monCore.Message.Contains(
            "Répertoire à traiter :") &&
            monCore.Message.Contains(
            "Pattern des fichiers à compter : *.*") &&
            monCore.Message.Contains("\\CompteFichiers") &&
           (!monCore.Message.Contains("ceci ne vient")),
                "Cas 1 Mauvais contenu de Message : " + Environment.NewLine +
                monCore.Message);


            // cas 2 qui  fait une assertion fausse 
            //  pour vérifier le message d'échec



            monCas.InnerXml = "<Assertion>" +
                   "<Type>CompteFichiers</Type>" +
                   "<Repertoire>{{monRepertoire}}CompteFichiers</Repertoire>" +
                   "<Operateur>=</Operateur>" +
                   "<ResultatAttendu>{{monResultat}}1111</ResultatAttendu>" +
                   "<MessageEchec>{{moMessageEchech}} ceci ne vient pas de la variable.</MessageEchec>" +
                   "</Assertion>";

            Assert.IsFalse(monCore.ExecuteCas(monCas),
                "L'assertion aurait dûe être fausse. " +
                monCore.Message + Environment.NewLine +
                monCore.MessageEchec);

            Assert.IsTrue(monCore.Message.Contains(
                "Assertion : 5 (Réel) = 11111 (Attendu)") &&
            !monCore.Message.Contains(
            "ceci ne vient"),
                "Cas 2 Mauvais contenu de Message : " + Environment.NewLine +
                monCore.Message);

            Assert.IsTrue(monCore.MessageEchec.Contains(
                "Ceci est le mssage d'échec provemamt d'une variable  ceci ne vient pas de la variable."),
                "Cas 2 mauvais message d'échec : " +
                Environment.NewLine + monCore.MessageEchec);


            


        }

    }
}
