using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCAXMLUT1
    {
        String Chemin = JCAssertionCore.JCACore.RepertoireAssembly() +
                "Ressources\\";

        [TestMethod]
        public void JCAXMLAssertXPathOK()
        {
            JCAXML monJCAXML = new JCAssertionCore.JCAXML();
            String monFichierTest = Chemin + "XML1.xml";
            Assert.IsTrue(System.IO.File.Exists(monFichierTest)); 
            // Cas 1 il existe au moins un book

            Int64 ResultatReel = 0;
            // Il y a plus de 0 livres
            // Operateur PG par défaut si pêrateur null
            Assert.IsTrue (monJCAXML.AssertXPath (monFichierTest,
                "//book",
                null , 0 , ref  ResultatReel),
                "Aucun livre (book) trouvé " +
                ResultatReel.ToString()  );

            // Il y a plus de 0 livres
            // Operateur >
            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book",
                ">", 0, ref  ResultatReel),
                "Aucun livre (book) trouvé " +
                ResultatReel.ToString());
            

            // il y a au moins 12 livres
            // Operateur pg=
            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book",
                "pg=", 12, ref  ResultatReel),
                " il devrait y avoir au moins 12 livres(book) trouvé " +
                ResultatReel.ToString());

            // il y a au moins 12 livres
            // Operateur >=
            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book",
                ">=", 12, ref  ResultatReel),
                " il devrait y avoir au moins 12 livres(book) trouvé " +
                ResultatReel.ToString());


            // Un livre a un certain titre
            // Operateur =
            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book/title[text()='Midnight Rain']",
                "=", 1, ref  ResultatReel),
                " il devrait y avoir um livres(book) trouvé pour le titre 'Midnight Rain' " +
                ResultatReel.ToString());

            // Un livre a un certain titre, pas deux livres
            // Operateur !=
            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book/title[text()='Midnight Rain']",
                "!=", 2, ref  ResultatReel),
                " il devrait y avoir juste um livres(book) trouvé pour le titre 'Midnight Rain' " +
                ResultatReel.ToString());

            // Un livre a un certain titre, pas deux livres
            // Operateur <>
            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book/title[text()='Midnight Rain']",
                "<>", 2, ref  ResultatReel),
                " il devrait y avoir juste um livres(book) trouvé pour le titre 'Midnight Rain' " +
                ResultatReel.ToString());

            // Opérateur pp
            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book/title[text()='Midnight Rain']",
                "pp", 2, ref  ResultatReel),
                " il devrait y avoir juste um livres(book) trouvé pour le titre 'Midnight Rain' " +
                ResultatReel.ToString());

            // Opérateur pp=
            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book/title[text()='Midnight Rain']",
                "pp=", 1, ref  ResultatReel),
                " il devrait y avoir juste um livres(book) trouvé pour le titre 'Midnight Rain' " +
                ResultatReel.ToString());


            // p^érateur <
            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book/title[text()='Midnight Rain']",
                "<", 2, ref  ResultatReel),
                " il devrait y avoir juste um livres(book) trouvé pour le titre 'Midnight Rain' " +
                ResultatReel.ToString());

            // Opérateur <=

            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book/title[text()='Midnight Rain']",
                "<=", 1, ref  ResultatReel),
                " il devrait y avoir juste um livres(book) trouvé pour le titre 'Midnight Rain' " +
                ResultatReel.ToString());


            // Ub cas qui n'est pas vrai
            Assert.IsFalse(monJCAXML.AssertXPath(monFichierTest,
                "//book/title[text()='Midnight Rain']",
                "<=", 0, ref  ResultatReel),
                " il devrait y avoir juste um livres(book) trouvé pour le titre 'Midnight Rain' " +
                ResultatReel.ToString());


            
        }

        [TestMethod]
        public void JCAXMLAssertXPathExcep()
        {
            Int64 ResultatReel = 0;
            JCAXML monJCAXML = new JCAssertionCore.JCAXML();
            

            // exception fichier inexistant
            try {
                Assert.IsFalse(monJCAXML.AssertXPath(Chemin + "pasla.pasla.rmr",
                "//book/title[text()='Midnight Rain']",
                "<=", 0, ref  ResultatReel),
                " il devrait y avoir juste um livres(book) trouvé pour le titre 'Midnight Rain' " +
                ResultatReel.ToString());
                Assert.Fail("Une erreur aurait du se produire");
            } catch (Exception excep)
                {
                    Assert.IsTrue(excep.Message.Contains("\\pasla.pasla.rmr"),
                        "Mauvais message : " +
                        excep.Message  );
                }

            // test de mauvais opérateur
            String monFichierTest = Chemin + "XML1.xml";
            
            try
            {
                Assert.IsFalse(monJCAXML.AssertXPath(monFichierTest,
                "//book/title[text()='Midnight Rain']",
                "ope", 0, ref  ResultatReel),
                " il devrait y avoir juste um livres(book) trouvé pour le titre 'Midnight Rain' " +
                ResultatReel.ToString());
                Assert.Fail("Une erreur aurait du se produire");
            }
            catch (Exception excep)
            {
                Assert.IsTrue(excep.Message.Contains(
                    "Pour une assertion XPath l'opérateur 'OPE' n'est pas un opérateur valide"),
                    "Mauvais message : " +
                    excep.Message);
            }

            
        }

        [TestMethod]
        public void JCAXMLAssertXPathOK2()
        {
            JCAXML monJCAXML = new JCAssertionCore.JCAXML();
            String monFichierTest = Chemin + "XML1.xml";
            Assert.IsTrue(System.IO.File.Exists(monFichierTest)); 
            // Cas 1 il existe au moins un book

            Int64 ResultatReel = 0;
            // Il y a plus de 0 livres publiés en 2009
            // Operateur PG par défaut si pêrateur null
            Assert.IsTrue (monJCAXML.AssertXPath (monFichierTest,
                "//book/publish_date",
                "PG" , 0 , ref  ResultatReel,
                "2000"),
                "Aucun livre publiés en 2009  (book) trouvé " +
                ResultatReel.ToString()  );
            
            
            // Trouver auteur partiel "Ralls"
            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book/author",
                "=", 1, ref  ResultatReel,
                "Rall"),
                "Aucun livre avec l'auteur partiel  (book) trouvé " +
                ResultatReel.ToString());


            // la case fait que l,on ne trouve rien
            // Trouver auteur partiel "Ralls"avec valeur défaut de sensiblecase

            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book/author",
                "=", 0, ref  ResultatReel,
                "RALl"),
                "Aucun livre avec l'auteur partiel  (book) n'aurait du être  trouvé " +
                ResultatReel.ToString());

            // la case fait que l,on ne trouve rien
            // Trouver auteur partiel "Ralls"avec vaexplicite de sensiblecase

            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book/author",
                "=", 0, ref  ResultatReel,
                "RALl",
                true ),
                "Aucun livre avec l'auteur partiel  (book) n'aurait du être  trouvé " +
                ResultatReel.ToString());

            // la case ne compte plus et on trouve un livre
            
            Assert.IsTrue(monJCAXML.AssertXPath(monFichierTest,
                "//book/author",
                "=", 1, ref  ResultatReel,
                "RALl",
                false ),
                "Aucun livre avec l'auteur partiel  (book) n'a été   trouvé " +
                ResultatReel.ToString());



             

        }

    }
}
