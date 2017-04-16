using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCASQLODPCore;


namespace JCASQLODPCoreTest
{
    [TestClass]
    public class HelperUnitTest1
    {
        [TestMethod]
        public void ConvertirByteArray()
        {
            JCASQLODPHelper monHelper = new JCASQLODPHelper();
            Byte[] Resultat;

            // type string
            String TypeString = "";
            Resultat = monHelper.ConvertirByteArray(TypeString);
            Assert.IsFalse(Resultat == null,
                "Le résultat ne devrait pas être null");
            Assert.AreEqual(0,
                Resultat.Length,
                "Le byte array aurait du avoir 0 octets"); 
            
 
 
            
            TypeString = "Testé";
            Resultat = monHelper.ConvertirByteArray(TypeString);
            Assert.IsFalse(Resultat == null ,
                "Le résultat ne devrait pas être null"); 
            Assert.AreEqual(5,
                Resultat.Length,
                "Le byte array aurait du avoir 5 octets"); 
            // type double
            Double  TypeDouble = 14.23;
            Resultat = monHelper.ConvertirByteArray(TypeDouble);
            

            // type int32
            Int32 TypeInt32 = 12;
            Resultat = monHelper.ConvertirByteArray(TypeInt32);
            
 
 
             
        }
    }
}
