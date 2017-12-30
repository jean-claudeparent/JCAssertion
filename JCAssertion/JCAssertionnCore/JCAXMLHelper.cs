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
using System.Xml;

using System.Threading.Tasks;

namespace JCAssertionCore
{
    public class JCAXMLHelper
    {
        /// <summary>
        /// Retourne une string qui contient
        /// le XML de l'assertion ConnectionOracle
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Password"></param>
        /// <param name="Serveur"></param>
        /// <param name="Action"></param>
        /// <param name="Cache"></param>
        /// <returns>Document xml dans une string</returns>
        public string xmlConnectionOracle(
            String User,
            String Password,
            String Serveur = null,
            String Action = null,
            String Cache = null)
        {
            String Resultat = "";
            Resultat = "<Assertion>" 
                + Environment.NewLine +
               "<Type>ConnectionOracle</Type>"
               + Environment.NewLine;

            if (User != null)
                Resultat =
                    Resultat
                    + "<User>" + User
                    + "</User>" + Environment.NewLine;

            if (Password != null)
                Resultat =
                    Resultat
                    + "<Password>" +
                    Password + "</Password>" + Environment.NewLine;
            if (Serveur != null)
                Resultat =
                    Resultat
                    + "<Serveur>" + Serveur
                    + "</Serveur>" + Environment.NewLine;

            if (Action != null)
                Resultat =
                    Resultat
                    + "<Action>" +
                    Action + "</Action>" 
                    + Environment.NewLine;

            if (Cache != null)
                Resultat =
                    Resultat
                    + "<Cache>" +
                    Cache + "</Cache>"
                    + Environment.NewLine;

            Resultat = Resultat +
                "</Assertion>";

            return Resultat;

        }

        public string xmlAssertSQL(
            String SQL,
            String Operateur,
            String AttenduNombre,
            String AttenduTexte = null,
            String MessageEchec = null)
        {
            String Resultat = "";
            Resultat = "<Assertion>"
                + Environment.NewLine +
               "<Type>AssertSQL</Type>"
               + Environment.NewLine;

            if (SQL != null)
                Resultat =
                    Resultat
                    + "<SQL>" + SQL
                    + "</SQL>" + Environment.NewLine;

            if (Operateur  != null)
                Resultat =
                    Resultat
                    + "<Operateur>" +
                    Operateur + "</Operateur>" + Environment.NewLine;
            if (AttenduNombre != null)
                Resultat =
                    Resultat
                    + "<AttenduNombre>" + AttenduNombre
                    + "</AttenduNombre>" + Environment.NewLine;

            if (AttenduTexte  != null)
                Resultat =
                    Resultat
                    + "<AttenduTexte>" +
                    AttenduTexte + "</AttenduTexte>"
                    + Environment.NewLine;

            if (MessageEchec  != null)
                Resultat =
                    Resultat
                    + "<MessageEchec>" +
                    MessageEchec + "</MessageEchec>"
                    + Environment.NewLine;

            Resultat = Resultat +
                "</Assertion>";

            return Resultat;

        }



    }
}
