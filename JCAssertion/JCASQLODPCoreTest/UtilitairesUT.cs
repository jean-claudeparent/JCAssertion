using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text;

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


    }
}
