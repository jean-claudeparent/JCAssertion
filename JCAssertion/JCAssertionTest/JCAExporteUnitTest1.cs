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
            if (System.IO.File.Exists(NomFichier)) System.IO.File.Delete(NomFichier );
            Assert.IsFalse(System.IO.File.Exists (NomFichier ));

            mesArgs[0] = "/f:" + NomFichier ;
            mesArgs[1] = "/v1:Test1=Valeur1";
            mesArgs[2] = "/v2:Test2=Valeur2";
            mesArgs[3] = "/v3:Test3=Valeur3";
            mesArgs[4] = "/v4:Test4=Valeur4";
            mesArgs[5] = "/v:Test5=Valeur5";
            mesArgs[6] = "/v655:Duplex";

            Assert.AreEqual(0, monExporte.ExecuteExporte(mesArgs, out Message));
            String Contenu = System.IO.File.ReadAllText(NomFichier );
            Assert.IsFalse (Contenu.Contains("AjoutApres"),"Le fichier ne devrai pas contenuui la valeur à ajouter dansleprochain test.");
            Assert.IsTrue (Contenu.Contains ("Variable Cle=\"Duplex\" Valeur=\"Duplex\""),"La cle Duplex ou la valeur Duplex ne sont pas dans le fichier");
            Assert.IsTrue(Contenu.Contains("Variable Cle=\"Test1\" Valeur=\"Valeur1\""),"La clé Test1 ou la valeurValeur1 ne sont pas dans le fichier");
            Assert.IsTrue(Contenu.Contains("Variable Cle=\"Test2\" Valeur=\"Valeur2\""));
            Assert.IsTrue(Contenu.Contains("Variable Cle=\"Test3\" Valeur=\"Valeur3\""));
            Assert.IsTrue(Contenu.Contains("Variable Cle=\"Test4\" Valeur=\"Valeur4\""));
            Assert.IsTrue(Contenu.Contains("Variable Cle=\"Test5\" Valeur=\"Valeur5\""),"Test5 n'est pas dans le fichier");
            
            // maj le fichier existant
            mesArgs = new String[2];
            mesArgs[1] = "/F:" + NomFichier ;
            mesArgs[0] = "/v:AjoutApres";
            Assert.AreEqual(0, monExporte.ExecuteExporte(mesArgs, out Message));
            Contenu = System.IO.File.ReadAllText(NomFichier);

            Assert.IsTrue (Contenu.Contains("AjoutApres"),"La valeur n'a pas  étéajoutée au fichier de sortie");
            Assert.IsTrue(Contenu.Contains("Variable Cle=\"Duplex\" Valeur=\"Duplex\""));
            Assert.IsTrue(Contenu.Contains("Variable Cle=\"Test1\" Valeur=\"Valeur1\""));
            Assert.IsTrue(Contenu.Contains("Variable Cle=\"Test2\" Valeur=\"Valeur2\""));
            Assert.IsTrue(Contenu.Contains("Variable Cle=\"Test3\" Valeur=\"Valeur3\""));
            Assert.IsTrue(Contenu.Contains("Variable Cle=\"Test4\" Valeur=\"Valeur4\""));
            Assert.IsTrue(Contenu.Contains("Variable Cle=\"Test5\" Valeur=\"Valeur5\""));
            

            
            
            
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
