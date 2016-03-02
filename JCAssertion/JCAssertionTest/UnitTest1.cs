using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAExporte;

namespace JCAssertionTest
{
    [TestClass]
    public class JCAssertionUnitTest
    {
        [TestMethod]
        public void ExporttePasOKTest()
        {
            // pasdMargument
            JCAExporte.JCAExporteCore monExporte = new JCAExporteCore();
            String[] mesArgs = new String[0];
            String Message;
            
            Assert.AreEqual(99, monExporte.ExecuteExporte(mesArgs , out Message ));
            Assert.IsTrue(Message.ToUpper().Contains("USAGE") ,"Le message devrait donner l'usage du programme : " + Message );

            // oas assez d'argument
            Message = null;
            String[] mesArgs1 = new String[1];
            mesArgs1[1] = "/v1:Test=ok";
            Assert.AreEqual(99, monExporte.ExecuteExporte(mesArgs1, out Message));
            Assert.IsTrue(Message.ToUpper().Contains("USAGE"), "Le message devrait donner l'usage du programme : " + Message);

            

            // assez d'argument mais pas d'argument fichier



        }
    }
}
