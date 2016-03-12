using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;
using System.Xml;



namespace JCAssertionCoreTest
{
    [TestClass]
    public class ExecuteCasTest
    {
        [TestMethod]
        public void FichierExiste()
        {
            // testc abec variablenon fournie
            JCACore monCore = new JCACore();
            XmlDocument monCas = new XmlDocument () ;

            monCas.InnerXml = "<Assertion><Type>FichierExiste</Type><Fichier>{{Fichier}}</Fichier></Assertion>";
            Assert.IsFalse(monCore.ExecuteCas(monCas ));
            Assert.IsTrue (monCore.Message.Contains("La variable Fichiern'a pas eu de valeur fournie"), "Attendu:La variable Fichiern'a pas eu de valeur fournie");

            // variable fournie mais fichier existe pas
            monCore.Variables.MAJVariable("Fichier","DivideByZeroException:existepas.pasla");
            // Assert.IsFalse(monCore.ExecuteCas (lecas ) );
                

        }
    }
}
