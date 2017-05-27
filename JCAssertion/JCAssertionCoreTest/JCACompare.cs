using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;


namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCACompareUT
    {
        [TestMethod]
        public void JCACompare1()
        {
            JCACompare monCompare = new JCACompare();

            Assert.IsTrue( 
                monCompare.Compare(1,"=",1));

            Assert.IsFalse(
                monCompare.Compare(1, "=", 2));

            Assert.IsTrue(
                monCompare.Compare(1, "pg", 0));

            Assert.IsTrue(
                monCompare.Compare(1, "pg=", 1));


            Assert.IsTrue(
                monCompare.Compare(1, ">", 0));

            Assert.IsTrue(
                monCompare.Compare(1, ">=", 1));

            // pp
            Assert.IsTrue(
                monCompare.Compare(1, "pp", 10));

            Assert.IsTrue(
                monCompare.Compare(1, "pp=", 1));


            Assert.IsTrue(
                monCompare.Compare(1, "<", 10));

            Assert.IsTrue(
                monCompare.Compare(1, "<=", 1));
            // !=
            Assert.IsTrue(
                monCompare.Compare(12, "!=", 1));
            Assert.IsTrue(
                monCompare.Compare(12, "<>", 1));
            // operateur invalide
            try
                {
                    Assert.IsTrue(
                        monCompare.Compare(12, "<xx>", 1));

                    Assert.Fail("Une exception aurait du se produire");
                } 
            catch (Exception excep)
            {
                Assert.IsTrue(excep.Message.Contains(
                    "Pour cette comparaison l'opérateur '<XX>' n'est pas un opérateur valide"),
                    "Mauvais messages "+
                    excep.Message  );     
            }


            
        }
    }
}
