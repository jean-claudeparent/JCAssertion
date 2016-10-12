using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCAssertionCore;

namespace JCAssertionCoreTest
{
    [TestClass]
    public class JCASQLExecuteUT1
    {
        JCACore monCore = new JCACore();
            
        [TestInitialize]
        public void InitTest()
        {
            // remplir les variables
            monCore.Variables.MAJVariable("NomTable","JCATest");
            monCore.Variables.MAJVariable("Egal", "­=");
            monCore.Variables.MAJVariable("Colonne", "INFO");
            monCore.Variables.MAJVariable("cleCas", "UT1SEXML1");
             
        }
        /// <summary>
        /// SQLExecutexml Test ;e SQ:Execute avec l'interface XML
        /// </summary>
        [TestMethod]
        public void SQLExecutexml()
        {
            Assert.Fail("Pas encore implémentée");
        }
    }
}
