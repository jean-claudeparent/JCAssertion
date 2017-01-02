// License
//
//
//  JCAssertion : Un ensemble d’outils
//              pour configurer et vérifier les environnements 
//              de tests sous windows.
//
//  Copyright 2016 Jean-Claude Parent 
// 
//  Informations : www.jcassertion.org
//
// Ce fichier fait partie de JCAssertion.
//
// JCAssertion  est un logiciel libre ; vous pouvez le redistribuer ou le 
// modifier suivant les termes de la GNU General Public License telle que 
// publiée par la Free Software Foundation ; soit la version 2 de la 
// licence, soit (à votre gré) toute version ultérieure.
// 
// JCAssertion est distribué dans l'espoir qu'il sera utile, 
// mais SANS AUCUNE GARANTIE ; sans même la garantie tacite 
// de QUALITÉ MARCHANDE ou d'ADÉQUATION à UN BUT PARTICULIER. 
// Consultez la GNU General Public License pour plus de détails.
// 
// Vous devez avoir reçu une copie de la GNU General Public License 
// en même temps que JCAssertion ; si ce n'est pas le cas, consultez
// <http://www.gnu.org/licenses>. Selon la recommandation  de la fondation 
// le seul texte officiel est celui en anglais car la fondation ne peut garantir
// que la traduction dans une langue  assurera les mêmes protections.
// 
// License
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;


namespace JCASQLODPCore
{
    public class JCASQLODPHelper
    {
        /// <summary>
        /// Net le contenu dMun fichier dans
        /// une des colonnes d'un dataset.
        /// Le dataset doit avoir une colonne
        /// nommée BLOB ou une colonne nommée CLOAB.
        /// Ceci est accompli en mettant un alias de colonne
        /// dans la commande SQL qui crée le dataset.
        /// </summary>
        /// <param name="BD">Le dataset à modifier passé par référence</param>
        /// <param name="Fichier">Le nom du fichier dont el contenu sera mis dans le dataset. Avec le chemin complet.</param>
        /// <returns>Le nombre de rangées du dataset modifiées.</returns>
        public Int32 MAJLOB(ref System.Data.DataSet BD,
            String Fichier)
            {
                
                Int32 NBRangees = BD.Tables[0].Rows.Count;
                for (Int32 i = (NBRangees - 1); (i < NBRangees) && (NBRangees > 0); i++)
                {
                    // Déterminer si BLOB, CLOB ou erreur
                    String monTypeLoB = TypeLOB(BD.Tables[0].Rows[i]);
                    // Traiter selon le type
                    switch (monTypeLoB)
                {

                    case "BLOB":
                        BD.Tables[0].Rows[i]["BLOB"] =
                            LireFichierBinaire(Fichier );
                        break;
                    case "CLOB":
                        BD.Tables[0].Rows[i]["BLOB"] =
                            LireFichierTexte(Fichier);
                        break;
                    default:
                        throw new JCASQLODPException(
                            "Il n'y a aucune colonne identifiée par un alias BLOB ou CLOB dans la commande SQL");
                        break;
                } // switch
            } // for
                return NBRangees;

            } 

        /// <summary>
        /// Retourne un des alias oracle permis dans une requête
        /// sql (BLOB ou CLOB) qio a servi `sélectionner
        /// la rangée de données. Cet alias se retrouve comme nom de colonne
        /// </summary>
        /// <param name="Rangee">Rangée dMun dataset qu'il faut typer avec l'alias</param>
        /// <returns>BLOB, CLOB ou inconnu</returns>
        private String TypeLOB(DataRow Rangee)
            {
            String Resultat = "Inconnu";
                
            foreach (DataColumn maColonne  in Rangee.Table.Columns  )
            {
                if (maColonne.ColumnName == "BLOB")
                    Resultat = "BLOB";
                if (maColonne.ColumnName == "CLOB")
                    Resultat = "CLOB";
            }
                return Resultat;
            } // TypeLOB


            /// <summary>
            /// Lire un fichier binaire
            /// </summary>
            /// <param name="Fichier">Nom du fichier avec chenin complet</param>
            /// <returns>Contenu du fichier en array de byte</returns>
            private Byte[] LireFichierBinaire(String Fichier)
                {
                    return System.IO.File.ReadAllBytes(Fichier);   
                }


            /// <summary>
            /// Lire le contenu d'unfichier texte
            /// </summary>
            /// <param name="Fichier">Nom du fichier avec chenin complet</param>
            /// <returns>Contenu du fichier</returns>
            private String  LireFichierTexte(String Fichier)
            {
                return System.IO.File.ReadAllText(Fichier);
            }


    }  // class
} // namespace
