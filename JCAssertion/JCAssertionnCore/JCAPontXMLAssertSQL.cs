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
using System.Xml;


namespace JCAssertionCore
{
    public partial class JCAPontXML
    {

        public bool JCAAssertSQL(XmlNode monXMLNode,
            ref string Message,
            ref Dictionary<String, String> Variables,
            ref string MessageEchec,
            ref JCASQLClient monODPSQLClient)
        {
            

            // Initialisations

            Message = Message + Environment.NewLine +
            "Assertion AssertSQL" + Environment.NewLine;
            MessageEchec = "";

            // Validations de base
            if (monXMLNode == null)
                throw new JCAssertionException("Le XML est vide.");
            ValideBalise(monXMLNode, "SQL");
            if ((!SiBaliseExiste(monXMLNode, "AttenduNombre")) &&
                (!SiBaliseExiste(monXMLNode, "AttenduTexte")))
                throw new JCAssertionException(
                    "Une des deux balises suivantes doit exister :AttenduNombre ou AttenduTexte ");
            if ((SiBaliseExiste(monXMLNode, "AttenduNombre")) &&
                (SiBaliseExiste(monXMLNode, "AttenduTexte")))
                throw new JCAssertionException(
                    "Une seule des deux balises suivantes doit exister dans le xml :AttenduNombre ou AttenduTexte ");
            // les validations de bases sont passées

            String monSQL = ValeurBalise(monXMLNode, "SQL");
            monSQL = JCAVariable.SubstituerVariables(
                    monSQL, Variables);

            Message = Message + monSQL + Environment.NewLine;
            Boolean Resultat = false;
            String monOperateur = "";
            if (SiBaliseExiste(monXMLNode, "AttenduNombre"))
            {
                monOperateur = ValeurBalise(monXMLNode, "Operateur");
                if (monOperateur == "")
                    monOperateur = "=";
                monOperateur = JCAVariable.SubstituerVariables(
                monOperateur, Variables);

                Message = Message + "Opérateur : " +
                    monOperateur + Environment.NewLine;
                Double ResultatAttendu = 0;
                String monANString = "";
                try
                {
                    monANString = 
                        ValeurBalise(
                        monXMLNode, "AttenduNombre");
                    monANString = JCAVariable.SubstituerVariables(
                    monANString, Variables);
                    MAJVariable(ref Variables,
                         "JCA.ValeurAttendue",
                        monANString);

                    ResultatAttendu = Convert.ToDouble(monANString);

                }
                catch (FormatException excep)
                {
                    throw new JCAssertionException(
                        "La balise AttenduNombre comporte une valeur (" +
                        monANString + ") ne pouvant pas être convertie en nombre" +
                        monXMLNode.InnerXml +
                        Environment.NewLine +
                        excep.Message, excep);
                }

                Message = Message +
                    "Valeur attendue : " +
                    ResultatAttendu.ToString() +
                    Environment.NewLine;
                Resultat = monODPSQLClient.SQLAssert(monSQL,
                        ResultatAttendu, monOperateur);
                MAJVariable(ref Variables,
                    "JCA.ValeurReelle",
                    monODPSQLClient.DernierResultat); 

                if (!(Resultat))
                    Message = Message +
                    "Valeur réelle : " +
                     monODPSQLClient.DernierResultat +
                    Environment.NewLine;
                // créer l'expressions
                MAJVariable(ref Variables,
                    "JCA.Expression",
                    FormaterExpression(
                        getValeurVariable(
                            Variables,
                            "JCA.ValeurReelle"),
                    monOperateur,
                    getValeurVariable(
                        Variables,
                        "JCA.ValeurAttendue")));
                        
                      
                MessageEchec = JCAVariable.SubstituerVariables(
                    ValeurBalise(
                    monXMLNode, "MessageEchec"), Variables);



            } // if
            else
            {
                // Le résultat attendu est une string

                String ResultatnAttenduTexte = ValeurBalise(
                        monXMLNode, "AttenduTexte");
                ResultatnAttenduTexte = JCAVariable.SubstituerVariables(
                    ResultatnAttenduTexte, Variables);
                MAJVariable(ref Variables,
                    "JCA.ValeurAttendue",
                    ResultatnAttenduTexte); 
                Message = Message +
                "Valeur attendue : " +
                ResultatnAttenduTexte +
                Environment.NewLine;
                Resultat = monODPSQLClient.SQLAssert(
                    monSQL,
                    ResultatnAttenduTexte);
                MAJVariable(ref Variables ,
                    "JCA.ValeurReelle",
                    monODPSQLClient.DernierResultat);

                MAJVariable(ref Variables,
                    "JCA.Expression",
                    FormaterExpression(
                        getValeurVariable(
                            Variables,
                            "JCA.ValeurReelle"),
                    "=",
                    getValeurVariable(
                        Variables,
                        "JCA.ValeurAttendue"),
                    true));


                if (!(Resultat))
                    MessageEchec = JCAVariable.SubstituerVariables(
                        ValeurBalise(
                        monXMLNode, "MessageEchec"), Variables);


            } // end else

            if (Resultat)
                Message = Message +
                    "L'expression évaluée est vraie" +
                    Environment.NewLine;
            else
                Message = Message +
                    "Valeur réelle : " +
                    getValeurVariable(
                            Variables,
                            "JCA.ValeurReelle") +
                    Environment.NewLine +
                    Environment.NewLine +
                    "L'expression évaluée est fausse" +
                    Environment.NewLine;


            return Resultat;
        }

    }
}
