using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
 

namespace JCASQLODPCoreTest
{
    /// <summary>
    /// Classe définissant des méthodes utiles
    /// pour les unit test
    /// </summary>
    public  class UtilitairesUT
    {
        /// <summary>
        /// Retourne si le contenu textuel de deux fichiers est 
        /// pareil, si les formats d'encodage sont différents
        /// ou si la taille est différente cela ne cause pas une 
        /// différence
        /// </summary>
        /// <param name="NomFichier1"></param>
        /// <param name="NomFichier2"></param>
        /// <returns></returns>
        public static  Boolean FichierTextePareils(
            String NomFichier1,
            String NomFichier2)
            {
                return (System.IO.File.ReadAllText(NomFichier1) ==
                    System.IO.File.ReadAllText(NomFichier2));
            }

        public static Boolean FichierBinairePareils(
            String NomFichier1,
            String NomFichier2)
        {
            Byte[] Contenu1 = System.IO.File.ReadAllBytes(NomFichier1);
            Byte[] Contenu2 = System.IO.File.ReadAllBytes(NomFichier2);
            if (Contenu1.Count() != Contenu2.Count() )
                return false;
            for (Int64 i = 0; i < Contenu1.Count(); i++)
            {
                if (Contenu1[i] != Contenu2[i] )
                    return false;
            }
            return true ;
            
        }


    }
}
