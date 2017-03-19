// License
//
//
//  JCAssertion : Un ensemble d’outils
//              pour configurer et vérifier les environnements 
//              de tests sous windows.
//
//  Copyright 2016,2017 Jean-Claude Parent 
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
        /// Met le contenu d'un fichier dans
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
                String monTypeLoB = "";
            if (NBRangees > 0)
                {
                for (Int32 i = 1; i <= NBRangees; i++)
                {
                    // Déterminer si BLOB, CLOB ou erreur
                    monTypeLoB = TypeLOB(BD.Tables[0].Rows[i-1]);
                    // Traiter selon le type
                    switch (monTypeLoB)
                {

                    case "BLOB":
                        BD.Tables[0].Rows[i-1]["BLOB"] =
                            LireFichierBinaire(Fichier );
                        break;
                    case "CLOB":
                        BD.Tables[0].Rows[i-1]["CLOB"] =
                            LireFichierTexte(Fichier);
                        break;
                    default:
                        throw new JCASQLODPException(
                            "Il n'y a aucune colonne identifiée par un alias BLOB ou CLOB dans la commande SQL");
                        
                } // switch
            } // for
                } // end if
                return NBRangees;

            } 

        
        /// <summary>
        /// NomColonne : retourne la nom de la colonne préfére
        /// si elle existe sinon retourner le nom de la première colonne
        /// </summary>
        /// <param name="Rangee">Rangée d`une table d`un dataset à analyser</param>
        /// <param name="ColonnePreferee">Colonne `utiliser si elle existe</param>
        /// <returns>Nom de colonne déterminée</returns>
        private String NomColonne(
            DataRow Rangee,
            String ColonnePreferee)
            {
                String Resultat = "";
                foreach (DataColumn maColonne  in Rangee.Table.Columns  )
                {
                    if (Resultat == "")
                        Resultat = maColonne.ColumnName;
                    if (maColonne.ColumnName == ColonnePreferee)
                        Resultat = maColonne.ColumnName;
                                
                }
                return Resultat ;
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
            String Resultat = "";
                
            foreach (DataColumn maColonne  in Rangee.Table.Columns  )
            {
                if (maColonne.ColumnName == "BLOB")
                    Resultat = "BLOB";
                if (maColonne.ColumnName ==  "CLOB")
                    Resultat = "CLOB";
            }
                return Resultat;
            } // TypeLOB

        /// <summary>
        /// Convertir un objet en Byte Array
        /// Le type de l'objet doit avoir une méthode
        /// toString() pour que ¸a marche bien.
        /// Le seul type vraiemnt sécuritaire à
        /// utiliser est Byte[]
        /// soit que l'objet est déjàen byte array
        /// Les autres types sont semis supportés pour voir ce qui se passe 
        /// deans les assertions
        /// Les caractères spéciaux des chaîne de caractères
        /// peuvent aussi être problématiques.
        /// </summary>
        /// <param name="Valeur">Objet à transformer en byte array</param>
        /// <returns></returns>
        public Byte[] ConvertirByteArray(
            object Valeur)
            {
                String TypeValeur = Valeur.GetType().ToString();
                switch (TypeValeur )
                    {
                    case "System.DBNull":
                        return null;
                    case "System.Byte[]":
                        return (Byte[])Valeur;
                    case "System.String":
                        {
                            String maValeur = Valeur.ToString() ;
                            return ConvertirByteArrayStr(maValeur );
                        }
                    default:
                        return ConvertirByteArrayStr(Valeur.ToString()  );
 
 
                    }

                     
                
                   
                 
            }

        /// <summary>
        /// Encode les string en ascii
        /// pour les transformer en byte array
        /// </summary>
        /// <param name="Valeur"></param>
        /// <returns></returns>
        private Byte[] ConvertirByteArrayStr(
            String  Valeur)
            {
                Encoding monEncoding = Encoding.ASCII;
 
                if (Valeur == null)
                    return null ;
                else
                    return monEncoding.GetBytes(Valeur);
            }

        


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
            /// Écrit un fichier binaire
            /// </summary>
            /// <param name="Fichier">Nom du fichier</param>
            /// <param name="Contenu">Contenu du fichier à écrire</param>
            private void  EcrireFichierBinaire(String Fichier,
                Byte[] Contenu)
            {
                if (Contenu == null)
                    throw new JCASQLODPException("Aucun contenu passé pour le fichier "+
                Fichier );

                System.IO.File.WriteAllBytes(Fichier,Contenu);
            }
            
            /// <summary>
            /// Écrit un fichier texte avec le bon encodage.
            /// Peut lancer une exception
            /// </summary>
            /// <param name="Fichier">Nom du fichier à écrire</param>
            /// <param name="Contenu">Contenu textuel à écrire</param>
            /// <param name="EncodageFichier">Encodega eu fichier texte</param>
            private void EcrireFichierTexte(String Fichier,
                String  Contenu,
                Encoding EncodageFichier )
            {
                if (EncodageFichier == null)
                    EncodageFichier = Encoding.UTF8;
 
                System.IO.File.WriteAllText( Fichier, Contenu, EncodageFichier );
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

        /// <summary>
        /// Cette fonction ne devrait plus être utilisé dans une
        /// release car elle sert à débugger
        /// </summary>
        /// <param name="monDS"></param>
        public void ExploreDataset (DataSet  BD)
            {
                Int32 NBRangees = BD.Tables[0].Rows.Count;
                String  nomColonne = "";
                // throw new Exception(
                //    BD.Tables[0].Columns[0].ToString() +
                //    BD.Tables[0].Columns[0].Unique.ToString()  );
            for (Int32 i = (NBRangees - 1); (i < NBRangees) && (NBRangees > 0); i++)
                {
                    System.IO.File.WriteAllText("d:\\Devcenter\\dataset.xml",
                        BD.GetXml())  ;
                    System.IO.File.WriteAllText("d:\\Devcenter\\shemadataset.xml",
                            BD.GetXmlSchema());
                    
                    nomColonne = TypeLOB(BD.Tables[0].Rows[i]);

                    //if (BD.Tables[0].Rows[i][nomColonne] == null)
                      //  throw new Exception(
                        //    "Explore dataset la colonne " +
                          //  nomColonne + " est à null"); 
                }
            }



        
        
        /// <summary>
        /// Validation de bases sur un nom de fichier
        /// Lance une exception en cas d'échec de la validation
        /// </summary>
        /// <param name="Fichier"></param>
        public void ValideFichier(String Fichier)
            {
            if (Fichier == null )
                throw new JCASQLODPException(
                    "Le nom de fichier doit contenir un nom de fichier");


            if (Fichier == "")
                throw new JCASQLODPException(
                    "Le nom de fichier doit contenir un nom de fichier");

            

            if (!System.IO.File.Exists(Fichier))
                throw new JCASQLODPException(
                    "Le fichier à charger sur la base de données n'existe pas ou est invalide "+
                    "Nom du fichier : " +
            Fichier );
            }

        /// <summary>
        /// Exporte les LOB du dataset
        /// </summary>
        /// <param name="monDS">Dataset ¸a traiter</param>
        /// <param name="Chemin">Chemin du répertoire o``u déposer les fichiers</param>
        /// <param name="TypeEncodage">Type d'encodage des fuichiers textes</param>
        /// <param name="ListeFichier">Liste des fichiers écrits</param>
        /// <returns>Nombre de fichiers écrits</returns>
        public Int32 ExporteLOB(
            DataSet  monDS,
            String Chemin,
            Encoding TypeEncodage,
            ref String ListeFichier)
            {
                ListeFichier = "Aucun LOB à extraire";
                Int32 NBRangees = monDS.Tables[0].Rows.Count;
                Int32 NbFichierExportes = 0;
                String ColonneNomFichier = "";
                if (NBRangees == 0)
                    ColonneNomFichier = "";
                else
                    ColonneNomFichier = NomColonne(
                    monDS.Tables[0].Rows[0], "NOM");

                ListeFichier = "Noms de fichier provenant de la colonne "+
                    ColonneNomFichier + Environment.NewLine;
                String monTypeLOB = "";
                if (NBRangees > 0 )
                    {
                        monTypeLOB = TypeLOB(monDS.Tables[0].Rows[0]);
                        if (monTypeLOB == "")
                            throw new JCASQLODPException(
                            "Il n'y a aucune colonne identifiée par un alias BLOB ou CLOB dans la commande SQL");
                    }
                    for (Int32 i = 1; (i <= (NBRangees)) 
                    && (NBRangees > 0); i++)
                {
                    if (monDS.Tables[0].Rows[i - 1][monTypeLOB] != DBNull.Value   )
                        {
                            NbFichierExportes = NbFichierExportes + 1;
                        ListeFichier = ListeFichier +
                        Chemin +
                        monDS.Tables[0].Rows[i - 1][ColonneNomFichier] +
                        Environment.NewLine  ;
                        if (monTypeLOB == "CLOB")
                            EcrireFichierTexte(
                            Chemin +
                            monDS.Tables[0].Rows[i - 1][ColonneNomFichier],
                            monDS.Tables[0].Rows[i - 1][monTypeLOB].ToString(),
                            TypeEncodage);
                        if (monTypeLOB == "BLOB")
                            EcrireFichierBinaire(
                            Chemin +
                            monDS.Tables[0].Rows[i - 1][ColonneNomFichier],
                            ConvertirByteArray (
                            monDS.Tables[0].Rows[i - 1][monTypeLOB]));
                        } // endif
                } // end for
                return NbFichierExportes;
            }




    }  // class
} // namespace
