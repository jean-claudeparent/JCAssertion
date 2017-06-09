using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JCAMC
{
    /// <summary>
    /// Le mini core contient des fonctions
    /// qui ont juste des dépendances
    /// avec le framework .net
    /// </summary>
    public class JCAMiniCore
    {
        
        /// <summary>
        /// Retourne l'encodage d'un fichier texte
        /// en se basant sur le BOM (octets d'identification au début du fichier)
        /// Si le BOM est manquant ou inconnu ASCII est assumé
        /// </summary>
        /// <param name="Fichier">Mom du fichier pour lequel il faut déterminer l'encodage</param>
        /// <returns>Type d'encodage</returns>
        public static Encoding TypeEncodage(
            String Fichier)
            {
                
            var bom = new byte[4];

            using (var file = new FileStream(Fichier, FileMode.Open, FileAccess.Read))

    {

        file.Read(bom, 0, 4);

    }

    // Analyser le BOM

    if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) 
        return Encoding.UTF7;

    if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) 
        return Encoding.UTF8;
    if (bom[0] == 0xff && bom[1] == 0xfe) 
        return Encoding.Unicode;
    if(bom[0] == 0xfe && bom[1] == 0xff) 
        return Encoding.BigEndianUnicode;
 

    if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) 
        return Encoding.UTF32;

    return Encoding.ASCII;




                
            } // Encodage

        /// <summary>
        /// Efface un fichier s'il existe
        /// </summary>
        /// <param name="NomFichier">Nom du fichier à efface avec son chemin</param>
        public static void EffaceFichier(
            String NomFichier)
            {
                if (System.IO.File.Exists(NomFichier ))
                    System.IO.File.Delete(NomFichier );   
            }

        /// <summary>
        /// Remplace les catactères invalides dans
        /// un nom de fichier par des soulignées
        /// Exemple "Fichier:01" deviendra "Fichier_-1"
        /// </summary>
        /// <param name="NomFichier">Nom du fichier à rendre conforme</param>
        /// <returns>Nom compatible avec le système de fichier de windows</returns>
        public static String NomValide(
            String NomFichier) 
            {
                String Resultat = "";
                Resultat = NomFichier.Replace(":","_");
                Resultat = Resultat.Replace("*","_");
                Resultat = Resultat.Replace("?", "_");
                Resultat = Resultat.Replace("\\", "_");
                Resultat = Resultat.Replace("\"", "_");
                
                Resultat = Resultat.Replace("|", "_");
                
                return Resultat; 
            }

        public static Int64 CompteFichiers(
            String Repertoire,
            String  Pattern = "*.*")
            {
                if(System.IO.Directory.Exists(Repertoire ))
                    {
                        return 
                            Directory.GetFiles(
                            Repertoire, Pattern  , 
                            SearchOption.AllDirectories).Length;;
                    }
            else
                return 0;
            }
        
        public static String  DateHeure()
            {
                return DateTime.Now.ToLongDateString() + " " +  
                   DateTime.Now.ToLongTimeString();
            }

    } // class
} // namespace
