using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertion;


namespace JCAssertionTest
{
    [TestClass]
    public class JCAssertionUnitTest
    {
        [TestMethod]
        public void JCAssertionTest1()
        {
            JCAssertion.JCAssertion monProgramme = new JCAssertion.JCAssertion ();
            monProgramme.Popup = false;
            Assert.IsTrue(monProgramme.gettxbActivite().Contains ("Démarrage")  );
            // cas sans argument
            String[] argsvide = new String[0];
            Assert.AreEqual (99, monProgramme.Execute ());


            // avec argument mais /FA

            monProgramme.args = new String[2] ;
            monProgramme.args[0] = "/FV:test";
            monProgramme.args[1] = "/ab:test";
            Assert.AreEqual(99, monProgramme.Execute());

            // cas qui marche
            monProgramme.args[0] = "/FV:test";
            monProgramme.args[1] = "/fa:test";
            Assert.AreEqual(0, monProgramme.Execute());



        }
    }
}
