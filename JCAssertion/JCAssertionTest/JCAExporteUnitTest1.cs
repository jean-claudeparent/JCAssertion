using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAExporte;

namespace JCAssertionTest
{


    

    [TestClass]
    public class JCAssertionUnitTest
    {
        [TestMethod]
        public void ExportteOKTest()
        {
            JCAExporte.JCAExporteCore monExporte = new JCAExporteCore();
            String[] mesArgs = new String[10];
            String Message;
            String NomFichier = JCAssertionCore.JCACore.RepertoireAssembly() +
                "\\Ressources\\JCAExporteOKCree.xml";
            mesArgs[0] = "/f:" + NomFichier ;
            mesArgs[1] = "/v1:Test1=Valeur1";
            mesArgs[2] = "/v2:Test1=Valeur1";
            mesArgs[3] = "/v3:Test1=Valeur1";
            mesArgs[4] = "/v4:Test1=Valeur1";
            mesArgs[5] = "/v5:Test1=Valeur1";
            mesArgs[6] = "/v1:Duplex";

            Assert.AreEqual(0, monExporte.ExecuteExporte(mesArgs, out Message));
            Assert.Fail("Implémenter la vérif de fichier");

            
        }

        
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
