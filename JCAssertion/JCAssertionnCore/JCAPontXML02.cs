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
        JCAXML monJCAXML = new JCAXML();
        JCACompare monCompare = new JCACompare(); 

        /// <summary>
        /// Évalue une assertion basée
        /// sur la sélection de noeuds dans un document xml.
        /// Les noeuds sont choisis avec une expression XPath
        /// et optionnellement avec une recherche  de texte
        /// </summary>
        /// <param name="monXMLNode">Assertion définie dans un ocument xml</param>
        /// <param name="Message">Message donnant de l'information sur le traitement de l'assertion</param>
        /// <param name="Variables">Dictionnaire de variables et leur valeurs</param>
        /// <param name="MessageEchec">Message donnant de l'information lorsuqe l'assertion est fausse</param>
        /// <returns>Si l'assertion est vraie ou fausse</returns>
        public bool JCAAssertXPath(XmlNode monXMLNode,
            ref string Message,
            ref  Dictionary<String, String> Variables,
            ref string MessageEchec)
        {
            Message = "Assertion AssertXPath" +
                Environment.NewLine;
            
            MessageEchec = "";

            if (monXMLNode == null)
                throw new JCAssertionException("Le XML est vide.");
            ValideBalise(monXMLNode, "Fichier");
            ValideBalise(monXMLNode, "Expression");
            // Valeur par défaut pour le résultat attendu

            
            Int64 monResultatAttendu = 0;
            
            // Continuer le traitement
            // Message d'échec
            String  monMessageEchec = ValeurBalise(monXMLNode, "MessageEchec");
            monMessageEchec = JCAVariable.SubstituerVariables(
                    monMessageEchec, Variables);

            // Fichier
            String monFichier = ValeurBalise(monXMLNode, "Fichier");
            monFichier = JCAVariable.SubstituerVariables(
                    monFichier, Variables);
            if (monFichier == "")
                throw new JCAssertionException("Le fichier n'a pa été spécifié corrrectemeent");

            // Resu;tat Attendu
            String monResultatStr = ValeurBalise(monXMLNode, "ResultatAttendu");
            monResultatStr = JCAVariable.SubstituerVariables(
                    monResultatStr, Variables);
            if (monResultatStr == "")
                monResultatStr = "0";
            try 
                {
                    monResultatAttendu = Convert.ToInt64(monResultatStr);
                
                } catch (Exception excep)
                {
                    MessageEchec = "";
                    throw new JCAssertionException(
                        "Impossible de convertir le "+
                    "résultat attendu '"+
                    monResultatStr +
                    "' en un nombre. Détail: "+
                    excep.Message  ); 
                }
            // Operateur
            String monOperateur =  ValeurBalise(monXMLNode, "Operateur");
            monOperateur = JCAVariable.SubstituerVariables(
                    monOperateur, Variables);
            if (monOperateur == "")
                monOperateur = "PG";
            // Expression
            String monExpression = ValeurBalise(monXMLNode, "Expression");
            monExpression = JCAVariable.SubstituerVariables(
                    monExpression, Variables);
            // Traiter Contient et ContientMajus
            bool monSensibleCase = true;
            String monContient = "";
            monContient = ValeurBalise(monXMLNode, "ContientMajus");
            monContient = JCAVariable.SubstituerVariables(
                    monContient, Variables);
            if (monContient != "")
                monSensibleCase = false;
            else
                {
                    monContient = ValeurBalise(monXMLNode, "Contient");
                    monContient = JCAVariable.SubstituerVariables(
                            monContient, Variables);
                }
            // Construire le message
            Message = Message + Environment.NewLine +
                "Fichier XML à traiter : " + monFichier ;
            Message = Message + Environment.NewLine +
                "Expression XPath : " + monExpression;
            if (monContient != "")
                {
                Message = Message + Environment.NewLine +
                    "Chercher les noeuds qui contiennent : " +
                    monContient + Environment.NewLine;
                if (!monSensibleCase)
                    Message = Message + 
                        "Comparer en ne tenant pas compte des majuscules et minuscules";
                }
            // appeler la métgode
            Int64 ResultatReel = 0;
            Boolean Resultat = false;
            if (monContient == "")
                Resultat = monJCAXML.AssertXPath(monFichier ,
                    monExpression, monOperateur,
                    monResultatAttendu,ref ResultatReel);
            else
                Resultat = monJCAXML.AssertXPath(monFichier,
                    monExpression, monOperateur,
                    monResultatAttendu, ref ResultatReel,
                    monContient  ,monSensibleCase );
            
            // Finir le message
            Message = Message + Environment.NewLine +
                "Assertion : " +
                Convert.ToString(ResultatReel) + " (Réel) " +
                monOperateur + " " +
                    Convert.ToString(monResultatAttendu) +
                    " (Attendu)";
            // traiter l'échec
            if (! Resultat)
                {
                    MessageEchec = monMessageEchec;
                    
                }
            Message = Message + Environment.NewLine;
 

            return Resultat;
        }

        public bool JCACompteFichiers(XmlNode monXMLNode,
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
                monXMLNode, "Repertoire",
                Variables,
                "",
                true, true);
            monRepertoire = JCAVariable.SubstituerVariables(
                    monRepertoire, Variables);
            
            // Resu;tat Attendu
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
            Resultat =  AssertCF(
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
        /// comptés dans un répertoire
        /// </summary>
        /// <param name="Repertoire">Répertoire où compter les fichiers</param>
        /// <param name="Patterm">Pattern des fichiers à compter, par exemple "*.xml"</param>
        /// <param name="Operateur">Operateur de comparaison entre le résultat réel des fichiers comptés et le résultat attendu, par exemple pg pour plus grand</param>
        /// <param name="ResultatAttendu">Résultat à comparer au résultat réel avec l'opérateur</param>
        /// <param name="ResultatReel">Retourne le nombre de fichiers comptés avec le pattern</param>
        /// <returns>Si l'assertion est vraie ou fausse</returns>
        public Boolean AssertCF(
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


        /// <summary>
        /// Retourne la valeur d'une balise
        /// avec substitutionde variables
        /// Optionnellement initialise avec une valeur
        /// par défaut et fait des validations
        /// </summary>
        /// <param name="xmlNode">XML d'où extraire la valeur de la balise</param>
        /// <param name="Balise">Balise à extraire</param>
        /// <param name="Variables">Dictionnaire de variables pour ébventuellementcompléter la valeur</param>
        /// <param name="ValeurParDefaut">Valeur par défaut si la balise est vide ou inexistante</param>
        /// <param name="Obligatoire">Indique de faire un exception si la balise n'est pas dans le xml</param>
        /// <param name="ExceptionSiVide">Indique de faire une exception  si la baleur trouvée est une cha¸ine vide</param>
        /// <returns>Valeur de la balise</returns>
        public String TraiterBalise (
            XmlNode  xmlNode,
            String Balise,
            Dictionary<String, String> Variables,
            String ValeurParDefaut = "",
            Boolean Obligatoire = false ,
            Boolean ExceptionSiVide = false 
            )
            {
                if (Obligatoire)
                    ValideBalise(xmlNode, Balise);
 
            String Resultat = ValeurBalise(xmlNode , Balise);
            Resultat  = JCAVariable.SubstituerVariables(
                    Resultat , Variables);
            if ((Resultat  == "") &&
                (ExceptionSiVide))
                throw new JCAssertionException(
                    "La balise <" +
                    Balise +
              "> n'a pa été spécifié correctement");
            if (Resultat == "")
                Resultat = ValeurParDefaut;
            
            return Resultat;     
        }




               
    }
}
