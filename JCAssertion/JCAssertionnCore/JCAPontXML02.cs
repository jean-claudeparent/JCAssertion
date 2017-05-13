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



namespace JCAssertionCore
{
    public partial class JCAPontXML
    {
        JCAXML monJCAXML = new JCAXML();

        
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


        /// <summary>
        /// Retourne le texte d'une balise XML
        /// ou une chaîne vide si on ne trouve pas la balise
        /// </summary>
        /// <param name="monXMLNode">Un document xml ou une autre structure XML qui peuut contenir la balise recherchée</param>
        /// <param name="maBalise">Balise à rechercher</param>
        /// <returns>La valeur de la balise ou ""</returns>

    }
}
