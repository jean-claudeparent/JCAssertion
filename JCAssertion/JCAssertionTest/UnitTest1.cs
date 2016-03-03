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

            // pas assez d'argument Cas 2
            Message = null;
            String[] mesArgs1 = new String[1];
            mesArgs1[0] = "/v1:Test=ok";
            try {
              Assert.AreEqual(99, monExporte.ExecuteExporte(mesArgs1, out Message));
            } catch (Exception excep)
            {
                Assert.Fail("Le cas 2 de ExporttePasOKTest a l'exception : " + excep.Message  );
            }
                Assert.IsTrue(Message.ToUpper().Contains("USAGE"), "Le message devrait donner l'usage du programme : " + Message);

            

            // assez d'argument mais pas d'argument fichier
                Message = null;
                String[] mesArgs2 = new String[3];
                mesArgs2[1] = "/v1:Test=ok";
                mesArgs2[2] = "/v2:Test2=ok";
                Assert.AreEqual(99, monExporte.ExecuteExporte(mesArgs2, out Message));
            
            
            // Nom de fichier invalide
                mesArgs2[0] = "/f:z:::;";
                try
                {
                    Assert.AreEqual(99, monExporte.ExecuteExporte(mesArgs2, out Message));
                } catch (Exception excep)
            {
                Assert.IsTrue(excep.Message.Contains ("hemin"));
            }
          }
    }
}
