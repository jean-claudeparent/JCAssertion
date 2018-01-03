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
using System.Text;
using System.Xml;
using System.IO;
using JCAssertionCore;
using JCAMC;



namespace JCAssertionCore
{
    public partial class JCAPontXML
    {
        
        /// <summary>
        /// Cette méthode évalue une assertion
        /// de type CompteFichiers
        /// définie par im document xml.
        /// </summary>
        /// <param name="monXMLNode">Assertion définie en xml</param>
        /// <param name="Message">Message qui retournera de l'information sur 
        /// l'évaluation de l'assertion</param>
        /// <param name="Variables">Dictionnaire des valeurs de variables</param>
        /// <param name="MessageEchec">Information supplémentaire
        /// si l'assertion est fausse</param>
        /// <returns>true si l'assertion est vraie, 
        /// false autrement. Peutt lancr des exceptions</returns>
        public bool JCACompteFichiers(
            XmlNode monXMLNode,
            ref string Message,
            ref  Dictionary<String, String> Variables,
            ref string MessageEchec)
        {
            Message = "Assertion CompteFichiers" +
                Environment.NewLine;

            MessageEchec = "";

            if (monXMLNode == null)
                throw new JCAssertionException("Le XML est vide.");
            //ValideBalise(monXMLNode, "Repertoire");
            // Valeur par défaut pour le résultat attendu


            Int64 monResultatAttendu = 0;

            // Continuer le traitement
            // Message d'échec
            String monMessageEchec = ValeurBalise(monXMLNode, "MessageEchec");
            monMessageEchec = JCAVariable.SubstituerVariables(
                    monMessageEchec, Variables);

            // Repertoire
            String monRepertoire = TraiterBalise(
                monXMLNode, 
                "Repertoire",
                Variables,
                "",
                true, true);
            monRepertoire = JCAVariable.SubstituerVariables(
                    monRepertoire, Variables);
            
            // Resultat Attendu
            String monResultatStr = ValeurBalise(monXMLNode, "ResultatAttendu");
            monResultatStr = JCAVariable.SubstituerVariables(
                    monResultatStr, Variables);
            if (monResultatStr == "")
                monResultatStr = "0";
            try
            {
                monResultatAttendu = Convert.ToInt64(monResultatStr);

            }
            catch (Exception excep)
            {
                MessageEchec = "";
                throw new JCAssertionException(
                    "Impossible de convertir le " +
                "résultat attendu '" +
                monResultatStr +
                "' en un nombre. Détail: " +
                excep.Message);
            }
            // traiter pattern
            String monPattern = TraiterBalise(
            monXMLNode,
            "Pattern",
            Variables,
            "*.*");

            // Operateur
            String monOperateur = ValeurBalise(monXMLNode, "Operateur");
            monOperateur = JCAVariable.SubstituerVariables(
                    monOperateur, Variables);
            if (monOperateur == "")
                monOperateur = "PG";
                
            // Construire le message
            Message = Message + Environment.NewLine +
                "Répertoire à traiter : " + monRepertoire +
                Environment.NewLine +
                "Pattern des fichiers à compter : "+
                monPattern ;
            
            // appeler la méthode
            Int64 ResultatReel = 0;
            Boolean Resultat = false;
            Resultat =  AssertCompteFichiers(
            monRepertoire,
            monPattern,
            monOperateur,
            monResultatAttendu,
            ref ResultatReel);

            // Finir le message
            Message = Message + Environment.NewLine +
                "Assertion : " +
                Convert.ToString(ResultatReel) + " (Réel) " +
                monOperateur + " " +
                    Convert.ToString(monResultatAttendu) +
                    " (Attendu)" +
                    Environment.NewLine  ;
            // traiter l'échec
            if (!Resultat)
            {
                MessageEchec = monMessageEchec;

            }
            Message = Message + Environment.NewLine;


            return Resultat;
        }

        /// <summary>
        /// Évalue une assertion sur le nombre de fichiers
        /// comptés dans un répertoire. Cette méthode
        /// est utilisée en passant des paramètres
        /// du bon type et non pas
        /// un document xml définisssant l'assertion.
        /// </summary>
        /// <param name="Repertoire">Répertoire où compter les fichiers</param>
        /// <param name="Patterm">Pattern des fichiers à compter, par exemple "*.xml"</param>
        /// <param name="Operateur">Operateur de comparaison entre le résultat réel des fichiers comptés et le résultat attendu, par exemple pg pour plus grand</param>
        /// <param name="ResultatAttendu">Résultat à comparer au résultat réel avec l'opérateur</param>
        /// <param name="ResultatReel">Retourne le nombre de fichiers comptés avec le pattern</param>
        /// <returns>Si l'assertion est vraie ou fausse</returns>
        public Boolean AssertCompteFichiers(
            String Repertoire,
            String Patterm,
            String Operateur,
            Int64 ResultatAttendu,
            ref Int64 ResultatReel)
            {
                ResultatReel =
                    JCAMiniCore.CompteFichiers(
                    Repertoire, Patterm  );

                Boolean Resultat =
                    monCompare.Compare(
                    ResultatReel,
                    Operateur,
                    ResultatAttendu);
                return Resultat; 
            }


        



               
    }
}
